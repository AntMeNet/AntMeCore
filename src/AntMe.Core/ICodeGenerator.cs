using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Schnittstelle für einen einzelnen Code Generator.
    /// </summary>
    public interface ICodeGenerator
    {

        /// <summary>
        /// Erzeugt den Code im angegebenen Verzeichnis mit dem angegebenen Namen.
        /// </summary>
        /// <param name="outputPath">Ausgabepfad der erzeugten Dateien</param>
        /// <param name="account">Name des AntMe! Online Accounts oder "local", falls lokal</param>
        /// <param name="name">Name der neuen KI</param>
        /// <returns>Pfad zur initialen Datei (meistens die sln-Datei)</returns>
        string GenerateFiles(string outputPath, string account, string name);
    }
}
