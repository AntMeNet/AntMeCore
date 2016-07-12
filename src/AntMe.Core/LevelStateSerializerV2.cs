using System;
using System.IO;

namespace AntMe
{
    internal sealed class LevelStateSerializerV2 : ILevelStateSerializer
    {
        private SimulationContext context;

        public LevelStateSerializerV2(SimulationContext context)
        {
            this.context = context;
        }

        public void Serialize(BinaryWriter writer, LevelState state)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
