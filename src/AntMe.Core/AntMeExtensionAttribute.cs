using System;

namespace AntMe
{
    /// <summary>
    /// Attribute to mark an assebmly as a AntMe Extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class AntMeExtensionAttribute : Attribute
    {
    }
}

