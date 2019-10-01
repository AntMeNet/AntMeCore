namespace AntMe.Levelpack.Levels
{
    //[LevelDescription(
    //    "{9FA155B6-9A8E-4F4C-AF1C-1BBAAA4B5D9C}", 
    //    typeof(Map), 
    //    "Capture the Apple (hard)", 
    //    "Das Ziel ist einfach: Hol dir den Apfel vor deinem Gegner!",
    //    Hidden = false, 
    //    MinPlayerCount = 2, 
    //    MaxPlayerCount = 4
    //)]
    //public sealed class CaptureTheAppleHard : Level
    //{
    //    public CaptureTheAppleHard(ITypeResolver resolver) : base(resolver) { }

    //    public Map GetMap()
    //    {
    //        using (MemoryStream stream = new MemoryStream(AntMe.Levelpack.Properties.Resources.plateau))
    //        {
    //            return Map.Deserialize(stream);
    //        }
    //    }

    //    private AppleItem apple;

    //    protected override void OnInit()
    //    {
    //        // The big apple
    //        Index2 cell = new Index2(20, 15);
    //        apple = new AppleItem(Resolver, new Vector2(cell.X * Map.CELLSIZE, cell.Y * Map.CELLSIZE), AppleItem.AppleMaxAmount);
    //        Engine.InsertItem(apple);
    //    }

    //    protected override void OnRemoveItem(Item item)
    //    {
    //        if (item is AppleItem)
    //        {
    //            int winner = -1;
    //            foreach (var faction in Factions)
    //            {
    //                if (faction.Points > 0)
    //                    winner = faction.SlotIndex;
    //            }

    //            if (winner > -1)
    //                Finish(winner);
    //            else
    //                Draw();
    //        }
    //    }
    //}
}