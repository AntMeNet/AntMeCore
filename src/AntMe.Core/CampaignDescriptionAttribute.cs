using System;

namespace AntMe
{
    /// <summary>
    /// Attribut zur Beschreibung von Kampagnen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [Serializable]
    public sealed class CampaignDescriptionAttribute : Attribute
    {
    }
}
