namespace AntMe
{
    /// <summary>
    /// Basisklasse für alle Properties, die nachträglich an ein Interop Objekt angehängt werden.
    /// </summary>
    public abstract class InteropProperty : Property
    {
        /// <summary>
        /// Interner Update-Call
        /// </summary>
        internal void InternalUpdate(int round)
        {
            Update(round);
        }

        /// <summary>
        /// Virtueller Update-Call, der von der Implementierung genutzt werden kann.
        /// </summary>
        protected virtual void Update(int round) { }

        /// <summary>
        /// Basis-Delegat für diverse parameterlose Interop Events.
        /// </summary>
        public delegate void InteropEvent();

        /// <summary>
        /// Basis-Delegat für diverse parameterisierte Interop Events.
        /// </summary>
        /// <typeparam name="T">Typ des Parameters.</typeparam>
        /// <param name="parameter">Wert des Parameters.</param>
        public delegate void InteropEvent<in T>(T parameter);
    }
}
