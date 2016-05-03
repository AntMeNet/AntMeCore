using AntMe.ItemProperties.Basics;
using System;
using System.Collections.Generic;

namespace AntMe.Factions.Ants
{
    public sealed class AntFactionSettings
    {
        #region AntHill Management

        /// <summary>
        ///     Gibt die Kosten für den Bau eines Ameisenhügels an.
        /// </summary>
        public Dictionary<CollectableGoodProperty, int> AntHillBuildCosts = new Dictionary<CollectableGoodProperty, int>();

        /// <summary>
        ///     Gibt an, ob Ameisen in der Lage sein können, Ameisenhügel zu bauen.
        /// </summary>
        public bool AntHillBuildable = false;

        /// <summary>
        ///     Gibt an, ob die Ameisenhügel Angreifbar und zerstörbar sind.
        /// </summary>
        public bool AntHillDestructable = false;

        /// <summary>
        ///     Gibt an, wie viele Hitpoints ein Ameisenhügel hat.
        /// </summary>
        public int AntHillHitpoints = 1000;

        /// <summary>
        ///     Gibt die maximale Anzahl gleichzeitiger Ameisenhügel an.
        /// </summary>
        public int AntHillMaxConcurrentCount = 1;

        /// <summary>
        ///     Maximale Anzahl Ameisenhügel insgesamt.
        /// </summary>
        public int AntHillMaxTotalCount = int.MaxValue;

        /// <summary>
        ///     Gibt die Kosten für eine Reperatur eines Ameisenhügels (pro 10 Hitpoints) an.
        /// </summary>
        public Dictionary<CollectableGoodProperty, int> AntHillRepairCosts = new Dictionary<CollectableGoodProperty, int>();

        /// <summary>
        ///     Gibt an, ob es beim Start der Simulation einen Ameisenhügel an der
        ///     Home-Position geben soll.
        /// </summary>
        public bool InitialAntHill = true;

        #endregion

        #region Ant Management

        /// <summary>
        ///     Kosten für eine Ameise.
        /// </summary>
        public Dictionary<Type, int> AntBuildCosts = new Dictionary<Type, int>();

        /// <summary>
        ///     Initiale Menge von Ameisen beim Start.
        /// </summary>
        public int AntCount = 0;

        /// <summary>
        ///     Maximale Anzahl Ameisen die gleichzeitig auf dem Spielfeld sind.
        /// </summary>
        public int AntMaxConcurrentCount = 100;

        /// <summary>
        ///     Maximale Anzahl Ameisen insgesamt für diesen Spieler.
        /// </summary>
        public int AntMaxTotalCount = int.MaxValue;

        /// <summary>
        ///     Wartezeit in Runden zwischen zwei Respawns von Ameisen.
        /// </summary>
        public int AntRespawnDelay = 1;

        #endregion

        #region Marker Management

        /// <summary>
        ///     GIbt die maximale Dauer einer Markierung in Runden an. Mit zunahme
        ///     der Markierungsgröße, wird dieser Wert proportional kleiner.
        /// </summary>
        public int MarkerDuration = 100;

        /// <summary>
        ///     Gibt die maximale Größe einer Markierung an.
        ///     TODO: use this
        /// </summary>
        public int MarkerMaxSize = 1000;

        /// <summary>
        ///     Gibt die minimale Größe einer Markierung an.
        ///     TODO: use this
        /// </summary>
        public int MarkerMinSize = 100;

        /// <summary>
        ///     Gibt an, ob eine Ameise in der Lage ist, Markierungen von
        ///     Alliierten zu riechen.
        ///     TODO: use this
        /// </summary>
        public bool SmellsAlianceMarker = false;

        /// <summary>
        ///     Gibt an, ob eine Ameise in der Lage ist, gegnerische Markierungen
        ///     zu riechen.
        ///     TODO: use this
        /// </summary>
        public bool SmellsForeignMarker = false;

        #endregion

        #region Ant Behavior

        /// <summary>
        /// Maximalgeschwindigkeit einer Ameise
        /// </summary>
        public float ANT_MAX_SPEED = 1f;

        /// <summary>
        /// Radius des Ameisenkörpers
        /// </summary>
        public float ANT_RADIUS = 2f;

        /// <summary>
        /// Initiale Hitpoints der Ameise bei Geburt
        /// </summary>
        public int ANT_HITPOINTS = 100;

        /// <summary>
        /// Reichweite der Ameise (Für Collect, Carry und Attack)
        /// </summary>
        public float ANT_RANGE = 3f;

        /// <summary>
        /// Kollisionsmasse einer Ameise
        /// </summary>
        public float ANT_MASS = 1f;

        /// <summary>
        /// Angriffsstärke der Ameise (Schaden pro Schlag)
        /// </summary>
        public int ANT_ATTACK_STRENGHT = 5;

        /// <summary>
        /// Erholungszeit pro Schlag
        /// </summary>
        public int ANT_ATTACK_RECOVERY = 2;

        /// <summary>
        /// Transport-Stärke der Ameise (wichtig für Carry)
        /// </summary>
        public float ANT_STRENGHT = 5f;

        /// <summary>
        /// Können Äpfel "abgebaut" werden?
        /// </summary>
        public bool ANT_APPLECOLLECT = true;

        /// <summary>
        /// Maximale Ladung Zucker für eine Ameise
        /// </summary>
        public int ANT_SUGAR_CAPACITY = 5;

        /// <summary>
        /// Maximale Ladung Apfelfragmente für eine Ameise
        /// </summary>
        public int ANT_APPLE_CAPACITY = 2;

        /// <summary>
        /// Sichtweite einer Ameise
        /// </summary>
        public float ANT_VIEW_RANGE = 20f;

        /// <summary>
        /// Sichtbereich einer Ameise (Sichtkegel)
        /// </summary>
        public int ANT_VIEW_ANGLE = 360;

        /// <summary>
        /// Drehgeschwindigkeit in Grad pro Runde
        /// </summary>
        public int ANT_ROTATIONSPEED = 20;

        /// <summary>
        /// Richtungsstreuung beim Zickzack-Weg
        /// </summary>
        public int ANT_ZICKZACKANGLE = 10;

        /// <summary>
        /// Etappenlänge beim Zickzack-Weg
        /// </summary>
        public int ANT_ZICKZACKRANGE = 30;

        /// <summary>
        /// Erzeugt einen kleinen neuen Zuckerberg, wenn Zucker fallen gelassen wird
        /// </summary>
        public bool ANT_DROP_SUGARHEAP = false;

        #endregion
    }
}