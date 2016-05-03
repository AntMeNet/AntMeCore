using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.LevelProperties
{
    public sealed class TriggerLevelProperty : LevelProperty
    {
        private List<ITrigger> triggers = null;

        public TriggerLevelProperty(Level level) : base(level) { }

        public override void OnInit()
        {
            triggers = new List<ITrigger>();
        }

        /// <summary>
        ///     Registriert einen Trigger am System.
        /// </summary>
        /// <param name="trigger"></param>
        public void RegisterTrigger(ITrigger trigger)
        {
            // Nur im Running-Mode erlaubt.
            if (Level.Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            if (!triggers.Contains(trigger))
                triggers.Add(trigger);
        }

        /// <summary>
        ///     Entfernt einen Trigger aus dem laufenden System.
        /// </summary>
        /// <param name="trigger"></param>
        public void UnregisterTrigger(ITrigger trigger)
        {
            // Nur im Running-Mode erlaubt.
            if (Level.Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            if (triggers.Contains(trigger))
                triggers.Remove(trigger);
        }
    }
}
