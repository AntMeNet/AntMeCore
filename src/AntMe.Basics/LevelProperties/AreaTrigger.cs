using System;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Level Trigger to inform about Units entering specific Areas.
    /// </summary>
    public class AreaTrigger : ITrigger
    {
        /// <summary>
        ///     Default Constructor without Parameter.
        /// </summary>
        public AreaTrigger()
        {
            Enabled = false;
            UpperLeft = new Vector2();
            LowerRight = new Vector2();
        }

        /// <summary>
        ///     Initializes the Trigger with the given Area.
        /// </summary>
        /// <param name="upperLeft">Upper Left Corner</param>
        /// <param name="lowerRight">Lower Right Corner</param>
        public AreaTrigger(Vector2 upperLeft, Vector2 lowerRight)
        {
            Enabled = true;
            UpperLeft = upperLeft;
            LowerRight = lowerRight;
        }

        /// <summary>
        ///     Gets or sets the upper Left Corner of the Area.
        /// </summary>
        public Vector2 UpperLeft { get; set; }

        /// <summary>
        ///     Gets or sets the lower Right Corner of the Area.
        /// </summary>
        public Vector2 LowerRight { get; set; }

        /// <summary>
        ///     Gets or Sets the optional Type Filter for Items.
        /// </summary>
        public Type ItemTypeFilter { get; set; }

        /// <summary>
        ///     Gets or Sets the optional Faction Filter for Items.
        /// </summary>
        public Type FactionTypeFilter { get; set; }

        /// <summary>
        ///     Gets or Sets the optional Slot Filter for Items.
        /// </summary>
        public byte? SlotFilter { get; set; }

        /// <summary>
        ///     Gets or sets a custom Filter.
        /// </summary>
        public Func<Item, bool> CustomFilter { get; set; }

        /// <summary>
        ///     Gets or sets if the Trigger is active and should be triggered.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets called in every Round to check Trigger Condition.
        /// </summary>
        /// <param name="level">Reference to the Level</param>
        /// <returns>Triggered?</returns>
        public bool Update(Level level)
        {
            var hit = false;
            foreach (var item in level.Engine.Items)
            {
                // Apply Item Filter
                if (ItemTypeFilter != null &&
                    !ItemTypeFilter.IsAssignableFrom(item.GetType()))
                    continue;

                var factionItem = item as FactionItem;

                // Apply Slot Filter
                if (SlotFilter.HasValue)
                {
                    if (factionItem == null) continue;
                    if (factionItem.Faction.SlotIndex != SlotFilter.Value) continue;
                }

                // Apply Faction Filter
                if (FactionTypeFilter != null)
                {
                    if (factionItem == null) continue;
                    if (FactionTypeFilter.IsAssignableFrom(factionItem.Faction.GetType())) continue;
                }

                // Apply custom Filter
                if (CustomFilter != null)
                    if (!CustomFilter(item))
                        continue;

                // Check Area
                if (item.Position.X >= UpperLeft.X &&
                    item.Position.X <= LowerRight.X &&
                    item.Position.Y >= UpperLeft.Y &&
                    item.Position.Y <= LowerRight.Y)
                {
                    hit = true;
                    OnItemTrapped?.Invoke(this, item);
                }
            }

            return hit;
        }

        /// <summary>
        ///     Signal for every Item that is within the Trigger Area.
        /// </summary>
        public event TriggerEvent<Item> OnItemTrapped;
    }
}