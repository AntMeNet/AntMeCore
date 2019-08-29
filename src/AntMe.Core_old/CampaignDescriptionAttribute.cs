using System;

namespace AntMe
{
    /// <summary>
    /// Attribute for description of campaigns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [Serializable]
    public sealed class CampaignDescriptionAttribute : Attribute
    {
    }
}
