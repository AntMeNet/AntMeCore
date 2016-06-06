namespace AntMe.Basics.MapTiles
{
    public abstract class CliffMapTile : MapTile
    {
        public CliffMapTile(byte heightLevel) : base(heightLevel, false)
        {
        }

        public override float GetHeight(Vector2 position)
        {
            return (HeightLevel + 1) * Map.LEVELHEIGHT;
        }
    }
}
