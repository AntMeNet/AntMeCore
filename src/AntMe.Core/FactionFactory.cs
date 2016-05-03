namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Spieler-Implementierungen der Factory-Klasse. 
    /// Im Falle von Ameisen ist das die Basis-Klasse für die Colony.
    /// </summary>
    public abstract class FactionFactory
    {
        /// <summary>
        /// Wird beim Initialisieren der Factory und zur Übergabe des Interop-Objektes aufgerufen.
        /// </summary>
        /// <param name="interop"></param>
        public abstract void Init(FactoryInterop interop);
    }
}
