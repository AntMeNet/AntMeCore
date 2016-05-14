namespace AntMe.Levelpack.Levels
{
    //[LevelDescription(
    //    "{48AB9FFC-66BF-402F-88D8-69E1547A9C27}", 
    //    typeof(Map), 
    //    "Small Battle Arena", 
    //    "This is the small battle Arena. Generations of ants used it to messure the mighty power of food-collecting.",
    //    Hidden = false,
    //    MinPlayerCount = 1,
    //    MaxPlayerCount = 8
    //)]
    //public sealed class ArenaLevel : Level
    //{
    //    public ArenaLevel(ITypeResolver resolver) : base(resolver) { }

    //    public Map GetMap()
    //    {
    //        using (MemoryStream stream = new MemoryStream(AntMe.Levelpack.Properties.Resources.arenaLevel))
    //        {
    //            return Map.Deserialize(stream);
    //        }
    //    }

    //    protected override void OnInit()
    //    {
    //        // TODO: Create a lot of sugar and Apple
    //        SugarItem sugar = null;
    //        AppleItem apple = null;
    //        Index2 cell;

    //        // Player 1
    //        cell = new Index2(6, 6);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 2
    //        cell = new Index2(23, 13);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 3
    //        cell = new Index2(6, 13);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 4
    //        cell = new Index2(23, 6);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 5
    //        cell = new Index2(13, 15);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 6
    //        cell = new Index2(16, 4);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 7
    //        cell = new Index2(10, 10);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Player 8
    //        cell = new Index2(21, 8);
    //        sugar = new SugarItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), SugarItem.SugarMaxCapacity);
    //        Engine.InsertItem(sugar);

    //        // Apple 1
    //        cell = new Index2(1, 4);
    //        apple = new AppleItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), AppleItem.AppleMaxAmount);
    //        Engine.InsertItem(apple);

    //        // Apple 2
    //        cell = new Index2(5, 18);
    //        apple = new AppleItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), AppleItem.AppleMaxAmount);
    //        Engine.InsertItem(apple);

    //        // Apple 3
    //        cell = new Index2(25, 18);
    //        apple = new AppleItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), AppleItem.AppleMaxAmount);
    //        Engine.InsertItem(apple);

    //        // Apple 4
    //        cell = new Index2(25, 1);
    //        apple = new AppleItem(Resolver, new Vector2((cell.X + 0.5f) * Map.CELLSIZE, (cell.Y + 0.5f) * Map.CELLSIZE), AppleItem.AppleMaxAmount);
    //        Engine.InsertItem(apple);
    //    }
    //}
}
