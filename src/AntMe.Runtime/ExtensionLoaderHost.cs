using System;
using System.Reflection;

namespace AntMe.Runtime
{
    internal sealed class ExtensionLoaderHost : MarshalByRefObject
    {
        public ExtensionLoaderHost()
        {
        }

        /// <summary>
        /// Analysiert die gegebene Assembly auf die angefragten Extension Items.
        /// </summary>
        /// <param name="file">Assembly als Byte-Dump</param>
        /// <param name="level">sollen Levels geliefert werden?</param>
        /// <param name="campaign">Sollen Campaigns geliefert werden?</param>
        /// <param name="player">Sollen Player geliefert werden?</param>
        /// <returns></returns>
        public LoaderInfo AnalyseExtension(byte[] file, bool level, bool campaign, bool player)
        {
            // Basis Assemblies laden
            ExtensionLoader.LoadExtensions(null, false);

            // Zu analysierende Assembly laden
            Assembly assembly = Assembly.Load(file);
            return ExtensionLoader.AnalyseAssembly(assembly, level, campaign, player);
        }
    }
}
