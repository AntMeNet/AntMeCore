using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.FactionProperties
{
    /// <summary>
    /// Property State for the Faction Property for Points.
    /// </summary>
    public sealed class PointsState : FactionStateProperty
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public PointsState() : base()
        {
            PointsPerCategory = new Dictionary<string, int>();
        }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Related Faction</param>
        /// <param name="property">Related Faction Property</param>
        public PointsState(Faction faction, PointsProperty property) : base(faction, property)
        {
            PointsPerCategory = new Dictionary<string, int>();

            // Bind Points
            Points = property.Points;
            property.OnPointsChanged += (v) => { Points = v; };

            // Bind Categories
            foreach (var key in property.PointsPerCategory.Keys)
                PointsPerCategory.Add(key, property.PointsPerCategory[key]);
            property.OnCategoryPointsChanged += (i, v) => { PointsPerCategory[i] = v; };
        }

        /// <summary>
        /// Total Amount of Points.
        /// </summary>
        [DisplayName("Points")]
        [Description("Total Amount of Points")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Points { get; set; }

        /// <summary>
        /// Points per Category
        /// </summary>
        [DisplayName("Points per Category")]
        [Description("Points per Category")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public Dictionary<string, int> PointsPerCategory { get; set; }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Points = stream.ReadInt32();
            var count = stream.ReadInt32();
            PointsPerCategory.Clear();
            for (int i = 0; i < count; i++)
            {
                string key = stream.ReadString();
                int value = stream.ReadInt32();
                PointsPerCategory.Add(key, value);
            }
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Points = stream.ReadInt32();
            var count = stream.ReadInt32();
            PointsPerCategory.Clear();
            for (int i = 0; i < count; i++)
            {
                string key = stream.ReadString();
                int value = stream.ReadInt32();
                PointsPerCategory.Add(key, value);
            }
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Points);
            stream.Write(PointsPerCategory.Count);
            foreach (var key in PointsPerCategory.Keys)
            {
                stream.Write(key);
                stream.Write(PointsPerCategory[key]);
            }
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Points);
            stream.Write(PointsPerCategory.Count);
            foreach (var key in PointsPerCategory.Keys)
            {
                stream.Write(key);
                stream.Write(PointsPerCategory[key]);
            }
        }
    }
}
