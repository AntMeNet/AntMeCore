using AntMe.Items.Basics;
using System.IO;

namespace AntMe.Levelpack.Levels
{
    //[LevelDescription(
    //    "{02B96E22-463B-4C37-BE1B-BB2396412F3E}", 
    //    typeof(Map), 
    //    "Capture the Apple (easy)", 
    //    "Das Ziel ist einfach: Hol dir den Apfel vor deinem Gegner!",
    //    Hidden = false, 
    //    MinPlayerCount = 2, 
    //    MaxPlayerCount = 4
    //)]
    //public sealed class CaptureTheAppleEasy : Level
    //{
    //    public CaptureTheAppleEasy(ITypeResolver resolver) : base(resolver) { }

    //    public Map GetMap()
    //    {
    //        using (MemoryStream stream = new MemoryStream(AntMe.Levelpack.Properties.Resources.openfield))
    //        {
    //            return Map.Deserialize(stream);
    //        }
    //    }

    //    private AppleItem apple;

    //    protected override void OnInit()
    //    {
    //        // The big apple
    //        Index2 cell = new Index2(26, 20);
    //        apple = new AppleItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), AppleItem.AppleMaxAmount);
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
