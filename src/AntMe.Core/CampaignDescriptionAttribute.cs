using System;

namespace AntMe
{
    /// <summary>
    ///     Attribute for description of campaigns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [Serializable]
    public sealed class CampaignDescriptionAttribute : Attribute
    {
    }
}