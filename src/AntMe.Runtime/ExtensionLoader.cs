
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Xml.Serialization;

namespace AntMe.Runtime
{
    /// <summary>
    /// Statischer Handler zum Laden und Analysieren von Zusatzinhalten wie 
    /// Factions, GameItems und Levels.
    /// </summary>
    public static class ExtensionLoader
    {
        #region Lokaler Prozessraum

        private static bool extensionsLoaded = false;
        private static IEnumerable<IExtensionPack> extensionPackCache = null;
        private static IEnumerable<CampaignInfo> campaignCache = null;
        private static IEnumerable<LevelInfo> levelCache = null;
        private static IEnumerable<PlayerInfo> playerCache = null;
        private static IEnumerable<CodeGeneratorAttribute> codeGeneratorCache = null;
        private static Dictionary<Guid, PlayerStatistics> playerStatistics = new Dictionary<Guid, PlayerStatistics>();
        private static Dictionary<Guid, CampaignStatistics> campaignStatistics = new Dictionary<Guid, CampaignStatistics>();
        private static Dictionary<Guid, LevelStatistics> levelStatistics = new Dictionary<Guid, LevelStatistics>();
        private static TypeMapper typeMapper = new TypeMapper();

        /// <summary>
        /// Lädt alle verfügbaren Extensions initial zum Programmstart. 
        /// 
        /// Die Suchpfade des Scripts sind
        /// - Applicationsverzeichnis
        /// - Applikationspfad/Extensions
        /// - AppData/Local/AntMe/Extensions
        /// 
        /// Es werden folgende Extensions geladen
        /// - Factions (immer)
        /// - GameItems (immer)
        /// - Extenders (immer)
        /// - Campaigns (nur full)
        /// - Players (nur full)
        /// - Levels (nur full)
        /// - Generators (nur full)
        /// </summary>
        /// <param name="token">Erlaubt die Übergabe eines Progress/Cancelation-Token</param>
        /// <param name="full">Legt den Umfang des Ladeprozesses fest</param>
        /// <param name="signedOnly">Nur signierte Assemblies werden geladen</param>
        public static void LoadExtensions(ProgressToken token, bool full, bool signedOnly = true)
        {
            if (extensionsLoaded)
                return;

            List<Exception> errors = new List<Exception>();

            // Loader-Host aufbauen
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Evidence evidence = new Evidence();
            AppDomain appDomain = AppDomain.CreateDomain("AntMe! Analyzer", evidence, setup);

            Type hostType = typeof(ExtensionLoaderHost);
            ExtensionLoaderHost host = appDomain.CreateInstanceAndUnwrap(hostType.Assembly.FullName, hostType.FullName) as ExtensionLoaderHost;

            // Stammverzeichnis
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string file in Directory.EnumerateFiles(dir, "*.dll"))
            {
                try
                {
                    if (!signedOnly || host.AnalyseExtensionAssembly(File.ReadAllBytes(file)))
                        Assembly.LoadFile(file);
                }
                catch (Exception ex)
                {
                    // Ladefehler dokumentieren
                    if (token != null && token.Errors != null)
                        token.Errors.Add(ex);
                }

                // Cancel
                if (token != null && token.Cancel) return;
            }

            // Extension-Folder
            dir = dir + "\\Extensions";
            if (Directory.Exists(dir))
            {
                foreach (string file in Directory.EnumerateFiles(dir, "*.dll"))
                {
                    try
                    {
                        if (!signedOnly || host.AnalyseExtensionAssembly(File.ReadAllBytes(file)))
                            Assembly.LoadFile(file);
                    }
                    catch (Exception ex)
                    {
                        // Ladefehler dokumentieren
                        if (token != null && token.Errors != null)
                            token.Errors.Add(ex);
                        errors.Add(ex);
                    }

                    // Cancel
                    if (token != null && token.Cancel) return;
                }
            }

