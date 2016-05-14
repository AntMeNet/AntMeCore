namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Allgemeines Interface für beliebige Level-Trigger.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        ///     Erlaubt das kurzzeitige aktivieren oder deaktivieren des
        ///     Triggers.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///     Update Methode wird in jeder Runde aufgerufen und sollte true
        ///     zurück liefern, falls etwas getriggert wurde.
        /// </summary>
        /// <param name="engine">Referenz auf das aktuelle Level</param>
        /// <returns>true, falls was getriggert wurde</returns>
        bool Update(Engine engine);
    }

    public delegate void TriggerEvent(ITrigger trigger);

    public delegate void TriggerEvent<T>(ITrigger trigger, T param);
}