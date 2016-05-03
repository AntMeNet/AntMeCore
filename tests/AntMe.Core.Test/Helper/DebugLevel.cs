using System;

namespace AntMe.Core.Test
{
    [LevelDescription(
        "{300EE6AF-D6A7-4351-A87D-531DD0848EE0}",
        typeof(DebugMap),
        "Debug Level",
        "Debug Level Description"
    )]
    internal class DebugLevel : Level
    {
        public DebugLevel(ITypeResolver resolver) : base(resolver) { }

        protected override void OnInit()
        {
            base.OnInit();
        }
    }
}