            // Extension Ordner (AppData/Local/AntMe/Extensions)
            dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            foreach (string file in Directory.EnumerateFiles(dir, "*.dll"))
            {
                try
                {
                    if (!signedOnly || host.AnalyseExtensionAssembly(File.ReadAllBytes(file)))
                        Assembly.LoadFile(file);
                }
                catch (Exception ex)
                {
                    // Ladefehler dokumentieren
                    if (token != null && token.Errors != null)
                        token.Errors.Add(ex);
                    errors.Add(ex);
                }

                // Cancel
                if (token != null && token.Cancel) return;
            }

            AppDomain.Unload(appDomain);

            // First Cancel-Point
            if (token != null && token.Cancel)
                return;

            // Anzahl Tasks ermitteln
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            int currentTask = 0;
            if (token != null)
            {
                token.TotalTasks = (full ? assemblies.Length * 2 : assemblies.Length) + 1;
                token.CurrentTask = currentTask;
            }

            // Pass 1 Load Extension Packs
            List<IExtensionPack> extensionPacks = new List<IExtensionPack>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.IsClass &&
                        !type.IsAbstract &&
                        typeof(IExtensionPack).IsAssignableFrom(type))
                    {
                        try
                        {
                            // Instanz erzeugen & Laden
                            var extensionPack = Activator.CreateInstance(type) as IExtensionPack;
                            extensionPack.Load(DefaultTypeMapper);
                            extensionPacks.Add(extensionPack);
                        }
                        catch (Exception ex)
                        {
                            // Error dokumentieren
                            if (token != null && token.Errors != null)
                                token.Errors.Add(ex);
                            errors.Add(ex);
                        }
                    }

                    currentTask++;

