using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Xml.Serialization;

namespace AntMe.Runtime
{
    /// <summary>
    ///     Static Class to load and analyze external Content like Extensions, Levels or Players.
    /// </summary>
    public static class ExtensionLoader
    {
        private const string DEFAULT_LANGUAGE = "en";

        #region local Process Area

        private static bool extensionsLoaded;
        private static IEnumerable<IExtensionPack> extensionPackCache;
        private static IEnumerable<CampaignInfo> campaignCache;
        private static IEnumerable<LevelInfo> levelCache;
        private static IEnumerable<PlayerInfo> playerCache;
        private static KeyValueStore extensionSettings;
        private static Dictionary<string, KeyValueStore> dictionaries;

        private static readonly Dictionary<Guid, PlayerStatistics> playerStatistics =
            new Dictionary<Guid, PlayerStatistics>();

        private static readonly Dictionary<Guid, CampaignStatistics> campaignStatistics =
            new Dictionary<Guid, CampaignStatistics>();

        private static readonly Dictionary<Guid, LevelStatistics> levelStatistics =
            new Dictionary<Guid, LevelStatistics>();

        private static readonly TypeMapper typeMapper = new TypeMapper();


        /// <summary>
        ///     Tries to Loads all available Extensions within the valid extension pathes.
        ///     The method searchs for Extensions in the following pathes:
        ///     - Application Path (e.g. "C:\Program Files\AntMe!\")
        ///     - Extension Folder of the Application Path (e.g. "C:\Program Files\AntMe!\Extensions")
        ///     - App Data Folder ("C:\Users\[username]\AppData\Local\AntMe\Extensions")
        ///     The method loads the following fragments:
        ///     - Factions (always)
        ///     - GameItems (always)
        ///     - Extenders (always)
        ///     - Campaigns (only full)
        ///     - Players (only full)
        ///     - Levels (only full)
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="token">Optional Reference to a Progress Token</param>
        /// <param name="full">Switch between basic Extension Loader or full Content Load.</param>
        public static void LoadExtensions(string[] extensionPaths, ProgressToken token, bool full)
        {
            if (extensionsLoaded)
                return;

            var extensionfiles = new List<string>();
            var locaFiles = new List<string>();
            var errors = new List<Exception>();
            var assemblies = new List<Assembly>();

            foreach (var path in extensionPaths)
            {
                // Check if Path exists
                if (!Directory.Exists(path))
                    continue;

                // Enumerate all dll-Files
                extensionfiles.AddRange(Directory.EnumerateFiles(path, "*.dll"));
                locaFiles.AddRange(Directory.EnumerateFiles(path, "*.language"));
            }

            // Calculation of total Tasks (based on the number of files)
            var currentTask = 0;
            if (token != null)
            {
                token.TotalTasks = (full ? extensionfiles.Count * 2 : extensionfiles.Count) + 1;
                token.CurrentTask = currentTask;
            }

            // Try to load all files from list
            foreach (var file in extensionfiles)
            {
                try
                {
                    // Try to load and add to list
                    var assembly = Assembly.LoadFile(file);
                    var attributes = assembly.GetCustomAttributes(typeof(AntMeExtensionAttribute), true);

                    if (attributes != null)
                    {
                        var extensionAttribute = attributes.FirstOrDefault() as AntMeExtensionAttribute;
                        if (extensionAttribute != null)
                            assemblies.Add(assembly);
                    }
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
            var extensionPacks = new List<IExtensionPack>();
            var settings = new KeyValueStore();
            var dictionary = new KeyValueStore();
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
                            extensionPack.Load(DefaultTypeMapper, settings, dictionary);
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

            dictionaries = new Dictionary<string, KeyValueStore>();
            dictionaries.Add(DEFAULT_LANGUAGE, dictionary);

            // Pass 2: Load Localization Files
            // TODO: Recalc Progress Token
            foreach (var file in locaFiles)
            {
                try
                {
                    var parts = file.Split('.');
                    var culture = new CultureInfo(parts[parts.Length - 2]);
                    var store = new KeyValueStore(file);

                    KeyValueStore target;
                    if (!dictionaries.TryGetValue(culture.TwoLetterISOLanguageName, out target))
                    {
                        target = dictionaries[DEFAULT_LANGUAGE].Clone();
                        dictionaries.Add(culture.TwoLetterISOLanguageName, target);
                    }

                    target.Merge(store);
                }
                catch (Exception ex)
                {
                    // Error dokumentieren
                    if (token != null && token.Errors != null)
                        token.Errors.Add(ex);
                    errors.Add(ex);
                }

                if (token != null)
                {
                    token.CurrentTask = currentTask;
                    if (token.Cancel) return;
                }
            }

            // TODO: Make sure all required Dictionary Entries are available

            // Fill Caches
            if (full)
            {
                var campaigns = new List<CampaignInfo>();
                var levels = new List<LevelInfo>();
                var players = new List<PlayerInfo>();

                // Pass 3 (Levels & Players) [campaigns, levels, players]
                foreach (var assembly in assemblies)
                {
                    var loader = AnalyseAssembly(assembly, true, true, true);

                    // Types mit File Dump
                    if (loader.Campaigns.Count > 0 ||
                        loader.Levels.Count > 0 ||
                        loader.Players.Count > 0)
                    {
                        // File dump
                        Stream stream = assembly.GetFiles()[0];
                        var file = new byte[stream.Length];
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
        ///     Loads all known statitics for the available Campagins, Levels and Players.
        /// </summary>
        public static void LoadStatistics()
        {
            // Statistics laden
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AntMe";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Levels
            try
            {
                var cache = File.ReadAllBytes(dir + "\\Level.stats");
                using (var stream = new MemoryStream(cache))
                {
                    var serializer = new XmlSerializer(typeof(LevelStatistics[]));
                    var result = serializer.Deserialize(stream) as LevelStatistics[];
                    if (result != null)
                    {
                        levelStatistics.Clear();
                        foreach (var item in result)
                            levelStatistics.Add(item.Guid, item);
                    }
                }
            }
            catch
            {
            }

            // Campaigns
            try
            {
                var cache = File.ReadAllBytes(dir + "\\Campaign.stats");
                using (var stream = new MemoryStream(cache))
                {
                    var serializer = new XmlSerializer(typeof(CampaignStatistics[]));
                    var result = serializer.Deserialize(stream) as CampaignStatistics[];
                    if (result != null)
                    {
                        campaignStatistics.Clear();
                        foreach (var item in result)
                            campaignStatistics.Add(item.Guid, item);
                    }
                }
            }
            catch
            {
            }

            // Player
            try
            {
                var cache = File.ReadAllBytes(dir + "\\Player.stats");
                using (var stream = new MemoryStream(cache))
                {
                    var serializer = new XmlSerializer(typeof(PlayerStatistics[]));
                    var result = serializer.Deserialize(stream) as PlayerStatistics[];
                    if (result != null)
                    {
                        playerStatistics.Clear();
                        foreach (var item in result)
                            playerStatistics.Add(item.Guid, item);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Saves the current state of Campaign-, Level- and Player Statistics.
        /// </summary>
        public static void SaveStatistics()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AntMe";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Levels
            try
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(LevelStatistics[]));
                    serializer.Serialize(stream, levelStatistics.Values.ToArray());

                    var cache = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(cache, 0, cache.Length);

                    File.WriteAllBytes(dir + "\\Level.stats", cache);
                }
            }
            catch
            {
            }

            // Players
            try
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(PlayerStatistics[]));
                    serializer.Serialize(stream, playerStatistics.Values.ToArray());

                    var cache = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(cache, 0, cache.Length);

                    File.WriteAllBytes(dir + "\\Player.stats", cache);
                }
            }
            catch
            {
            }

            // Campaigns
            try
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(CampaignStatistics[]));
                    serializer.Serialize(stream, campaignStatistics.Values.ToArray());

                    var cache = new byte[stream.Position];
                    stream.Position = 0;
                    stream.Read(cache, 0, cache.Length);

                    File.WriteAllBytes(dir + "\\Campaign.stats", cache);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///     List of all available Extension Packs.
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
        ///     List of all available Campaigns.
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
        ///     List of all available Levels.
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
        ///     List of all available Players.
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
        ///     Reference to the default Type Mapper.
        /// </summary>
        public static ITypeMapper DefaultTypeMapper => typeMapper;

        /// <summary>
        ///     Reference to the default Type Resolver.
        /// </summary>
        public static ITypeResolver DefaultTypeResolver => typeMapper;

        /// <summary>
        ///     Returns a copy of the Extension Settings.
        /// </summary>
        public static KeyValueStore ExtensionSettings => extensionSettings.Clone();

        public static string[] LocalizedLanguages => dictionaries.Keys.ToArray();

        public static KeyValueStore GetDictionary(string culture)
        {
            KeyValueStore result;
            if (!dictionaries.TryGetValue(culture, out result))
                result = dictionaries[DEFAULT_LANGUAGE];
            return result.Clone();
        }

        #endregion

        #region Common Analyze Methods

        /// <summary>
        ///     Scans the given Assembly for potential stuff like Campaigns, Levels and Players.
        /// </summary>
        /// <param name="assembly">Assembly to search in</param>
        /// <param name="campaign">Scan for new Campaigns</param>
        /// <param name="level">Scan for new Levels</param>
        /// <param name="player">Scan for new Players</param>
        /// <returns>Scan Results</returns>
        internal static LoaderInfo AnalyseAssembly(Assembly assembly, bool level, bool campaign, bool player)
        {
            var loaderInfo = new LoaderInfo();
            var isStatic = false;

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
                        try
                        {
                            var levelInfo = AnalyseLevelType(type);
                            loaderInfo.Levels.Add(levelInfo);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }

                    // Found Campaign
                    if (campaign && type.IsSubclassOf(typeof(Campaign)))
                        try
                        {
                            var campaignInfo = AnalyseCampaignType(type);
                            loaderInfo.Campaigns.Add(campaignInfo);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }

                    // Found Player (Ignorieren, falls Faction-Liste null)
                    if (player && type.GetCustomAttributes(typeof(FactoryAttribute), true).Length > 0)
                        try
                        {
                            var playerInfo = AnalysePlayerType(type);
                            loaderInfo.Players.Add(playerInfo);
                        }
                        catch (Exception ex)
                        {
                            loaderInfo.Errors.Add(ex);
                        }

                    // Found Extender
                    if (type.GetInterface("IExtender") != null)
                    {
                        var interfaces = type.GetInterfaces();
                    }
                }
            }

            // Static Flag nachtragen
            foreach (var item in loaderInfo.Players)
                item.IsStatic = isStatic;

            return loaderInfo;
        }

        /// <summary>
        ///     Liest alle notwendigen Informationen aus dem gegebenen Typ aus und
        ///     liefert das gesammelte LevelInfo.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Infos über das Level</returns>
        private static LevelInfo AnalyseLevelType(Type type)
        {
            var descriptionAttributes =
                (LevelDescriptionAttribute[]) type.GetCustomAttributes(
                    typeof(LevelDescriptionAttribute), false);
            var filterAttributes =
                (FactionFilterAttribute[]) type.GetCustomAttributes(
                    typeof(FactionFilterAttribute), false);

            // Kein oder zu viele Description Attributes
            if (descriptionAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid LevelDescription",
                        type.FullName,
                        type.Assembly.FullName));

            var levelInfo = new LevelInfo();
            levelInfo.Type = TypeInfo.FromType(type);
            levelInfo.LevelDescription = descriptionAttributes[0];
            levelInfo.FactionFilter = new LevelFilterInfo[filterAttributes.Length];
            for (var i = 0; i < filterAttributes.Length; i++)
                levelInfo.FactionFilter[i] = new LevelFilterInfo
                {
                    SlotIndex = filterAttributes[i].SlotIndex,
                    Comment = filterAttributes[i].Comment,
                    Type = new TypeInfo
                    {
                        AssemblyName = filterAttributes[i].FactionType.Assembly.FullName,
                        TypeName = filterAttributes[i].FactionType.FullName
                    }
                };

            // Load Map
            var context = CreateSimulationContext();
            var level = Activator.CreateInstance(type, context) as Level;
            levelInfo.Map = level.GetMap();

            // Stats anhängen
            if (!levelStatistics.ContainsKey(levelInfo.LevelDescription.Id))
                levelStatistics.Add(levelInfo.LevelDescription.Id,
                    new LevelStatistics {Guid = levelInfo.LevelDescription.Id});
            levelInfo.Statistics = levelStatistics[levelInfo.LevelDescription.Id];

            return levelInfo;
        }

