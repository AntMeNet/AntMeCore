using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugEngineExtension : EngineExtension
    {
        public static int CreateCalls { get; set; }

        public DebugEngineExtension(IEngine engine) : base(engine)
        {
            CreateCalls++;
        }

        public int InitCalls { get; set; }

        public int UpdateCalls { get; set; }

        public int InsertCalls { get; set; }

        public int RemoveCalls { get; set; }

        public override void Init()
        {
            InitCalls++;
        }

        public override void Update()
        {
            UpdateCalls++;
        }

        public override void Insert(Item item)
        {
            InsertCalls++;
        }

        public override void Remove(Item item)
        {
            RemoveCalls++;
        }
    }
}
