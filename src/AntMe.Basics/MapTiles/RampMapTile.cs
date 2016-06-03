using System;

namespace AntMe.Basics.MapTiles
{
    public class RampMapTile : MapTile
    {
        public RampMapTile(byte heightLevel, bool canEnter) : base(heightLevel, canEnter)
        {
        }

        public Compass Orientation { get; set; }

        public override float GetHeight(Vector2 position)
        {
            position = new Vector2(
                Math.Max(0, Math.Min(Map.CELLSIZE, position.X)),
                Math.Max(0, Math.Min(Map.CELLSIZE, position.Y)));

            throw new NotImplementedException();
        }
    }
}
