namespace AntMe
{
    /// <summary>
    ///     List of possible Level States.
    /// </summary>
    public enum LevelMode
    {
        /// <summary>
        ///     Level is new generated and uninitialized.
        /// </summary>
        Uninit = 0,

        /// <summary>
        ///     Initialization failed. Check <see cref="Level.LastException" /> for more Information.
        /// </summary>
        InitFailed = 1,

        /// <summary>
        ///     Simulation Init was successful. Level is ready to simulate.
        /// </summary>
        Running = 5,

        /// <summary>
        ///     Simulation finished with a Winner. See <see cref="Level.LevelModeSlots" /> for more Information.
        /// </summary>
        Finished = 10,

        /// <summary>
        ///     Player failed to achieve Mission. See <see cref="Level.LevelModeSlots" /> for more Information.
        /// </summary>
        Failed = 11,

        /// <summary>
        ///     Simulation finished without a Winner or Loser.
        /// </summary>
        Draw = 12,

        /// <summary>
        ///     Durch einen Fehler im System wurde die Simulation abgebrochen.
        /// </summary>
        SystemException = 20,

        /// <summary>
        ///     One Player caused an Exception. See <see cref="Level.LastException" /> for Excecption and
        ///     <see cref="Level.LevelModeSlots" /> for the responsible Slot.
        /// </summary>
        PlayerException = 21,

        /// <summary>
        ///     The simulation detected a Cheat. See <see cref="Level.LastException" /> for Excecption and
        ///     <see cref="Level.LevelModeSlots" /> for the responsible Slot.
        /// </summary>
        PlayerCheating = 22
    }
}