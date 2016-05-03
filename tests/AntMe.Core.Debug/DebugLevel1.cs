using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntMe.Simulation.Factions.Ants;
using AntMe.Simulation.Items;
using AntMe.Core;
 

namespace AntMe.Simulation.Debug
{
    [LevelDescription(
        MinPlayerCount = 1,
        MaxPlayerCount = 1
    )]
    [FactionFilter(
        PlayerIndex = 0, 
        FactionType = typeof(AntFaction), 
        Comment = "Dieses Level ist ausschließlich zum testen von Ameisen da."
    )]
    public sealed class DebugLevel1 : Level
    {
        public override Guid Guid { get { return Guid.Parse("{7BBBFDD7-6C22-4EA0-A79C-BA6E5A6EC62B}"); } }
        public override string Name { get { return "Debug Level 1"; } }
        public override string Description { get { return "Erstes Debug Level mit der Standard Debug Map"; } }

        protected override void OnInit()
        {
            SugarItem sugar = new SugarItem(new Core.Vector2(300, 100), 1000);
            InsertItem(sugar);
            sugar.Removed += sugar_Removed;

            AppleItem apple = new AppleItem(new Core.Vector2(300, 300), 250);
            InsertItem(apple);
            apple.Removed += apple_Removed;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        void apple_Removed(Item item)
        {
            AppleItem apple = new AppleItem(new Core.Vector2(Random.Next(150, 400), Random.Next(150, 400)), 250);
            InsertItem(apple);
            apple.Removed += apple_Removed;
        }

        void sugar_Removed(Item item)
        {
            var sugar = new SugarItem(new Core.Vector2(Random.Next(150, 300), Random.Next(50, 200)), Random.Next(100,2000));
            InsertItem(sugar);
            sugar.Removed += sugar_Removed;
        }

        public override Map GetMap()
        {
            Map map = new Map();
            map.BlockBorder = true;
            int height = 20;
            int width = 20;
            map.Tiles = new MapTile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map.Tiles[x, y] = new MapTile()
                    {
                        Height = TileHeight.Medium,
                        Shape = TileShape.Flat,
                        Speed = TileSpeed.Normal
                    };
                }
            }

            // create a little plateau where ants cannot move on
            for (int x = 0; x < width / 2; x++)
            {
                for (int y = 0; y < height / 3; y++)
                {
                    map.Tiles[x, y].Height = TileHeight.High;
                }
            }

            map.StartPoints = new Core.Index2[2];
            map.StartPoints[0] = new Core.Index2(10, 19);
            map.StartPoints[1] = new Core.Index2(15, 15);

            return map;
        }
    }
}
