namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Spieler-Implementierten Einheiten.
    /// </summary>
    public abstract class FactionUnit
    {
        /// <summary>
        /// Initialisierung der Spieler-Klasse.
        /// </summary>
        /// <param name="interop">Zentrales Interop-Objekt für diese Einheit.</param>
        public abstract void Init(UnitInterop interop);
    }
}
