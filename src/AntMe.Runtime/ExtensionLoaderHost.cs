using System;
using System.Linq;
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
            return ExtensionLoader.AnalyseAssembly(assembly, level, campaign, player, false);
        }

        /// <summary>
        /// Prüft, ob die angegebene Assembly mit dem AntMe-Key Signiert wurde.
        /// </summary>
        /// <param name="file">Assembly als Byte-Dump</param>
        /// <returns>Gibt an, ob die Assembly gültig signiert wurde</returns>
        public bool AnalyseExtensionAssembly(byte[] file)
        {
            try
            {
                Assembly assembly = Assembly.Load(file);

                byte[] key = assembly.GetName().GetPublicKey();

                // Kein Key, nicht signiert
                if (key == null)
                    return false;

                // Vergleich mit dem Key von AntMe.Runtime
                return key.SequenceEqual(Assembly.GetExecutingAssembly().GetName().GetPublicKey());
            }
            catch { }

            // Im Fehlerfall ist die Assembly natürlich auch nicht gültig
            return false;
        }
    }
}
