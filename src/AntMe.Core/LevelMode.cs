namespace AntMe
{
    /// <summary>
    ///     Liste der möglichen Modi eines Levels.
    /// </summary>
    public enum LevelMode
    {
        #region Pre-Running (<10)

        /// <summary>
        ///     Das Level wurde zwar erstellt, aber noch nicht initialisiert.
        /// </summary>
        Uninit = 0,

        /// <summary>
        /// Gibt an, dass die Initialisierung fehlgeschlagen ist.
        /// </summary>
        InitFailed = 1,

        #endregion

        /// <summary>
        ///     Das Level wurde initialisiert und läuft.
        /// </summary>
        Running = 10,

        #region Finished (>20)

        #region Success (2x)

        /// <summary>
        ///     Spieler 1 hat gewonnen.
        /// </summary>
        FinishedPlayer1 = 21,

        /// <summary>
        ///     Spieler 2 hat gewonnen.
        /// </summary>
        FinishedPlayer2 = 22,

        /// <summary>
        ///     Spieler 3 hat gewonnen.
        /// </summary>
        FinishedPlayer3 = 23,

        /// <summary>
        ///     Spieler 4 hat gewonnen.
        /// </summary>
        FinishedPlayer4 = 24,

        /// <summary>
        ///     Spieler 5 hat gewonnen.
        /// </summary>
        FinishedPlayer5 = 25,

        /// <summary>
        ///     Spieler 6 hat gewonnen.
        /// </summary>
        FinishedPlayer6 = 26,

        /// <summary>
        ///     Spieler 7 hat gewonnen.
        /// </summary>
        FinishedPlayer7 = 27,

        /// <summary>
        ///     Spieler 8 hat gewonnen.
        /// </summary>
        FinishedPlayer8 = 28,

        #endregion

        #region Failed (3x)

        /// <summary>
        ///     Spieler 1 hat verloren.
        /// </summary>
        FailedPlayer1 = 31,

        /// <summary>
        ///     Spieler 2 hat verloren.
        /// </summary>
        FailedPlayer2 = 32,

        /// <summary>
        ///     Spieler 3 hat verloren.
        /// </summary>
        FailedPlayer3 = 33,

        /// <summary>
        ///     Spieler 4 hat verloren.
        /// </summary>
        FailedPlayer4 = 34,

        /// <summary>
        ///     Spieler 5 hat verloren.
        /// </summary>
        FailedPlayer5 = 35,

        /// <summary>
        ///     Spieler 6 hat verloren.
        /// </summary>
        FailedPlayer6 = 36,

        /// <summary>
        ///     Spieler 7 hat verloren.
        /// </summary>
        FailedPlayer7 = 37,

        /// <summary>
        ///     Spieler 8 hat verloren.
        /// </summary>
        FailedPlayer8 = 38,

        /// <summary>
        /// Durch einen Fehler im System wurde die Simulation abgebrochen.
        /// </summary>
        FailedSystem = 39,

        #endregion

        #region Draw (4x)

        /// <summary>
        /// Ein unentschiedener Spielausgang.
        /// </summary>
        Draw = 41

        #endregion

        #endregion
    }
}