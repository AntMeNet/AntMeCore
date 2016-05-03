using System;


namespace AntMe
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class PlayerAttribute : Attribute
    {
        public bool Hidden { get; set; }
    }
}
