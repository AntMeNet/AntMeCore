using System;
using System.Collections.Generic;
using System.IO;

namespace AntMe.Basics.MapProperties
{
    public class UpdateTileProperty : MapProperty
    {
        /// <summary>
        /// List of all updateable Map Tiles.
        /// </summary>
        private HashSet<IUpdateableMapTile> updateableMapTiles;

        public UpdateTileProperty(Map map) : base(map)
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
