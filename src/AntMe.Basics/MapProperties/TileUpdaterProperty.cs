using System;
using System.Collections.Generic;
using System.IO;

namespace AntMe.Basics.MapProperties
{
    /// <summary>
    /// Map Property for individual Tile Updates.
    /// </summary>
    public class TileUpdaterProperty : MapProperty
    {
        public TileUpdaterProperty(Map map) : base(map)
        {
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void Update(int round)
        {
            throw new NotImplementedException();
        }

        // TODO: Collect MapTiles and Materials (!!!)

    }
}
