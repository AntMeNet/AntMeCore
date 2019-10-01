using System.Collections.Generic;
using AntMe.Basics.Items;
using AntMe.Levelpack.Properties;

namespace AntMe.Levelpack.Levels
{
    /// <summary>
    ///     This one is the classic AntMe! 1.0 Level. Static, plain Map with random resources here and there...
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
        private AppleItem apple;
        private readonly List<ClassicBugItem> bugs = new List<ClassicBugItem>();
        private SugarItem sugar;

        public ClassicLevel(SimulationContext context) : base(context)
        {
        }

        public override byte[] GetMap()
        {
            return Resources.defaultmap;
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

                    if (winner == null) winner = faction;
                }

                if (winner != null)
                    FinishPlayer(winner.SlotIndex);
                else
                    Draw();
            }

            var cells = Engine.Map.GetCellCount();

            if (sugar == null)
            {
                var pos = new Vector2(
                    (float) Random.NextDouble() * (cells.X - 1) * Map.CELLSIZE,
                    (float) Random.NextDouble() * (cells.Y - 1) * Map.CELLSIZE);
                sugar = new SugarItem(Context, pos, 1000);
                Engine.InsertItem(sugar);
            }

            if (apple == null)
            {
                var pos = new Vector2(
                    (float) Random.NextDouble() * (cells.X - 1) * Map.CELLSIZE,
                    (float) Random.NextDouble() * (cells.Y - 1) * Map.CELLSIZE);
                apple = new AppleItem(Context, pos, 250);
                Engine.InsertItem(apple);
            }

            if (bugs.Count < 3)
            {
                var pos = new Vector2(
                    (float) Random.NextDouble() * (cells.X - 1) * Map.CELLSIZE,
                    (float) Random.NextDouble() * (cells.Y - 1) * Map.CELLSIZE);
                var orientation = Angle.FromDegree(Random.Next(0, 359));
                var bug = new ClassicBugItem(Context, pos, orientation);
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