        /// <summary>
        ///     Liest alle notwendigen Informationen aus dem gegebenen Typ aus und
        ///     liefert das gesammelte CampaignInfo.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Infos über die Campaign</returns>
        private static CampaignInfo AnalyseCampaignType(Type type)
        {
            var descriptionAttributes =
                (CampaignDescriptionAttribute[]) type.GetCustomAttributes(
                    typeof(CampaignDescriptionAttribute), false);

            // Kein oder zu viele Description Attributes
            if (descriptionAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid CampaignDescription",
                        type.FullName,
                        type.Assembly.FullName));

            var campaign = Activator.CreateInstance(type) as Campaign;

            // Config setzen, falls vorhanden
            CampaignStatistics stats = null;
            if (campaignStatistics.TryGetValue(campaign.Guid, out stats))
                campaign.Settings = stats.Settings;

            var campaignInfo = new CampaignInfo();
            campaignInfo.Guid = campaign.Guid;
            campaignInfo.Name = campaign.Name;
            campaignInfo.Description = campaign.Description;
            campaignInfo.Picture = campaign.Picture;
            campaignInfo.Type = TypeInfo.FromType(type);
            campaignInfo.DescriptionAttribute = descriptionAttributes[0];

            // List all unlocked Levels
            foreach (var level in campaign.GetUnlockedLevels())
            {
                var info = AnalyseLevelType(level);

                // Sicherstellen, dass Level-Anforderungen passen
                if (info.LevelDescription.MinPlayerCount > 1)
                    throw new Exception("Level is not playable alone");

                campaignInfo.Levels.Add(info);
            }

