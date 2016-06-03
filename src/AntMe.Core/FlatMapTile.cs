namespace AntMe
{
    public abstract class FlatMapTile : MapTile
    {
        public FlatMapTile(byte heightLevel) : base(heightLevel, true)
        {
        }

        public override float GetHeight(Vector2 position)
        {
            return HeightLevel * Map.LEVELHEIGHT;
        }
    }
}
