using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Level Property that allows to register Trigger for the Simulation.
    /// </summary>
    public sealed class TriggerLevelProperty : LevelProperty
    {
        private List<ITrigger> triggers = null;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Related Level</param>
        public TriggerLevelProperty(Level level) : base(level)
        {
            triggers = new List<ITrigger>();
        }

        /// <summary>
        /// Registeres a new Trigger.
        /// </summary>
        /// <param name="trigger">Trigger</param>
        public void RegisterTrigger(ITrigger trigger)
        {
            // This is only valid in running Mode.
            if (Level.Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            if (!triggers.Contains(trigger))
                triggers.Add(trigger);
        }

        /// <summary>
        /// Removes a Trigger from the List.
        /// </summary>
        /// <param name="trigger">Trigger to remove</param>
        public void UnregisterTrigger(ITrigger trigger)
        {
            // This is only valid in running Mode.
            if (Level.Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            if (triggers.Contains(trigger))
                triggers.Remove(trigger);
        }

        /// <summary>
        /// Gets called every Round to regulate the Situation.
        /// </summary>
        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var trigger in triggers.Where(t => t.Enabled))
                trigger.Update(Level);
        }
    }
}
