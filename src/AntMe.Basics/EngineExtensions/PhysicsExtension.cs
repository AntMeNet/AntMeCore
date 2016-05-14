using AntMe.Basics.ItemProperties;
using System.Collections.Generic;

namespace AntMe.Basics.EngineExtensions
{
    /// <summary>
    ///     Basis Extension für jegliche Art von Bewegung, Kollision und
    /// </summary>
    public sealed class PhysicsExtension : EngineProperty
    {
        private Dictionary<int, PhysicsUnit> _cluster;
        private HashSet<PhysicsUnit> _collidables;
        private Map _map;
        private MipMap<PhysicsUnit> _mipmap;

        public PhysicsExtension(Engine engine) : base(engine) { }

        public override void Init()
        {
            _cluster = new Dictionary<int, PhysicsUnit>();
            _collidables = new HashSet<PhysicsUnit>();

            _map = Engine.Map;

            Vector2 size = _map.GetSize();
            _mipmap = new MipMap<PhysicsUnit>(size.X, size.Y);
        }

        public override void Insert(Item item)
        {
            // Relevante Elemente finden
            if ((item.ContainsProperty<WalkingProperty>() ||
                 item.ContainsProperty<CollidableProperty>() ||
                 item.ContainsProperty<CarrierProperty>() ||
                 item.ContainsProperty<PortableProperty>()) &&
                !_cluster.ContainsKey(item.Id))
            {
                var cluster = new PhysicsUnit(item, _cluster);
                _cluster.Add(item.Id, cluster);

                // Im Falle von Kollisionen in die Liste einsortieren
                if (cluster.CanCollide)
                    _collidables.Add(cluster);
            }
        }

        public override void Remove(Item item)
        {
            if (_cluster.ContainsKey(item.Id))
            {
                PhysicsUnit cluster = _cluster[item.Id];

                // Im Falle von Kollisionen aus der Liste entfernen
                if (cluster.CanCollide)
                    _collidables.Remove(cluster);

                cluster.Dispose();
                _cluster.Remove(item.Id);
            }
        }

        public override void Update()
        {
            // Positionen updaten
            foreach (PhysicsUnit item in _cluster.Values)
            {
                if (!item.Update())
                    KillUnit(item);
            }

            // Kollisionen auflösen
            _mipmap.Clear();
            foreach (PhysicsUnit item in _collidables)
            {
                if (item.CanCollide)
                    _mipmap.Add(item, item.Position, item.Radius);
            }

            // Kollisionen finden
            foreach (PhysicsUnit collidable1 in _collidables)
            {
                HashSet<PhysicsUnit> hits = _mipmap.FindAll(collidable1.Position, collidable1.Radius);
                foreach (PhysicsUnit collidable2 in hits)
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

            // Drops behandeln
            Vector2 size = Engine.Map.GetSize();
            foreach (PhysicsUnit item in _collidables)
            {
                if (item.Position.X < 0 ||
                    item.Position.Y < 0 ||
                    item.Position.X > size.X ||
                    item.Position.Y > size.Y)
                {
                    KillUnit(item);
                }
            }

            // Carrier <-> Portable Abstände checken
            foreach (PhysicsUnit item in _cluster.Values)
            {
                item.CheckPortableDistance();
            }
        }

        private void KillUnit(PhysicsUnit unit)
        {
            // Remove
            Engine.RemoveItem(unit.Item);
        }
    }
}