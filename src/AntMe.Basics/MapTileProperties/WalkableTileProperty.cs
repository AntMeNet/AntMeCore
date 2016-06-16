using System;
using System.IO;

namespace AntMe.Basics.MapTileProperties
{
    public class WalkableTileProperty : MapTileProperty
    {
        public WalkableTileProperty(SimulationContext context, MapTile mapTile) : base(context, mapTile)
        {
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
        }
    }
}
