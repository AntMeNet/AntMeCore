using AntMe.Basics.Items;
using System;
using System.Collections.Generic;

namespace AntMe.Levelpack.Levels
{
    /// <summary>
    /// This one is the classic AntMe! 1.0 Level. Static, plain Map with random resources here and there...
    /// </summary>
    [LevelDescription(
        "{C2A14502-944A-4B9E-8B26-0434F0DEC190}",
        "AntMe! Classic",
        "This one is the classic AntMe! 1.0 Level. Static, plain Map with random resources here and there...",
        MinPlayerCount = 0,
        MaxPlayerCount = MAX_SLOTS
    )]
    public sealed class ClassicLevel : Level
    {
        private SugarItem sugar = null;
        private AppleItem apple = null;
        private List<ClassicBugItem> bugs = new List<ClassicBugItem>();

        public ClassicLevel(SimulationContext context) : base(context) { }

        public override Map GetMap()
        {
            return new Map(30, 20, true);
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

                    if (winner == null)
                    {
                        winner = faction;
                    }
                }

                if (winner != null)
                    FinishPlayer(winner.SlotIndex);
                else
                    Draw();
            }

            Index2 cells = Engine.Map.GetCellCount();

            if (sugar == null)
            {
                Vector2 pos = new Vector2(
                    ((float)Random.NextDouble() * (cells.X - 1)) * Map.CELLSIZE,
                    ((float)Random.NextDouble() * (cells.Y - 1)) * Map.CELLSIZE);
                sugar = new SugarItem(Context, pos, 1000);
                Engine.InsertItem(sugar);
            }

            if (apple == null)
            {
                Vector2 pos = new Vector2(
                    ((float)Random.NextDouble() * (cells.X - 1)) * Map.CELLSIZE,
                    ((float)Random.NextDouble() * (cells.Y - 1)) * Map.CELLSIZE);
                apple = new AppleItem(Context, pos, 250);
                Engine.InsertItem(apple);
            }

            if (bugs.Count < 3)
            {
                Vector2 pos = new Vector2(
                    ((float)Random.NextDouble() * (cells.X - 1)) * Map.CELLSIZE,
                    ((float)Random.NextDouble() * (cells.Y - 1)) * Map.CELLSIZE);
                Angle orientation = Angle.FromDegree(Random.Next(0, 359));
                ClassicBugItem bug = new ClassicBugItem(Context, pos, orientation);
                bugs.Add(bug);
                Engine.InsertItem(bug);
            }
        }

        protected override void OnRemoveItem(Item item)
        {
            if (item == sugar)
                sugar = null;
            if (item == apple)
                apple = null;
            if (item is ClassicBugItem)
                bugs.Remove(item as ClassicBugItem);
        }
    }
}
