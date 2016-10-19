using System;

namespace AntMe
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AntMeExtensionAttribute : Attribute
    {
        public AntMeExtensionAttribute()
        {
        }
    }
}

