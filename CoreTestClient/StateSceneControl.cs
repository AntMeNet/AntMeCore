using AntMe;
using CoreTestClient.Renderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CoreTestClient
{
    internal class StateSceneControl : BaseSceneControl
    {
        private LevelState state;

        private Dictionary<string, TileRenderer> materials;

        private Dictionary<string, TileRenderer> tiles;

        private Font playgroundText = new Font("Courier New", 7f);
        private Brush playgroundTextBrush = new SolidBrush(Color.Black);

        private Brush itemBodyBrush = new SolidBrush(Color.FromArgb(128, 50, 50, 50));
        private Pen itemDirectionPen = new Pen(Color.Black);
        private Pen[] itemFactionPens = new[] {
            new Pen(Color.Black),
            new Pen(Color.Red),
            new Pen(Color.Blue),
            new Pen(Color.Yellow),
            new Pen(Color.Purple),
            new Pen(Color.Orange),
            new Pen(Color.Green),
            new Pen(Color.White),
        };

        private Pen[] slotPens = new Pen[8];

        public StateSceneControl()
        {
            materials = new Dictionary<string, TileRenderer>();
            foreach (var material in ExtensionLoader.DefaultTypeMapper.MapMaterials)
            {
                string path = Path.Combine(".", "Resources", material.Type.Name + ".png");
                Bitmap bitmap = new Bitmap(Image.FromFile(path));
                materials.Add(material.Type.FullName, new TileRenderer(bitmap));
            }

            tiles = new Dictionary<string, TileRenderer>();
            foreach (var mapTile in ExtensionLoader.DefaultTypeMapper.MapTiles)
            {
                string path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                Bitmap bitmap = new Bitmap(Image.FromFile(path));
                tiles.Add(mapTile.StateType.FullName, new TileRenderer(bitmap));
            }
        }

        public void SetState(LevelState state)
        {
            if (this.state != state)
            {
                this.state = state;

                if (state != null)
                {
                    foreach (var faction in state.Factions)
                        slotPens[faction.SlotIndex] = itemFactionPens[(int)faction.PlayerColor];

                    SetMapSize(state.Map.GetCellCount());
                }
                else
                {
                    SetMapSize(Index2.Zero);
                }
            }
        }

        protected override void OnDraw(Graphics g)
        {
            if (state != null)
            {
                Index2 mapSize = state.Map.GetCellCount();

                foreach (var item in state.Items.ToArray())
                {
                    float x = item.Position.X;
                    float y = item.Position.Y;

                    // Kollisionsbody
                    float rad = item.Radius;
                    g.FillEllipse(itemBodyBrush, x - rad, y - rad, rad * 2, rad * 2);

                    // Orientation
                    Vector2 angle = Vector2.FromAngle(Angle.FromDegree(item.Orientation)) * rad;
                    g.DrawLine(itemDirectionPen, x, y, x + angle.X, y + angle.Y);

                    // Faction-colored Outline
                    if (item is FactionItemState)
                    {
                        FactionItemState factionItem = item as FactionItemState;
                        Pen slotPen = slotPens[factionItem.SlotIndex];
                        g.DrawEllipse(slotPen, x - rad, y - rad, rad * 2, rad * 2);
                    }

                    // ID
                    g.DrawString(item.Id.ToString(), playgroundText, playgroundTextBrush, x, y);
                }
            }
        }

        protected override TileRenderer OnRenderMaterial(int x, int y, out MapTileOrientation orientation)
        {
            orientation = MapTileOrientation.NotRotated;

            // No Map - no Renderer
            if (state == null || state.Map == null)
                return null;

            MapTileState tile = state.Map.Tiles[x, y];

            // No Tile or Material - no Renderer
            if (tile == null || tile.Material == null)
                return null;

            TileRenderer renderer;
            orientation = tile.Orientation;
            if (materials.TryGetValue(tile.Material.GetType().FullName, out renderer))
                return renderer;

            return null;
        }

        protected override TileRenderer OnRenderTile(int x, int y, out MapTileOrientation orientation)
        {
            orientation = MapTileOrientation.NotRotated;

            // No Map - no Renderer
            if (state == null || state.Map == null)
                return null;

            MapTileState tile = state.Map.Tiles[x, y];

            // No Tile - no Renderer
            if (tile == null)
                return null;

            TileRenderer renderer;
            orientation = tile.Orientation;
            if (tiles.TryGetValue(tile.GetType().FullName, out renderer))
                return renderer;

            return null;
        }
    }
}
