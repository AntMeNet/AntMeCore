using System.IO;

namespace AntMe.Basics.MapProperties
{
    /// <summary>
    /// Map Property for individual Tile Updates.
    /// </summary>
    public class TileUpdaterProperty : MapProperty
    {
        public TileUpdaterProperty(SimulationContext context, Map map) : base(context, map)
        {
            // TODO: Implement
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
        }

        public override void OnUpdate(int round)
        {
        }
    }
}
