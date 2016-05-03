using System;

namespace AntMe.Factions.Bugs
{
    /// <summary>
    /// A bug faction. Creates new Bugs based on the given bug colony and settings.
    /// </summary>
    public sealed class BugFaction : Faction
    {
        //    private readonly Dictionary<int, BugBase> Bugs = new Dictionary<int, BugBase>();

        //    private readonly Random random = new Random();
        //    private int BugCounter;
        //    private int BugRespawnDelay;
        //    private BugNestBase Colony;
        //    private readonly Type colonyType;

        private BugFaction Nest;
        private BugFactoryInterop NestInterop;

        public BugFaction(ITypeResolver resolver, Type colony, string name, PlayerColor color)
            : base(resolver, colony, name, color)
        {
            // prüfen, ob das auch vom richtigen Typ ist
            if (!colony.IsSubclassOf(typeof(BugFactory)))
                throw new ArgumentException("Nest Type is wrong");
        }

        protected override void OnInit()
        {
            throw new NotImplementedException();
        }

        protected override void OnUpdate(int round)
        {
            throw new NotImplementedException();
        }

        //    public override void Init()
        //    {
        //        Level.RemovedItem += Level_RemovedItem;

        //        // Factory für Ameisen erzeugen
        //        Colony = (BugNestBase)Activator.CreateInstance(colonyType);

        //        // Erste Gruppe Ameisen erstellen
        //        for (int i = 0; i < BugSettings.InitialBugCount; i++)
        //            CreateBug();
        //    }

        //    private void Level_RemovedItem(Item item)
        //    {
        //        if (Bugs.ContainsKey(item.Id))
        //            Bugs.Remove(item.Id);
        //    }

        //    public override void Update(int round)
        //    {
        //        // Bugs nachproduzieren
        //        // - die Zeitverzögerung für neue Bugs muss abgelaufen sein
        //        // - die Anzahl gleichzeitiger Bugs muss kleiner dem maximalwert sein
        //        // - die Anzahl ingesamt erstellter Bugs muss kleiner als der maximalwert sein
        //        if (BugRespawnDelay-- <= 0 &&
        //            Bugs.Count < BugSettings.BugMaxConcurrentCount &&
        //            BugCounter < BugSettings.BugMaxTotalCount)
        //        {
        //            CreateBug();
        //        }

        //        // Updates für die Bugs
        //        foreach (BugBase item in Bugs.Values)
        //            item.Update();
        //    }

        //    /// <summary>
        //    /// Creates a bug somewhere on the playing field.
        //    /// </summary>
        //    private void CreateBug()
        //    {
        //        Vector2 position = new Vector2(Random.Next(0, (int)(this.Level.Map.Width * this.Level.Map.CellSize)), Random.Next(0, (int)(this.Level.Map.Height * this.Level.Map.CellSize)));
        //        var bug = (BugBase)Activator.CreateInstance(Colony.CreateMember());

        //        // TODO: change the player index when the bug management by players is clear
        //        bug.EntityItem = new BugItem(PlayerIndex, position);

        //        bug.Faction = this;
        //        bug.EntityItem.Orientation = Angle.FromDegree(random.Next(0, 359));

        //        MovingProperty prop = bug.EntityItem.GetProperty(typeof(MovingProperty)) as MovingProperty;
        //        prop.MoveDirection = bug.EntityItem.Orientation;

        //        Level.AddItem(bug.EntityItem);
        //        Bugs.Add(bug.EntityItem.Id, bug);

        //        // TODO: Kosten

        //        // Stats
        //        BugRespawnDelay = BugSettings.BugRespawnDelay;
        //        BugCounter++;
        //    }
    }
}
