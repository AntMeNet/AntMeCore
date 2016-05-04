using System;

namespace AntMe.Levelpack.Levels
{
    [Serializable]
    internal sealed class ClassicMap : Map
    {
        public ClassicMap() : base(30, 20, true, TileSpeed.Normal, TileHeight.Medium)
        {
        }
    }
}
