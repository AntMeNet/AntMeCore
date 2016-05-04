using AntMe.Items.Basics;
using System;

namespace AntMe.Levelpack.Levels
{
    /// <summary>
    /// This one is the classic AntMe! 1.0 Level. Static, plain Map with random resources here and there...
    /// </summary>
    [LevelDescription(
        "{C2A14502-944A-4B9E-8B26-0434F0DEC190}",
        typeof(ClassicMap),
        "AntMe! Classic",
        "This one is the classic AntMe! 1.0 Level. Static, plain Map with random resources here and there...",
        MinPlayerCount = 0,
        MaxPlayerCount = 8
    )]
    public sealed class ClassicLevel : Level
    {
        private SugarItem sugar = null;
        private AppleItem apple = null;

        public ClassicLevel(ITypeResolver resolver) : base(resolver) { }

        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void OnUpdate()
        {
            // Gewinnbedingung
            if (Engine.Round >= 4800 && Mode == LevelMode.Running)
            {
                Faction winner = null;
                foreach (var faction in Factions)
                {
                    if (faction == null)
                        continue;

                    if (winner == null || faction.Points > winner.Points)
                    {
                        winner = faction;
                    }
                }

                if (winner != null)
                    Finish(winner.PlayerIndex);
                else
                    Draw();
            }

            Index2 cells = Engine.Map.GetCellCount();

            if (sugar == null)
            {
                Vector2 pos = new Vector2(
                    ((float)Random.NextDouble() * (cells.X - 1)) * Map.CELLSIZE,
                    ((float)Random.NextDouble() * (cells.Y - 1)) * Map.CELLSIZE);
                sugar = new SugarItem(Resolver, pos, 1000);
                Engine.InsertItem(sugar);
            }

            if (apple == null)
            {
                Vector2 pos = new Vector2(
                    ((float)Random.NextDouble() * (cells.X - 1)) * Map.CELLSIZE,
                    ((float)Random.NextDouble() * (cells.Y - 1)) * Map.CELLSIZE);
                apple = new AppleItem(Resolver, pos, 250);
                Engine.InsertItem(apple);
            }
        }

        protected override void OnRemoveItem(Item item)
        {
            if (item == sugar)
                sugar = null;
            if (item == apple)
                apple = null;
        }
    }
}
