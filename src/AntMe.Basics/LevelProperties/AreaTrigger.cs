namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Trigger zur Überwachung eines bestimmten Spielbereiches und wird
    ///     ausgelöst, wenn sich ein beliebiges Game Item hinein bewegt.
    /// </summary>
    public class AreaTrigger : AreaTrigger<Item>
    {
    }

    /// <summary>
    ///     Trigger zur Überwachung eines bestimmten Spielbereiches und wird
    ///     ausgelöst, wenn sich ein Game Item eines bestimmten Typs und Fraktion
    ///     hinein bewegt.
    /// </summary>
    /// <typeparam name="T">Überwachter Item Typ</typeparam>
    public class FactionAreaTrigger<T> : AreaTrigger<T> where T : FactionItem
    {
        public FactionAreaTrigger()
        {
            FactionFilter = 0;
        }

        public FactionAreaTrigger(Vector2 upperLeft, Vector2 lowerRight, int factionFilter)
            : base(upperLeft, lowerRight)
        {
            FactionFilter = factionFilter;
        }

        public int FactionFilter { get; set; }

        protected override bool Filter(T item)
        {
            return item.Faction.SlotIndex == FactionFilter;
        }
    }

    /// <summary>
    ///     Trigger zur Überwachung eines bestimmten Spielbereiches und wird
    ///     ausgelöst, wenn sich der entsprechende Item Typ hinein bewegt.
    /// </summary>
    /// <typeparam name="T">Überwachter Item Typ</typeparam>
    public class AreaTrigger<T> : ITrigger where T : Item
    {
        public AreaTrigger()
        {
            Enabled = false;
            UpperLeft = new Vector2();
            LowerRight = new Vector2();
        }

        public AreaTrigger(Vector2 upperLeft, Vector2 lowerRight)
        {
            Enabled = true;
            UpperLeft = upperLeft;
            LowerRight = lowerRight;
        }

        public Vector2 UpperLeft { get; set; }
        public Vector2 LowerRight { get; set; }

        #region ITrigger Members

        public bool Enabled { get; set; }

        public bool Update(Engine engine)
        {
            bool hit = false;
            foreach (Item item in engine.Items)
            {
                if (item is T &&
                    item.Position.X >= UpperLeft.X &&
                    item.Position.X <= LowerRight.X &&
                    item.Position.Y >= UpperLeft.Y &&
                    item.Position.Y <= LowerRight.Y)
                {
                    hit = true;
                    if (OnItemTrapped != null)
                        OnItemTrapped(this, item as T);
                }
            }
            return hit;
        }

        #endregion

        protected virtual bool Filter(T item)
        {
            return true;
        }

        public event TriggerEvent<T> OnItemTrapped;
    }
}