            // Stats anhängen
            if (!campaignStatistics.ContainsKey(campaign.Guid))
            {
                var statistics = new CampaignStatistics
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
        ///     Liest alle notwendigen Informationen aus dem gegebenen Typ aus und
        ///     liefert das gesammelte PlayerInfo.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Infos über den Player</returns>
        private static PlayerInfo AnalysePlayerType(Type type)
        {
            var playerAttributes =
                (FactoryAttribute[]) type.GetCustomAttributes(typeof(FactoryAttribute), true);

            // Kein oder zu viele Description Attributes
            if (playerAttributes.Length != 1)
                throw new NotSupportedException(
                    string.Format("The Class '{0}' ({1}) has no valid PlayerAttribute",
                        type.FullName,
                        type.Assembly.FullName));

            // Property Mapping
            var playerAttribute = playerAttributes[0];
            var mappings = playerAttribute.GetType()
                .GetCustomAttributes(typeof(FactoryAttributeMappingAttribute), true);

            if (mappings.Length != 1)
                throw new NotSupportedException("The Player Attribute has no valid Property Mapping");

            var mapping = mappings[0] as FactoryAttributeMappingAttribute;
            var playerType = playerAttribute.GetType();

            // Map Name
            var nameProperty = playerType.GetProperty(mapping.NameProperty);
            if (nameProperty == null)
                throw new NotSupportedException("The Name Property from Player Attribute Mapping does not exist");
            var name = (string) nameProperty.GetValue(playerAttribute, null);
            if (string.IsNullOrEmpty(name))
                throw new NotSupportedException("The Name of a Player can't be Empty");

            // Map Author
            var authorProperty = playerType.GetProperty(mapping.AuthorProperty);
            if (authorProperty == null)
                throw new NotSupportedException("The Author Property from Player Attribute Mapping does not exist");
            var author = (string) authorProperty.GetValue(playerAttribute, null);
            if (author == null)
                author = string.Empty;

            var playerInfo = new PlayerInfo
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
        ///     Scans the given Assembly for additional Level-, Campaign- and Player-Elements within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="filename">Filename</param>
        /// <param name="level">Search for Levels and Campagins</param>
        /// <param name="player">Search for Players</param>
        /// <returns>Collection of found Elements and occured Errors</returns>
        public static LoaderInfo SecureAnalyseExtension(string[] extensionPaths, string filename, bool level,
            bool player)
        {
            // Datei öffnen
            var file = File.ReadAllBytes(filename);

            // Analyse
            return SecureAnalyseExtension(extensionPaths, file, level, player);
        }

        /// <summary>
        ///     Scans the given Assembly for additional Level-, Campaign- and Player-Elements within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Filedump</param>
        /// <param name="level">Search for Levels and Campagins</param>
        /// <param name="player">Search for Players</param>
        /// <returns>Collection of found Elements and occured Errors</returns>
        public static LoaderInfo SecureAnalyseExtension(string[] extensionPaths, byte[] file, bool level, bool player)
        {
            var setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var evidence = new Evidence();
            var appDomain = AppDomain.CreateDomain("AntMe! Analyzer", evidence, setup);

            var hostType = typeof(ExtensionLoaderHost);

            var host =
                appDomain.CreateInstanceAndUnwrap(hostType.Assembly.FullName, hostType.FullName) as ExtensionLoaderHost;
            var info = host.AnalyseExtension(extensionPaths, file, level, level, player);
            foreach (var item in info.Players)
                item.Source = PlayerSource.Imported;

            AppDomain.Unload(appDomain);

            return info;
        }

        /// <summary>
        ///     Generates a Simulatio Context from the Default Mapper and Settings from this Extension Loader.
        /// </summary>
        /// <param name="random">Optional Randomizer</param>
        /// <returns>New Instance of Simulation Context</returns>
        public static SimulationContext CreateSimulationContext(Random random = null)
        {
            return new SimulationContext(DefaultTypeResolver, DefaultTypeMapper, ExtensionSettings, random);
        }

        /// <summary>
        ///     Searches in the given Assembly for the requested Level Type within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Filedump of the Assembly</param>
        /// <param name="typeName">Name of the Type</param>
        /// <returns>LevelInfo of the fitting Level or null in case of no result</returns>
        public static LevelInfo SecureFindLevel(string[] extensionPaths, byte[] file, string typeName)
        {
            var info = SecureAnalyseExtension(extensionPaths, file, true, false);
            foreach (var level in info.Levels)
                if (level.Type.TypeName.Equals(typeName))
                    return level;
            return null;
        }

        /// <summary>
        ///     Searches in the given Assembly for the requested Player Type within a closed AppDomain.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Filedump of the Assembly</param>
        /// <param name="typeName">Name of the Type</param>
        /// <returns>PlayerInfo of the fitting Player or null in case of no result</returns>
        public static PlayerInfo SecureFindPlayer(string[] extensionPaths, byte[] file, string typeName)
        {
            var info = SecureAnalyseExtension(extensionPaths, file, false, true);
            foreach (var player in info.Players)
                if (player.Type.TypeName.Equals(typeName))
                    return player;
            return null;
        }

        #endregion
    }

    /// <summary>
    ///     Token for tracking the current progress and handle the Cancel-Flag.
    /// </summary>
    public class ProgressToken
    {
        /// <summary>
        ///     Create a new token.
        /// </summary>
        public ProgressToken()
        {
            Errors = new List<Exception>();
        }

        /// <summary>
        ///     Cancel Signal to stop the current Task.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        ///     Total Amount of Tasks within the current process.
        /// </summary>
        public int TotalTasks { get; set; }

        /// <summary>
        ///     Amount of finished Tasks within the current process.
        /// </summary>
        public int CurrentTask { get; set; }

        /// <summary>
        ///     List of occured Errors so far.
        /// </summary>
        public List<Exception> Errors { get; }
    }
}