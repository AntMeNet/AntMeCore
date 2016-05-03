using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugSightingItem : Item
    {
        private SightingProperty sighting;

        public DebugSightingItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            sighting = new SightingProperty(this, 20);

            AddProperty(sighting);
        }
    }
}
