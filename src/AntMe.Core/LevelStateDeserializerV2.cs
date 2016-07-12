using System;
using System.IO;

namespace AntMe
{
    internal sealed class LevelStateDeserializerV2 : ILevelStateDeserializer
    {
        private SimulationContext context;

        public LevelStateDeserializerV2(SimulationContext context)
        {
            this.context = context;
        }

        public LevelState Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
