namespace AntMe
{
    /// <summary>
    ///     Base Class for all Unit Classes of different Factions.
    /// </summary>
    public abstract class FactionUnit
    {
        /// <summary>
        ///     Gets a call during Initialization of single Units to handle the Interop Instance.
        /// </summary>
        /// <param name="interop">Interop Instance</param>
        public abstract void Init(UnitInterop interop);
    }
}