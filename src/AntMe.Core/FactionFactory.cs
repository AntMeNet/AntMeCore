namespace AntMe
{
    /// <summary>
    ///     Base Class for all Factory Classes for the different Factions.
    /// </summary>
    public abstract class FactionFactory
    {
        /// <summary>
        ///     Gets a call during Initialization to handover the Interop Instance.
        /// </summary>
        /// <param name="interop">Interop Instance</param>
        public abstract void Init(FactoryInterop interop);
    }
}