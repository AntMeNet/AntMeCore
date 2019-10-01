using System.Collections.Generic;
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.EngineProperties
{
    /// <summary>
    ///     Engine Property for handling all physical stuff like Movement, Collision and Grouping
    /// </summary>
    public sealed class PhysicsProperty : EngineProperty
    {
        private readonly HashSet<PhysicsGroup> collidables;
        private readonly Dictionary<int, PhysicsGroup> groups;

        private Map map;
        private MipMap<PhysicsGroup> mipmap;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="engine">Engine Reference</param>
        public PhysicsProperty(Engine engine) : base(engine)
        {
            groups = new Dictionary<int, PhysicsGroup>();
            collidables = new HashSet<PhysicsGroup>();
        }

        /// <summary>
        ///     Gets a call after Engine Initialization.
        /// </summary>
        public override void Init()
        {
            map = Engine.Map;

            var size = map.GetSize();
            mipmap = new MipMap<PhysicsGroup>(size.X, size.Y);
        }

        /// <summary>
        ///     Gets a call after adding a new Item to the Engine.
        /// </summary>
        /// <param name="item">New Item</param>
        protected override void Insert(Item item)
        {
            // Filter for relevant Items.
            if ((item.ContainsProperty<WalkingProperty>() ||
                 item.ContainsProperty<CollidableProperty>() ||
                 item.ContainsProperty<CarrierProperty>() ||
                 item.ContainsProperty<PortableProperty>()) &&
                !groups.ContainsKey(item.Id))
            {
                var group = new PhysicsGroup(item, groups);
                groups.Add(item.Id, group);

                // Collect all Collidable Group
                if (group.CanCollide)
                    collidables.Add(group);
            }
        }

        /// <summary>
        ///     Gets a call before removing an item from Engine.
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected override void Remove(Item item)
        {
            if (groups.ContainsKey(item.Id))
            {
                var group = groups[item.Id];

                // Remove from Collidable List
                if (group.CanCollide)
                    collidables.Remove(group);

                group.Dispose();
                groups.Remove(item.Id);
            }
        }

        /// <summary>
        ///     Gets a call after every Engine Update.
        /// </summary>
        public override void Update()
        {
            // Update Position
            foreach (var item in groups.Values)
                if (!item.Update())
                    KillUnit(item);

            // Collisions
            mipmap.Clear();
            foreach (var item in collidables)
                if (item.CanCollide)
                    mipmap.Add(item, item.Position, item.Radius);

            foreach (var collidable1 in collidables)
            {
                var hits = mipmap.FindAll(collidable1.Position, collidable1.Radius);
                foreach (var collidable2 in hits)
                {
                    // Kollision mit sich selbst überspringen
                    if (collidable2 == collidable1)
                        continue;

                    // TODO: Prüfen, ob diese Item-Kombination schon mal dran war

                    // Auf Kollision prüfen
                    if (Item.GetDistance(collidable1.Item, collidable2.Item) > collidable1.Radius + collidable2.Radius)
                        continue;

                    // Kollision auflösen
                    collidable1.Collide(collidable2);
                }
            }

            // Handle Map-Drops
            var size = Engine.Map.GetSize();
            foreach (var item in collidables)
                if (item.Position.X < 0 ||
                    item.Position.Y < 0 ||
                    item.Position.X > size.X ||
                    item.Position.Y > size.Y)
                    KillUnit(item);

            // Check Carrier/Portable Distance
            foreach (var item in groups.Values) item.CheckPortableDistance();
        }

        private void KillUnit(PhysicsGroup unit)
        {
            // Remove
            Engine.RemoveItem(unit.Item);
        }
    }
}