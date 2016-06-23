using AntMe.Basics.ItemProperties;
using AntMe.Basics.MapTileProperties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.EngineProperties
{
    /// <summary>
    /// Groups Items from a Carrier/Portable-Connection.
    /// </summary>
    internal sealed class PhysicsGroup : IDisposable
    {
        private readonly CarrierProperty carrier;
        private readonly HashSet<PhysicsGroup> clusterUnits;
        private readonly CollidableProperty collidable;
        private readonly Item item;
        private readonly Dictionary<int, PhysicsGroup> items;
        private readonly Map map;
        private readonly Vector2 mapSize;

        private readonly WalkingProperty moving;
        private readonly PortableProperty portable;

        // Cache für die lokalen Infos (ohne Berücksichtigung anderer)
        private float clusterMass;
        private Vector3 clusterVelocity = Vector3.Zero;
        private PhysicsGroup load;
        private float ownMass;
        private Vector3 ownVelocity = Vector3.Zero;

        #region Connect und Disconnect

        public PhysicsGroup(Item item, Dictionary<int, PhysicsGroup> items)
        {
            this.item = item;
            this.items = items;
            moving = item.GetProperty<WalkingProperty>();
            collidable = item.GetProperty<CollidableProperty>();
            carrier = item.GetProperty<CarrierProperty>();
            portable = item.GetProperty<PortableProperty>();

            map = item.Engine.Map;

            mapSize = map.GetSize();

            // Attach Item Stuff
            item.CellChanged += item_CellChanged;

            // Attach Moving Stuff
            if (moving != null)
            {
                moving.OnMaximumMoveSpeedChanged += moving_OnMaximumMoveSpeedChanged;
                moving.OnMoveDirectionChanged += moving_OnMoveDirectionChanged;
                moving.OnMoveSpeedChanged += moving_OnMoveSpeedChanged;
                moving.MoveMalus = 1;
            }

            // Attach Collision Stuff
            if (collidable != null)
            {
                collidable.OnCollisionMassChanged += collidable_OnCollisionMassChanged;
                collidable.OnCollisionFixedChanged += collidable_OnCollisionFixedChanged;
            }

            // Attach Carrier Stuff
            if (carrier != null)
            {
                carrier.OnCarrierLoadChanged += carrier_OnCarrierLoadChanged;
                carrier.OnCarrierStrengthChanged += carrier_OnCarrierStrengthChanged;
            }

            // Attach Portable Stuff
            if (portable != null)
            {
                portable.OnPortableWeightChanged += portable_OnPortableMassChanged;
                portable.OnNewCarrierItem += portable_OnNewCarrierItem;
                portable.OnLostCarrierItem += portable_OnLostCarrierItem;
                clusterUnits = new HashSet<PhysicsGroup>();
            }

            Recalc();
        }

        /// <summary>
        ///     Hängt alle Eventhandler wieder aus.
        /// </summary>
        public void Dispose()
        {
            // Detach Portable Stuff
            if (portable != null)
            {
                // Drop all Carriers
                foreach (CarrierProperty carrierItem in portable.CarrierItems.ToArray())
                    carrierItem.Drop();

                portable.OnPortableWeightChanged -= portable_OnPortableMassChanged;
                portable.OnNewCarrierItem -= portable_OnNewCarrierItem;
                portable.OnLostCarrierItem -= portable_OnLostCarrierItem;
            }

            // Detach Carrier Stuff
            if (carrier != null)
            {
                // Drop carried item
                carrier.Drop();

                carrier.OnCarrierLoadChanged -= carrier_OnCarrierLoadChanged;
                carrier.OnCarrierStrengthChanged -= carrier_OnCarrierStrengthChanged;
            }

            // Detach Collision Stuff
            if (collidable != null)
            {
                collidable.OnCollisionFixedChanged -= collidable_OnCollisionFixedChanged;
                collidable.OnCollisionMassChanged -= collidable_OnCollisionMassChanged;
            }

            // Detach Moving Stuff
            if (moving != null)
            {
                moving.OnMoveSpeedChanged -= moving_OnMoveSpeedChanged;
                moving.OnMoveDirectionChanged -= moving_OnMoveDirectionChanged;
                moving.OnMaximumMoveSpeedChanged -= moving_OnMaximumMoveSpeedChanged;
            }

            // Detach Item Stuff
            item.CellChanged -= item_CellChanged;
        }

        #endregion

        #region Item Events

        /// <summary>
        ///     Zelle wechselt - Fortbewegung muss neu berechnet werden
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        private void item_CellChanged(Item item, Index2 newValue)
        {
            Recalc();
        }

        #endregion

        #region Portable Events

        private void portable_OnPortableMassChanged(Item item, float newValue)
        {
            Recalc();
        }

        private void portable_OnNewCarrierItem(CarrierProperty item)
        {
            // Fügt den Träger hinzu
            int id = item.Item.Id;
            if (!items.ContainsKey(id))
                throw new IndexOutOfRangeException("Item does not exist");
            clusterUnits.Add(items[id]);

            Recalc();
        }

        private void portable_OnLostCarrierItem(CarrierProperty item)
        {
            // Entfernt den Träger
            int id = item.Item.Id;
            if (!items.ContainsKey(id))
                throw new IndexOutOfRangeException("Item does not exist");
            clusterUnits.Remove(items[id]);

            Recalc();
        }

        #endregion

        #region Carrier Events

        /// <summary>
        ///     Das tragende Teil wurde geändert - Abhängigkeiten neu prüfen
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        private void carrier_OnCarrierLoadChanged(Item item, PortableProperty newValue)
        {
            if (newValue != null)
            {
                int id = newValue.Item.Id;
                if (!items.ContainsKey(id))
                    throw new IndexOutOfRangeException("Item does not exist");

                load = items[id];
            }
            else
                load = null;

            Recalc();
        }

        private void carrier_OnCarrierStrengthChanged(Item item, float newValue)
        {
            Recalc();
        }

        #endregion

        #region Collision Events

        private void collidable_OnCollisionFixedChanged(Item item, bool newValue)
        {
            Recalc();
        }

        private void collidable_OnCollisionMassChanged(Item item, float newValue)
        {
            Recalc();
        }

        #endregion

        #region Moving Events

        /// <summary>
        ///     Bewegungsparameter geändert - Geschwindigkeit neu berechnen
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        private void moving_OnMoveSpeedChanged(Item item, float newValue)
        {
            Recalc();
        }

        /// <summary>
        ///     Bewegungsparameter geändert - Geschwindigkeit neu berechnen
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        private void moving_OnMoveDirectionChanged(Item item, Angle newValue)
        {
            Recalc();
        }

        /// <summary>
        ///     Bewegungsparameter geändert - Geschwindigkeit neu berechnen
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        private void moving_OnMaximumMoveSpeedChanged(Item item, float newValue)
        {
            Recalc();
        }

        #endregion

        /// <summary>
        ///     Berechnet die resultierende Geschwindigkeit des lokalen Elementes
        ///     neu. Berücksichtigt werden die move Parameter Speed, Direction, CellSpeed.
        /// </summary>
        private void Recalc()
        {
            #region Berechnung lokaler Variablen

            // Zelle berücksichtigen
            Index2 cell = map.GetCellIndex(item.Position);
            float cellspeed = map[cell.X, cell.Y].Material.Speed;

            // Eigenständige Bewegung
            if (moving != null)
            {
                float speed = Math.Min(moving.MaximumSpeed, moving.Speed);
                ownVelocity = Vector3.FromAngleXY(moving.Direction)*speed*cellspeed;
            }

            ownMass = (collidable != null ? collidable.CollisionMass : 0f);

            #endregion

            #region Berechnung der Cluster Variablen

            // Portable Stuff
            if (portable != null)
            {
                // Sammeln der Daten
                Vector3 tempVelocity = ownVelocity;
                float tempMass = ownMass;
                float tempStrength = 0f;
                int itemCount = 1;
                foreach (PhysicsGroup unit in clusterUnits)
                {
                    tempVelocity += unit.clusterVelocity;
                    tempMass += unit.clusterMass;
                    tempStrength += (unit.carrier != null ? unit.carrier.CarrierStrength : 0f);
                    itemCount++;
                }

                clusterMass = tempMass;

                // Trägerkraft berücksichtigen
                // (Geschwindigkeit verhält sich antiproportional zur Summe der Stärken)
                // Übersteigt die Summe der Stärken das Gewicht, ist die maximal mögliche Geschwindigkeit erreicht
                float speedFactor = (Weight > 0f ? tempStrength/Weight : 1f);
                speedFactor = Math.Min(1f, Math.Max(0f, speedFactor));

                // Geschwindigkeit des Portables nur berüchsichtigen, wenn es ebenso Moving ist
                itemCount -= (moving != null ? 0 : 1);

                // Resultierende Geschwindigkeit berechnen
                clusterVelocity = (itemCount > 0 ? (tempVelocity/itemCount)*speedFactor : Vector3.Zero);
            }
            else
            {
                clusterVelocity = ownVelocity;
                clusterMass = ownMass;
            }

            #endregion

            // Änderungen weiter geben
            if (load != null)
                load.Recalc();
        }

        /// <summary>
        ///     Führt ein Positionsupdate durch. Die Kollisionsauflösung muss aber später passieren. Gibt zurück, ob das Element
        ///     weiter existiert.
        /// </summary>
        /// <returns>Existiert das Element weiter</returns>
        public bool Update()
        {
            Vector3 bounce = Vector3.Zero;
            PositionResult result = SetPosition(Position + AppliedVelocity, ref bounce, false);
            if (moving != null) moving.MoveMalus = 1;
            return result != PositionResult.Dropped;
        }

        /// <summary>
        ///     Prüft, ob ein eventuell getragenes Element immernoch in Tragreichweite ist und lässt es gegebenenfalls fallen.
        /// </summary>
        public void CheckPortableDistance()
        {
            if (load != null)
            {
                if (load.portable.PortableRadius <= Item.GetDistance(Item, load.Item))
                    carrier.Drop();
            }
        }

        /// <summary>
        ///     Kümmert sich um eine Kollision zweier Elemente.
        /// </summary>
        /// <param name="cluster"></param>
        public void Collide(PhysicsGroup cluster)
        {
            float max = Radius + cluster.Radius;
            float dist = (Position - cluster.Position).Length();

            // Auflösung
            if (!IsFixed || !cluster.IsFixed)
            {
                // Richtungsvektor Item1->Item2
                Vector3 direction = (cluster.Position - Position);

                // Fallback 1, falls direction == [0,0]:
                // vorherige Positionen zum Diff verwenden
                if (direction.LengthSquared() < Vector3.EPS_MIN)
                    direction = ((cluster.Position - cluster.AppliedVelocity) - (Position - AppliedVelocity));

                // Fallback 2, falls direction == [0,0];
                // fixe Achse
                if (direction.LengthSquared() < Vector3.EPS_MIN)
                    direction = new Vector3(1, 0, 0);

                // Richtungsvektor Normalisieren
                direction = direction.Normalize();


                float diff = max - dist;
                var bounce = new Vector3();

                // Item1 fixed, item2 not fixed
                if (IsFixed)
                {
                    // Nur Item2 bewegen
                    // Ignore Result, da eh nix zu retten ist
                    cluster.SetPosition(cluster.Position + (direction*diff), ref bounce, true);
                }
                    // Item1 not fixed, item2 fixed
                else if (cluster.IsFixed)
                {
                    // Nur Item1 bewegen
                    // Result unwichtig, da eh nichts zu retten ist
                    SetPosition(Position + (direction.InvertXYZ()*diff), ref bounce, true);
                }
                else
                {
                    // Über Masse auflösen
                    float totalmass = AppliedMass + cluster.AppliedMass;
                    float spanItem1 = diff*(cluster.AppliedMass/totalmass);
                    float spanItem2 = diff*(AppliedMass/totalmass);
                    Vector3 diffItem1 = direction.InvertXYZ()*spanItem1;
                    Vector3 diffItem2 = direction*spanItem2;

                    // Apply
                    // Im Falle eines Blocks wird der Bounce auf das andere Element übertragen.
                    bounce = Vector3.Zero;
                    if (SetPosition(Position + diffItem1, ref bounce, true) == PositionResult.Blocked)
                        diffItem2 += bounce;

                    // Im Falle eines Bounce beim zweiten Element wird der Bounce ein letztes mal an Element 1 weiter gegeben.
                    bounce = Vector3.Zero;
                    if (cluster.SetPosition(cluster.Position + diffItem2, ref bounce, true) == PositionResult.Blocked)
                        SetPosition(Position + bounce, ref bounce, true);
                }
            }

            // Kollision melden
            if (collidable != null)
                collidable.CollideItem(cluster.Item);
            if (cluster.collidable != null)
                cluster.collidable.CollideItem(Item);
        }

        /// <summary>
        ///     Setzt die neue Position des Elementes (
        /// </summary>
        /// <param name="position">Neue Position des Elements</param>
        /// <param name="bounce">
        ///     Der Abprallvektor, falls das Element
        ///     gegen eine Wand stößt
        /// </param>
        /// <param name="forced">
        ///     Legt fest, ob das Element freiwillig
        ///     (normales Update) oder mit Gewalt (Kollision) bewegt wird.
        /// </param>
        /// <returns>Gibt das Ergebnis der Bewegung zurück</returns>
        private PositionResult SetPosition(Vector3 position,
            ref Vector3 bounce, bool forced)
        {
            bool dropped = false;
            bool blocked = false;

            var temp = PositionResult.Done;
            Vector3 posTemp = position;

            #region Rand

            // Randverhalten
            // X-Achse
            if (position.X < Radius)
                temp = HandleLeftBorder(ref position, forced);
            else if (position.X >= mapSize.X - Radius)
                temp = HandleRightBorder(ref position, forced);
            else temp = PositionResult.Done;

            if (temp == PositionResult.Blocked)
            {
                bounce += (position - posTemp);
                posTemp = position;
            }

            dropped |= temp == PositionResult.Dropped;
            blocked |= temp == PositionResult.Blocked;

            // Y-Achse
            if (position.Y < Radius)
                temp = HandleTopBorder(ref position, forced);
            else if (position.Y >= mapSize.Y - Radius)
                temp = HandleBottomBorder(ref position, forced);
            else temp = PositionResult.Done;

            if (temp == PositionResult.Blocked)
            {
                bounce += (position - posTemp);
                posTemp = position;
            }

            dropped |= temp == PositionResult.Dropped;
            blocked |= temp == PositionResult.Blocked;

            // Z-Achse
            position.Z = map.GetHeight(new Vector2(position.X, position.Y));

            // TODO: Bei fliegenden Einheiten die freie Bewegbarkeit prüfen
            // Z-Achse
            //float height = map.GetHeight(new Vector2(position.X, position.Y));
            //if (position.Z < Map.MIN_Z || position.Z < height)
            //    temp = HandleFloorBorder(ref position, height);
            //else if (position.Z >= Map.MAX_Z - Radius)
            //    temp = HandleCeilingBorder(ref position);
            //else temp = PositionResult.Done;

            //if (temp == PositionResult.Blocked)
            //{
            //    bounce += (position - posTemp);
            //    posTemp = position;
            //}

            dropped |= temp == PositionResult.Dropped;
            blocked |= temp == PositionResult.Blocked;

            #endregion

            #region Zelle

            // Zellenrand
            Vector3 diff = position - item.Position;

            // X-Achse
            if (diff.X < 0)
                temp = HandleLeftCell(ref position, forced);
            else if (diff.X > 0)
                temp = HandleRightCell(ref position, forced);
            else temp = PositionResult.Done;

            if (temp == PositionResult.Blocked)
            {
                bounce += (position - posTemp);
                posTemp = position;
            }

            blocked |= temp == PositionResult.Blocked;

            // Y-Achse
            if (diff.Y < 0)
                temp = HandleTopCell(ref position, forced);
            else if (diff.Y > 0)
                temp = HandleBottomCell(ref position, forced);
            else temp = PositionResult.Done;

            if (temp == PositionResult.Blocked)
            {
                bounce += (position - posTemp);
                posTemp = position;
            }

            blocked |= temp == PositionResult.Blocked;

            #endregion

            item.Position = position;

            // Falls das Element gedropped wurde
            if (dropped)
                return PositionResult.Dropped;

            // Falls geblockt wurde
            if (blocked)
                return PositionResult.Blocked;

            // Falls alles gut ging
            return PositionResult.Done;
        }

        #region Border Handling

        /// <summary>
        ///     Verarbeitet die Kollision mit der linken Wand
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleLeftBorder(ref Vector3 position, bool forced)
        {
            var result = PositionResult.Done;
            if (map.BlockBorder)
            {
                position.X = Radius;
                result = PositionResult.Blocked;
            }
            else
            {
                result = PositionResult.Dropped;
            }

            // Event werfen
            if (!forced)
                TriggerBorderEvent(Compass.West);

            return result;
        }

        /// <summary>
        ///     Verarbeitet die Kollision mit der rechten Wand
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleRightBorder(ref Vector3 position, bool forced)
        {
            var result = PositionResult.Done;
            if (map.BlockBorder)
            {
                position.X = mapSize.X - Radius - 0.0001f;
                result = PositionResult.Blocked;
            }
            else
            {
                result = PositionResult.Dropped;
            }

            // Event werfen
            if (!forced)
                TriggerBorderEvent(Compass.East);

            return result;
        }

        /// <summary>
        ///     Verarbeitet die Kollision mit der oberen Wand
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleTopBorder(ref Vector3 position, bool forced)
        {
            var result = PositionResult.Done;
            if (map.BlockBorder)
            {
                position.Y = Radius;
                result = PositionResult.Blocked;
            }
            else
            {
                result = PositionResult.Dropped;
            }

            // Event werfen
            if (!forced)
                TriggerBorderEvent(Compass.North);

            return result;
        }

        /// <summary>
        ///     Verarbeitet die Kollision mit der unteren Wand
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleBottomBorder(ref Vector3 position, bool forced)
        {
            var result = PositionResult.Done;
            if (map.BlockBorder)
            {
                position.Y = mapSize.Y - Radius - 0.0001f;
                result = PositionResult.Blocked;
            }
            else
            {
                result = PositionResult.Dropped;
            }

            // Event werfen
            if (!forced)
                TriggerBorderEvent(Compass.South);

            return result;
        }

        /// <summary>
        ///     Verarbeitet die Kollision mit der Decke
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleCeilingBorder(ref Vector3 position)
        {
            position.Z = Map.MAX_Z - Radius - Vector3.EPS_MIN;
            return PositionResult.Blocked;
        }

        /// <summary>
        ///     Verarbeitet die Kollision mit dem Boden.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleFloorBorder(ref Vector3 position, float height)
        {
            // TODO: Cell-Höhe berücksichtigen
            position.Z = Math.Max(Map.MIN_Z, height);
            return PositionResult.Blocked;
        }

        /// <summary>
        ///     Löst das Border-Event aus, falls die Rahmenparameter stimmen
        /// </summary>
        /// <param name="direction"></param>
        private void TriggerBorderEvent(Compass direction)
        {
            if (moving != null && map.BlockBorder)

                moving.HitBorder(direction);
        }

        #endregion

        #region Cell Handling

        /// <summary>
        ///     Prüft den Höhenunterschied beim Zellenwechsel nach links.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleLeftCell(ref Vector3 position, bool forced)
        {
            Index2 cell = map.GetCellIndex(new Vector3(
                position.X - Radius, item.Position.Y, item.Position.Z));
            if (cell != item.Cell)
            {
                // Prüft, ob die neue Zelle begehbar ist.
                if (!map[cell.X, cell.Y].ContainsProperty<WalkableTileProperty>())
                {
                    // Position korrigieren
                    position.X = ((cell.X + 1)*Map.CELLSIZE) + Radius;
                    if (!forced) TriggerCellEvent(Compass.West);
                    return PositionResult.Blocked;
                }
            }

            return PositionResult.Done;
        }

        /// <summary>
        ///     Prüft den Höhenunterschied beim Zellenwechsel nach rechts.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleRightCell(ref Vector3 position, bool forced)
        {
            Index2 cell = map.GetCellIndex(new Vector3(
                position.X + Radius, item.Position.Y, item.Position.Z));
            if (cell != item.Cell)
            {
                // Prüft, ob die neue Zelle begehbar ist.
                if (!map[cell.X, cell.Y].ContainsProperty<WalkableTileProperty>())
                {
                    // Position korrigieren
                    position.X = (cell.X*Map.CELLSIZE) - Radius;
                    if (!forced) TriggerCellEvent(Compass.East);
                    return PositionResult.Blocked;
                }
            }

            return PositionResult.Done;
        }

        /// <summary>
        ///     Prüft den Höhenunterschied beim Zellenwechsel nach oben.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleTopCell(ref Vector3 position, bool forced)
        {
            Index2 cell = map.GetCellIndex(new Vector3(
                item.Position.X, position.Y - Radius, position.Z));
            if (cell != item.Cell)
            {
                // Prüft, ob die neue Zelle begehbar ist.
                if (!map[cell.X, cell.Y].ContainsProperty<WalkableTileProperty>())
                {
                    // Position korrigieren
                    position.Y = ((cell.Y + 1)*Map.CELLSIZE) + Radius;
                    if (!forced) TriggerCellEvent(Compass.North);
                    return PositionResult.Blocked;
                }
            }

            return PositionResult.Done;
        }

        /// <summary>
        ///     Prüft den Höhenunterschied beim Zellenwechsel nach links.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        private PositionResult HandleBottomCell(ref Vector3 position, bool forced)
        {
            Index2 cell = map.GetCellIndex(new Vector3(
                item.Position.X, position.Y + Radius, item.Position.Z));
            if (cell != item.Cell)
            {
                // Prüft, ob die neue Zelle begehbar ist.
                if (!map[cell.X, cell.Y].ContainsProperty<WalkableTileProperty>())
                {
                    // Position korrigieren
                    position.Y = (cell.Y*Map.CELLSIZE) - Radius;
                    if (!forced) TriggerCellEvent(Compass.South);
                    return PositionResult.Blocked;
                }
            }

            return PositionResult.Done;
        }

        /// <summary>
        ///     Feuert das Event für eine Wand-Kollision.
        /// </summary>
        /// <param name="direction"></param>
        private void TriggerCellEvent(Compass direction)
        {
            if (moving != null)
                moving.HitWall(direction);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt die Referenz auf das Item zurück.
        /// </summary>
        public Item Item
        {
            get { return item; }
        }

        /// <summary>
        ///     Gibt die Position des Items zurück.
        /// </summary>
        public Vector3 Position
        {
            get { return item.Position; }
        }

        /// <summary>
        ///     Gibt den Radius des Körpers zurück.
        /// </summary>
        public float Radius
        {
            get { return (collidable != null ? collidable.CollisionRadius : 0); }
        }

        /// <summary>
        ///     Gibt zurück, ob der Cluster bei den Kollisionen berücksichtigt werden soll.
        /// </summary>
        public bool CanCollide
        {
            get { return (collidable != null); }
        }

        /// <summary>
        ///     Gibt zurück, ob das Element fixiert ist oder bewegt werden kann.
        /// </summary>
        public bool IsFixed
        {
            get { return (collidable != null ? collidable.CollisionFixed : false); }
        }

        /// <summary>
        ///     Gibt das Gewicht des Elementes an.
        /// </summary>
        private float Weight
        {
            get { return (portable != null ? portable.PortableWeight : 0f); }
        }

        /// <summary>
        ///     Gibt die Stärke dieser Einheit zurück.
        /// </summary>
        private float Strength
        {
            get { return (carrier != null ? carrier.CarrierStrength : 0f); }
        }

        /// <summary>
        ///     Effektiv anzuwendende Geschwindigkeit bei der Bewegungsberechnung.
        /// </summary>
        public Vector3 AppliedVelocity
        {
            get
            {
                if (load != null)
                    return load.AppliedVelocity;
                return clusterVelocity;
            }
        }

        /// <summary>
        ///     Effektiv anzuwendende Masse bei Kollisionsberechnungen.
        /// </summary>
        public float AppliedMass
        {
            get
            {
                if (load != null)
                    return load.AppliedMass;
                return clusterMass;
            }
        }

        #endregion
    }

    internal enum PositionResult
    {
        Done,
        Blocked,
        Dropped
    }
}