using System;
using System.Reflection;

namespace AntMe.Runtime
{
    /// <summary>
    /// Extension Loader Host is the inner Part of the Extension Loader that runs 
    /// within the App Domain and should only be used by the Extension Loader class.
    /// </summary>
    internal sealed class ExtensionLoaderHost : MarshalByRefObject
    {
        public ExtensionLoaderHost()
        {
        }

        /// <summary>
        /// Checks the given file for additional Level-, Campaign- or Player-Elements.
        /// </summary>
        /// <param name="extensionPaths">List of pathes to search for Extensions</param>
        /// <param name="file">Assembly als Byte-Dump</param>
        /// <param name="level">Search for Levels</param>
        /// <param name="campaign">Search for Campaigns</param>
        /// <param name="player">Search for Player</param>
        /// <returns>Collection of found elements and occured errors</returns>
        public LoaderInfo AnalyseExtension(string[] extensionPaths, byte[] file, bool level, bool campaign, bool player)
        {
            // Load all Extensions
            ExtensionLoader.LoadExtensions(extensionPaths, null, false);

            // Load given Assembly and analyze it
            Assembly assembly = Assembly.Load(file);
            return ExtensionLoader.AnalyseAssembly(assembly, level, campaign, player);
        }
    }
}
