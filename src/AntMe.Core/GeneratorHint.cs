using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Attribute to link properties to events for the generator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event,AllowMultiple = false, Inherited = false)]
    [Serializable]
    public sealed class GeneratorHint : Attribute
    {

        public Type Type { get; private set; }

        public GeneratorHint(Type type)
        {
            Type = type;
        }

    }
}