                    if (token != null)
                    {
                        token.CurrentTask = currentTask;
                        if (token.Cancel) return;
                    }
                }
            }

            extensionPackCache = extensionPacks;

            // Fill Caches

            if (full)
            {
                List<CampaignInfo> campaigns = new List<CampaignInfo>();
                List<LevelInfo> levels = new List<LevelInfo>();
                List<PlayerInfo> players = new List<PlayerInfo>();
                List<CodeGeneratorAttribute> generators = new List<CodeGeneratorAttribute>();

                // Pass 2 (Levels & Players) [generators, campaigns, levels, players]
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    LoaderInfo loader = AnalyseAssembly(assembly, true, true, true, true);
                    generators.AddRange(loader.CodeGenerators);

                    // Types mit File Dump
                    if (loader.Campaigns.Count > 0 ||
                        loader.Levels.Count > 0 ||
                        loader.Players.Count > 0)
                    {
                        // File dump
                        Stream stream = assembly.GetFiles()[0];
                        byte[] file = new byte[stream.Length];
                        stream.Read(file, 0, file.Length);

                        // Campaigns laden
                        foreach (var campaign in loader.Campaigns)
                        {
                            foreach (var level in campaign.Levels)
                                level.Type.AssemblyFile = file;

                            campaigns.Add(campaign);
                        }

                        // Levels laden
                        foreach (var level in loader.Levels)
                        {
                            level.Type.AssemblyFile = file;
                            levels.Add(level);
                        }

                        // Player laden
                        foreach (var player in loader.Players)
                        {
                            player.Type.AssemblyFile = file;
                            player.Source = PlayerSource.Native;
                            players.Add(player);
                        }
                    }

                    currentTask++;
                    if (token != null)
                    {
                        token.CurrentTask = currentTask;
                        if (token.Cancel) return;
                    }
                }

                campaignCache = campaigns;
                levelCache = levels;
                playerCache = players;
                codeGeneratorCache = generators;
            }

            extensionsLoaded = true;

            if (errors.Count > 0)
                throw new AggregateException("Extension Loader had some Exceptions", errors);
        }

        /// <summary>
        /// Lädt die statistischen Daten zu Campaigns, Levels und Playern
        /// </summary>
        public static void LoadStatistics()
        {
            // Statistics laden
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AntMe";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Levels
            try
            {
                byte[] cache = File.ReadAllBytes(dir + "\\Level.stats");
                using (MemoryStream stream = new MemoryStream(cache))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(LevelStatistics[]));
                    var result = serializer.Deserialize(stream) as LevelStatistics[];
                    if (result != null)
                    {
                        levelStatistics.Clear();
                        foreach (var item in result)
                            levelStatistics.Add(item.Guid, item);
                    }
                }
            }
            catch { }

            // Campaigns
            try
            {
                byte[] cache = File.ReadAllBytes(dir + "\\Campaign.stats");
                using (MemoryStream stream = new MemoryStream(cache))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CampaignStatistics[]));
                    var result = serializer.Deserialize(stream) as CampaignStatistics[];
                    if (result != null)
                    {
                        campaignStatistics.Clear();
                        foreach (var item in result)
                            campaignStatistics.Add(item.Guid, item);
                    }
                }
            }
            catch { }

            // Player
            try
            {
                byte[] cache = File.ReadAllBytes(dir + "\\Player.stats");
                using (MemoryStream stream = new MemoryStream(cache))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PlayerStatistics[]));
                    var result = serializer.Deserialize(stream) as PlayerStatistics[];
                    if (result != null)
                    {
                        playerStatistics.Clear();
                        foreach (var item in result)
                            playerStatistics.Add(item.Guid, item);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Speichert die gesammelten Statistik-Daten weg.
        /// </summary>
        public static void SaveStatistics()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AntMe";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Levels
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(LevelStatistics[]));
                    serializer.Serialize(stream, levelStatistics.Values.ToArray());

                    byte[] cache = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(cache, 0, cache.Length);

                    File.WriteAllBytes(dir + "\\Level.stats", cache);
                }
            }
            catch { }

            // Players
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PlayerStatistics[]));
                    serializer.Serialize(stream, playerStatistics.Values.ToArray());

                    byte[] cache = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(cache, 0, cache.Length);

                    File.WriteAllBytes(dir + "\\Player.stats", cache);
                }
            }
            catch { }

            // Campaigns
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CampaignStatistics[]));
                    serializer.Serialize(stream, campaignStatistics.Values.ToArray());

                    byte[] cache = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(cache, 0, cache.Length);

                    File.WriteAllBytes(dir + "\\Campaign.stats", cache);
                }
            }
            catch { }

        }

        /// <summary>
        /// Liefert eine Liste der geladenen ExtensionPacks zurück.
        /// </summary>
        public static IEnumerable<IExtensionPack> ExtensionPacks
        {
            get
            {
                if (extensionPackCache == null)
                    throw new Exception("Extensions are not loaded");

                return extensionPackCache;
            }
        }

        /// <summary>
        /// Liefert alle geladenen Kampagnen
        /// </summary>
        /// <returns>Geladene Kampagnen</returns>
        public static IEnumerable<CampaignInfo> Campaigns
        {
            get
            {
                if (campaignCache == null)
                    throw new Exception("Extensions are not loaded");

                return campaignCache;
            }
        }

        /// <summary>
        /// Liefert alle geladenen Levels inkl. dem Assembly Byte-Array.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LevelInfo> Levels
        {
            get
            {
                if (levelCache == null)
                    throw new Exception("Extensions are not loaded");
                return levelCache;
            }
        }

        /// <summary>
        /// Liefert alle geladenen Player inkl. dem Assembly Byte-Array.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PlayerInfo> Players
        {
            get
            {
                if (playerCache == null)
                    throw new Exception("Extensions are not loaded");

                return playerCache;
            }
        }

        /// <summary>
        /// Liefert alle geladenen Code Generatoren.
        /// </summary>
        public static IEnumerable<CodeGeneratorAttribute> CodeGenerators
        {
            get
            {
                if (codeGeneratorCache == null)
                    throw new Exception("Extensions are not loaded");

                return codeGeneratorCache;
            }
        }

        public static ITypeMapper DefaultTypeMapper { get { return typeMapper; } }

        public static ITypeResolver DefaultTypeResolver { get { return typeMapper; } }

        #endregion

        #region Neutrale Analyse Methoden

        /// <summary>
        /// Analysiert ein gegebenes Assembly auf brauchbare Klassen.
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>Gesammelte Infos der Assembly</returns>
        internal static LoaderInfo AnalyseAssembly(Assembly assembly, bool level, bool campaign, bool player, bool generator)
        {
            LoaderInfo loaderInfo = new LoaderInfo();
            bool isStatic = false;

            foreach (var type in assembly.GetTypes())
            {
                // Static-Flag abfragen
                // TODO: Abhängigkeiten in andere Assemblies müssen hier auch aufgegriffen werden
                var members = type.GetMembers(BindingFlags.Static);
                isStatic |= members.Length > 0;

                if (type.IsClass &&
                    type.IsPublic &&
                    !type.IsAbstract)
                {
                    // Found Level
                    if (level && type.IsSubclassOf(typeof(Level)))
                    {
                        try
                        {
                            LevelInfo levelInfo = AnalyseLevelType(type);
                            loaderInfo.Levels.Add(levelInfo);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }
                    }

                    // Found Campaign
                    if (campaign && type.IsSubclassOf(typeof(Campaign)))
                    {
                        try
                        {
                            CampaignInfo campaignInfo = AnalyseCampaignType(type);
                            loaderInfo.Campaigns.Add(campaignInfo);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }
                    }

                    // Found Player (Ignorieren, falls Faction-Liste null)
                    if (player && type.GetCustomAttributes(typeof(PlayerAttribute), true).Length > 0)
                    {
                        try
                        {
                            PlayerInfo playerInfo = AnalysePlayerType(type);
                            loaderInfo.Players.Add(playerInfo);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }
                    }

                    // Found Generator
                    if (generator && type.GetInterface("ICodeGenerator") != null && type.GetCustomAttributes(typeof(CodeGeneratorAttribute), false).Length > 0)
                    {
                        try
                        {
                            CodeGeneratorAttribute attribute = (CodeGeneratorAttribute)type.GetCustomAttributes(typeof(CodeGeneratorAttribute), false).First();
                            attribute.Generator = (ICodeGenerator)Activator.CreateInstance(type);
                            loaderInfo.CodeGenerators.Add(attribute);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }
                    }

                    // Found Extender
                    if (type.GetInterface("IExtender") != null)
                    {
                        Type[] interfaces = type.GetInterfaces();
                    }
                }
            }

            // Static Flag nachtragen
            foreach (var item in loaderInfo.Players)
                item.IsStatic = isStatic;

            return loaderInfo;
        }

        /// <summary>
        /// Liest alle notwendigen Informationen aus dem gegebenen Typ aus und 
        /// liefert das gesammelte LevelInfo.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Infos über das Level</returns>
        private static LevelInfo AnalyseLevelType(Type type)
        {
            LevelDescriptionAttribute[] descriptionAttributes =
                (LevelDescriptionAttribute[])type.GetCustomAttributes(
                typeof(LevelDescriptionAttribute), false);
            FactionFilterAttribute[] filterAttributes =
                (FactionFilterAttribute[])type.GetCustomAttributes(
                typeof(FactionFilterAttribute), false);

            // Kein oder zu viele Description Attributes
            if (descriptionAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid LevelDescription",
                    type.FullName,
                    type.Assembly.FullName));

            LevelInfo levelInfo = new LevelInfo();
            levelInfo.Type = TypeInfo.FromType(type);
            levelInfo.LevelDescription = descriptionAttributes[0];
            levelInfo.FactionFilter = new LevelFilterInfo[filterAttributes.Length];
            for (int i = 0; i < filterAttributes.Length; i++)
            {
                levelInfo.FactionFilter[i] = new LevelFilterInfo()
                {
                    PlayerIndex = filterAttributes[i].PlayerIndex,
                    Comment = filterAttributes[i].Comment,
                    Type = new TypeInfo()
                    {
                        AssemblyName = filterAttributes[i].FactionType.Assembly.FullName,
                        TypeName = filterAttributes[i].FactionType.FullName
                    }
                };
            }

            // Stats anhängen
            if (!levelStatistics.ContainsKey(levelInfo.LevelDescription.Id))
                levelStatistics.Add(levelInfo.LevelDescription.Id, new LevelStatistics() { Guid = levelInfo.LevelDescription.Id });
            levelInfo.Statistics = levelStatistics[levelInfo.LevelDescription.Id];

            return levelInfo;
        }

        /// <summary>
        /// Liest alle notwendigen Informationen aus dem gegebenen Typ aus und 
        /// liefert das gesammelte CampaignInfo.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Infos über die Campaign</returns>
        private static CampaignInfo AnalyseCampaignType(Type type)
        {
            CampaignDescriptionAttribute[] descriptionAttributes =
                (CampaignDescriptionAttribute[])type.GetCustomAttributes(
                typeof(CampaignDescriptionAttribute), false);

            // Kein oder zu viele Description Attributes
            if (descriptionAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid CampaignDescription",
                    type.FullName,
                    type.Assembly.FullName));

            Campaign campaign = Activator.CreateInstance(type) as Campaign;

            // Config setzen, falls vorhanden
            CampaignStatistics stats = null;
            if (campaignStatistics.TryGetValue(campaign.Guid, out stats))
                campaign.Settings = stats.Settings;

            CampaignInfo campaignInfo = new CampaignInfo();
            campaignInfo.Guid = campaign.Guid;
            campaignInfo.Name = campaign.Name;
            campaignInfo.Description = campaign.Description;
            campaignInfo.Picture = campaign.Picture;
            campaignInfo.Type = TypeInfo.FromType(type);
            campaignInfo.DescriptionAttribute = descriptionAttributes[0];

            // List all unlocked Levels
            foreach (var level in campaign.GetUnlockedLevels())
            {
                LevelInfo info = AnalyseLevelType(level);

                // Sicherstellen, dass Level-Anforderungen passen
                if (info.LevelDescription.MinPlayerCount > 1)
                    throw new Exception("Level is not playable alone");

                campaignInfo.Levels.Add(info);
            }

            // Stats anhängen
            if (!campaignStatistics.ContainsKey(campaign.Guid))
            {
                CampaignStatistics statistics = new CampaignStatistics()
                {
                    Guid = campaign.Guid,
                    Settings = campaign.Settings
                };
                campaignStatistics.Add(campaign.Guid, statistics);
            }
            campaignInfo.Statistics = campaignStatistics[campaign.Guid];

            return campaignInfo;
        }

        /// <summary>
        /// Liest alle notwendigen Informationen aus dem gegebenen Typ aus und 
        /// liefert das gesammelte PlayerInfo.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Infos über den Player</returns>
        private static PlayerInfo AnalysePlayerType(Type type)
        {
            PlayerAttribute[] playerAttributes =
                (PlayerAttribute[])type.GetCustomAttributes(typeof(PlayerAttribute), true);

            // Kein oder zu viele Description Attributes
            if (playerAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid PlayerAttribute",
                    type.FullName,
                    type.Assembly.FullName));

            // Property Mapping
            PlayerAttribute playerAttribute = playerAttributes[0];
            var mappings = playerAttribute.GetType().GetCustomAttributes(typeof(PlayerAttributeMappingAttribute), true);

            if (mappings.Length != 1)
                throw new NotSupportedException("The Player Attribute has no valid Property Mapping");

            var mapping = mappings[0] as PlayerAttributeMappingAttribute;
            Type playerType = playerAttribute.GetType();

            // Map Name
            PropertyInfo nameProperty = playerType.GetProperty(mapping.NameProperty);
            if (nameProperty == null)
                throw new NotSupportedException("The Name Property from Player Attribute Mapping does not exist");
            string name = (string)nameProperty.GetValue(playerAttribute, null);
            if (string.IsNullOrEmpty(name))
                throw new NotSupportedException("The Name of a Player can't be Empty");

            // Map Author
            PropertyInfo authorProperty = playerType.GetProperty(mapping.AuthorProperty);
            if (authorProperty == null)
                throw new NotSupportedException("The Author Property from Player Attribute Mapping does not exist");
            string author = (string)authorProperty.GetValue(playerAttribute, null);
            if (author == null)
                author = string.Empty;

            PlayerInfo playerInfo = new PlayerInfo()
            {
                Guid = type.GUID,
                Type = TypeInfo.FromType(type),
                Name = name,
                Author = author
            };

            // Faction finden
            // TODO: Richtiger lookup
            playerInfo.FactionType = "";

            // TODO: Stats anhängen
            // if (PlayerStatistics.ContainsKey(playerInfo.

            return playerInfo;
        }

        #endregion

        #region Externe AppDomain

        /// <summary>
        /// Analysiert die angegebene Datei, lädt sie aber nicht in den aktuellen Prozessraum.
        /// </summary>
        /// <param name="filename">Dateiname der Datei, die analysiert werden soll</param>
        /// <returns>Liste der gefundenen Spielelemente</returns>
        public static LoaderInfo SecureAnalyseExtension(string filename, bool level, bool player)
        {
            // Datei öffnen
            byte[] file = File.ReadAllBytes(filename);

            // Analyse
            return SecureAnalyseExtension(file, level, player);
        }

        /// <summary>
        /// Analysiert die angegebene Datei, lädt sie aber nicht in den aktuellen Prozessraum.
        /// </summary>
        /// <param name="file">Eingelesene Datei, die analysiert werden soll</param>
        /// <returns>Liste der gefundenen Spielelemente</returns>
        public static LoaderInfo SecureAnalyseExtension(byte[] file, bool level, bool player)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Evidence evidence = new Evidence();
            AppDomain appDomain = AppDomain.CreateDomain("AntMe! Analyzer", evidence, setup);

            Type hostType = typeof(ExtensionLoaderHost);

            ExtensionLoaderHost host = appDomain.CreateInstanceAndUnwrap(hostType.Assembly.FullName, hostType.FullName) as ExtensionLoaderHost;
            LoaderInfo info = host.AnalyseExtension(file, level, level, player);
            foreach (var item in info.Players)
                item.Source = PlayerSource.Imported;

            AppDomain.Unload(appDomain);

            return info;
        }

        /// <summary>
        /// Analysiert die mitgegebene Assembly und durchsucht sie nach dem angegebenen Level.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static LevelInfo SecureFindLevel(byte[] file, string typeName)
        {
            LoaderInfo info = ExtensionLoader.SecureAnalyseExtension(file, true, false);
            foreach (var level in info.Levels)
            {
                if (level.Type.TypeName.Equals(typeName))
                {
                    return level;
                }
            }
            return null;
        }

        /// <summary>
        /// Analysiert die mitgegebene Assembly und durchsucht sie nach dem angegebenen Player.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static PlayerInfo SecureFindPlayer(byte[] file, string typeName)
        {
            // TODO: Abhängigkeiten prüfen (wegen static und so)

            LoaderInfo info = ExtensionLoader.SecureAnalyseExtension(file, false, true);
            foreach (var player in info.Players)
            {
                if (player.Type.TypeName.Equals(typeName))
                {
                    return player;
                }
            }
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Token zur Übergabe in einen Task um den Prozess abzubrechen und den Fortschritt zu ermitteln.
    /// </summary>
    public class ProgressToken
    {
        /// <summary>
        /// Neue Instanz eines Tokens.
        /// </summary>
        public ProgressToken()
        {
            Errors = new List<Exception>();
        }

        /// <summary>
        /// Erlaubt den Abbruch von Außen.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gibt die Anzahl Arbeitsschritte an.
        /// </summary>
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gibt die Anzahl bereits erledigter Aufgaben an.
        /// </summary>
        public int CurrentTask { get; set; }

        /// <summary>
        /// Gibt eine Auflistung angefallener Fehler zurück.
        /// </summary>
        public List<Exception> Errors { get; private set; }
    }
}
