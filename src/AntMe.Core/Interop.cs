namespace AntMe
{
    /// <summary>
    /// Basisklasse für jegliche Interop-Klasse.
    /// </summary>
    public abstract class Interop : PropertyList<InteropProperty>
    {
        internal void InternalUpdate(int round)
        {
            Update(round);
            foreach (var property in Properties)
                property.InternalUpdate(round);
        }

        /// <summary>
        /// Geschützter Update-Call.
        /// </summary>
        protected virtual void Update(int round) { }
    }
}
