namespace AntMe.Levelpack.Levels
{
    //[LevelDescription(
    //    "{5746DFF8-BFB2-43AD-A97F-50906CE0FFE1}", 
    //    typeof(Map), 
    //    "Open Battle", 
    //    "[UGKA] Ein offenes Feld und eine ganze Menge Ameisen.",
    //    Hidden = false, 
    //    MinPlayerCount = 2, 
    //    MaxPlayerCount = 8
    //)]
    //public sealed class OpenBattle : Level
    //{
    //    public OpenBattle(ITypeResolver resolver) : base(resolver) { }

    //    public Map GetMap()
    //    {
    //        using (MemoryStream stream = new MemoryStream(AntMe.Levelpack.Properties.Resources.battlefield))
    //        {
    //            return Map.Deserialize(stream);
    //        }
    //    }

    //    protected override void OnInit()
    //    {
    //        // Init a lot of sugar :)
    //        for (int x = 23; x < 28; x++)
    //        {
    //            for (int y = 17; y < 22; y++)
    //            {
    //                SugarItem sugar = new SugarItem(Resolver, new Vector2((x + 0.5f) * Map.CELLSIZE, (y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //                Engine.InsertItem(sugar);
    //            }   
    //        }
    //    }

    //    protected override void OnUpdate()
    //    {
    //        // Gewinnbedingung
    //        if (Engine.Round >= 4800 && Mode == LevelMode.Running)
    //        {
    //            Faction winner = null;
    //            foreach (var faction in Factions)
    //            {
    //                if (faction == null)
    //                    continue;

    //                if (winner == null || faction.Points > winner.Points)
    //                {
    //                    winner = faction;
    //                }
    //            }

    //            if (winner != null)
    //                Finish(winner.SlotIndex);
    //            else
    //                Draw();
    //        }
    //    }
    //}
}
