
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
    /// Static Class to load and analyze external Content like Extensions, Levels or Players.
    /// </summary>
    public static class ExtensionLoader
    {
        #region local Process Area

        private static bool extensionsLoaded = false;
        private static IEnumerable<IExtensionPack> extensionPackCache = null;
        private static IEnumerable<CampaignInfo> campaignCache = null;
        private static IEnumerable<LevelInfo> levelCache = null;
        private static IEnumerable<PlayerInfo> playerCache = null;
        private static Settings extensionSettings = null;
        private static Dictionary<Guid, PlayerStatistics> playerStatistics = new Dictionary<Guid, PlayerStatistics>();
        private static Dictionary<Guid, CampaignStatistics> campaignStatistics = new Dictionary<Guid, CampaignStatistics>();
        private static Dictionary<Guid, LevelStatistics> levelStatistics = new Dictionary<Guid, LevelStatistics>();
        private static TypeMapper typeMapper = new TypeMapper();
        

        /// <summary>
        /// Tries to Loads all available Extensions within the valid extension pathes.
        /// 
        /// The method searchs for Extensions in the following pathes:
        /// - Application Path (e.g. "C:\Program Files\AntMe!\")
        /// - Extension Folder of the Application Path (e.g. "C:\Program Files\AntMe!\Extensions")
        /// - App Data Folder ("C:\Users\[username]\AppData\Local\AntMe\Extensions")
        /// 
        /// The method loads the following fragments:
        /// - Factions (always)
        /// - GameItems (always)
        /// - Extenders (always)
        /// - Campaigns (only full)
        /// - Players (only full)
        /// - Levels (only full)
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="token">Optional Reference to a Progress Token</param>
        /// <param name="full">Switch between basic Extension Loader or full Content Load.</param>
        public static void LoadExtensions(string[] extensionPaths, ProgressToken token, bool full)
        {
            if (extensionsLoaded)
                return;

            List<string> files = new List<string>();
            List<Exception> errors = new List<Exception>();
            List<Assembly> assemblies = new List<Assembly>();

            foreach (var path in extensionPaths)
            {
                // Check if Path exists
                if (!Directory.Exists(path))
                    continue;

                // Enumerate all dll-Files
                files.AddRange(Directory.EnumerateFiles(path, "*.dll"));
            }

            // Calculation of total Tasks (based on the number of files)
            int currentTask = 0;
            if (token != null)
            {
                token.TotalTasks = (full ? files.Count * 2 : files.Count) + 1;
                token.CurrentTask = currentTask;
            }

            // Try to load all files from list
            foreach (var file in files)
            {
                try
                {
                    // Try to load and add to list
                    assemblies.Add(Assembly.LoadFile(file));
                }
                catch (Exception ex)
                {
                    // Add Loading Error to List
                    if (token != null && token.Errors != null)
                        token.Errors.Add(ex);
                    errors.Add(ex);
                }

                // Cancel
                if (token != null && token.Cancel) return;
            }

            // Recalculation of total Tasks (based on the number of loaded assemblies)
            currentTask = 1;
            if (token != null)
            {
                token.TotalTasks = (full ? assemblies.Count * 2 : assemblies.Count) + 1;
                token.CurrentTask = currentTask;
            }

            // Cancel
            if (token != null && token.Cancel) return;

            // Pass 1 Load Extension Packs
            List<IExtensionPack> extensionPacks = new List<IExtensionPack>();
            Settings settings = new Settings();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.IsClass &&
                        !type.IsAbstract &&
                        typeof(IExtensionPack).IsAssignableFrom(type))
                    {
                        IExtensionPack extensionPack = null;
                        try
                        {
                            // Instanz erzeugen & Laden
                            extensionPack = Activator.CreateInstance(type) as IExtensionPack;
                            extensionPack.Load(DefaultTypeMapper, settings);
                            extensionPacks.Add(extensionPack);
                        }
                        catch (Exception ex)
                        {
                            // Error dokumentieren
                            if (token != null && token.Errors != null)
                                token.Errors.Add(ex);
                            errors.Add(ex);

                            // Remove all Type Mapper elements
                            if (extensionPack != null)
                                DefaultTypeMapper.RemoveExtensionPack(extensionPack);
                        }
                    }

                    if (token != null)
                    {
                        token.CurrentTask = currentTask;
                        if (token.Cancel) return;
                    }
                }

                currentTask++;
            }

            extensionPackCache = extensionPacks;
            extensionSettings = settings;

            // Fill Caches
            if (full)
            {
                List<CampaignInfo> campaigns = new List<CampaignInfo>();
                List<LevelInfo> levels = new List<LevelInfo>();
                List<PlayerInfo> players = new List<PlayerInfo>();

                // Pass 2 (Levels & Players) [campaigns, levels, players]
                foreach (var assembly in assemblies)
                {
                    LoaderInfo loader = AnalyseAssembly(assembly, true, true, true);

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

                    errors.AddRange(loader.Errors);
                    if (token != null)
                    {
                        token.Errors.AddRange(loader.Errors);
                        token.CurrentTask = currentTask;
                        if (token.Cancel) return;
                    }
                }

                campaignCache = campaigns;
                levelCache = levels;
                playerCache = players;
            }

            extensionsLoaded = true;

            if (errors.Count > 0)
                throw new AggregateException("Extension Loader had some Exceptions", errors);
        }

        /// <summary>
        /// Loads all known statitics for the available Campagins, Levels and Players.
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
        /// Saves the current state of Campaign-, Level- and Player Statistics.
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
        /// List of all available Extension Packs.
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
        /// List of all available Campaigns.
        /// </summary>
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
        /// List of all available Levels.
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
        /// List of all available Players.
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
        /// Reference to the default Type Mapper.
        /// </summary>
        public static ITypeMapper DefaultTypeMapper { get { return typeMapper; } }

        /// <summary>
        /// Reference to the default Type Resolver.
        /// </summary>
        public static ITypeResolver DefaultTypeResolver { get { return typeMapper; } }

        /// <summary>
        /// Returns a copy of the Extension Settings.
        /// </summary>
        public static Settings ExtensionSettings { get { return extensionSettings.Clone(); } }

        #endregion

        #region Common Analyze Methods

        /// <summary>
        /// Scans the given Assembly for potential stuff like Campaigns, Levels and Players.
        /// </summary>
        /// <param name="assembly">Assembly to search in</param>
        /// <param name="campaign">Scan for new Campaigns</param>
        /// <param name="level">Scan for new Levels</param>
        /// <param name="player">Scan for new Players</param>
        /// <returns>Scan Results</returns>
        internal static LoaderInfo AnalyseAssembly(Assembly assembly, bool level, bool campaign, bool player)
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
                    if (player && type.GetCustomAttributes(typeof(FactoryAttribute), true).Length > 0)
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
            FactoryAttribute[] playerAttributes =
                (FactoryAttribute[])type.GetCustomAttributes(typeof(FactoryAttribute), true);

            // Kein oder zu viele Description Attributes
            if (playerAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid PlayerAttribute",
                    type.FullName,
                    type.Assembly.FullName));

            // Property Mapping
            FactoryAttribute playerAttribute = playerAttributes[0];
            var mappings = playerAttribute.GetType().GetCustomAttributes(typeof(FactoryAttributeMappingAttribute), true);

            if (mappings.Length != 1)
                throw new NotSupportedException("The Player Attribute has no valid Property Mapping");

            var mapping = mappings[0] as FactoryAttributeMappingAttribute;
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

        #region Additional App Domain Stuff

        /// <summary>
        /// Scans the given Assembly for additional Level-, Campaign- and Player-Elements within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="filename">Filename</param>
        /// <param name="level">Search for Levels and Campagins</param>
        /// <param name="player">Search for Players</param>
        /// <returns>Collection of found Elements and occured Errors</returns>
        public static LoaderInfo SecureAnalyseExtension(string[] extensionPaths, string filename, bool level, bool player)
        {
            // Datei öffnen
            byte[] file = File.ReadAllBytes(filename);

            // Analyse
            return SecureAnalyseExtension(extensionPaths, file, level, player);
        }

        /// <summary>
        /// Scans the given Assembly for additional Level-, Campaign- and Player-Elements within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Filedump</param>
        /// <param name="level">Search for Levels and Campagins</param>
        /// <param name="player">Search for Players</param>
        /// <returns>Collection of found Elements and occured Errors</returns>
        public static LoaderInfo SecureAnalyseExtension(string[] extensionPaths, byte[] file, bool level, bool player)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Evidence evidence = new Evidence();
            AppDomain appDomain = AppDomain.CreateDomain("AntMe! Analyzer", evidence, setup);

            Type hostType = typeof(ExtensionLoaderHost);

            ExtensionLoaderHost host = appDomain.CreateInstanceAndUnwrap(hostType.Assembly.FullName, hostType.FullName) as ExtensionLoaderHost;
            LoaderInfo info = host.AnalyseExtension(extensionPaths, file, level, level, player);
            foreach (var item in info.Players)
                item.Source = PlayerSource.Imported;

            AppDomain.Unload(appDomain);

            return info;
        }

        /// <summary>
        /// Searches in the given Assembly for the requested Level Type within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Filedump of the Assembly</param>
        /// <param name="typeName">Name of the Type</param>
        /// <returns>LevelInfo of the fitting Level or null in case of no result</returns>
        public static LevelInfo SecureFindLevel(string[] extensionPaths, byte[] file, string typeName)
        {
            LoaderInfo info = SecureAnalyseExtension(extensionPaths, file, true, false);
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
        /// Searches in the given Assembly for the requested Player Type within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Filedump of the Assembly</param>
        /// <param name="typeName">Name of the Type</param>
        /// <returns>PlayerInfo of the fitting Player or null in case of no result</returns>
        public static PlayerInfo SecureFindPlayer(string[] extensionPaths, byte[] file, string typeName)
        {
            LoaderInfo info = SecureAnalyseExtension(extensionPaths, file, false, true);
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
    /// Token for tracking the current progress and handle the Cancel-Flag.
    /// </summary>
    public class ProgressToken
    {
        /// <summary>
        /// Create a new token.
        /// </summary>
        public ProgressToken()
        {
            Errors = new List<Exception>();
        }

        /// <summary>
        /// Cancel Signal to stop the current Task.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Total Amount of Tasks within the current process.
        /// </summary>
        public int TotalTasks { get; set; }

        /// <summary>
        /// Amount of finished Tasks within the current process.
        /// </summary>
        public int CurrentTask { get; set; }

        /// <summary>
        /// List of occured Errors so far.
        /// </summary>
        public List<Exception> Errors { get; private set; }
    }
}
