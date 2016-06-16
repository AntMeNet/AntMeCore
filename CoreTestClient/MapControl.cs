using AntMe;
using CoreTestClient.Renderer;
using System.Collections.Generic;
using System.Drawing;
using AntMe.Runtime;
using System.IO;
using System;

namespace CoreTestClient
{
    internal partial class MapControl : BaseSceneControl
    {
        private Map map;

        private Dictionary<string, TileRenderer> materials;

        private Dictionary<string, TileRenderer> tiles;

        public MapControl()
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
                tiles.Add(mapTile.Type.FullName, new TileRenderer(bitmap));
            }
        }

        public void SetMap(Map map)
        {
            if (this.map != map)
            {
                this.map = map;
                if (map != null) SetMapSize(map.GetCellCount());
                else SetMapSize(Index2.Zero);
            }
        }

        protected override void OnDraw(Graphics g)
        {
        }

        #region Buffer Generation

        protected override TileRenderer OnRenderMaterial(int x, int y, out Compass orientation)
        {
            orientation = Compass.East;

            // No Map - no Renderer
            if (map == null)
                return null;

            MapTile tile = map[x, y];

            // No Tile or Material - no Renderer
            if (tile == null || tile.Material == null)
                return null;

            TileRenderer renderer;
            orientation = tile.Orientation;
            if (materials.TryGetValue(tile.Material.GetType().FullName, out renderer))
                return renderer;

            return null;
        }

        protected override TileRenderer OnRenderTile(int x, int y, out Compass orientation)
        {
            orientation = Compass.East;

            // No Map - no Renderer
            if (map == null)
                return null;

            MapTile tile = map[x, y];

            // No Tile - no Renderer
            if (tile == null)
                return null;

            TileRenderer renderer;
            orientation = tile.Orientation;
            if (tiles.TryGetValue(tile.GetType().FullName, out renderer))
                return renderer;

            return null;
        }

        #endregion
    }
}
