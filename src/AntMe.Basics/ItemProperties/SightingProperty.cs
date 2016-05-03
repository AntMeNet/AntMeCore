using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für alle sehenden Spielelemente. Diese Eigenschaft
    ///     lässt das Spielelement die Umgebung betrachten (Bodenbeschaffenheit,
    ///     Höhenkarte und Randerkennung) und erlaubt das Erkennen von Elementen
    ///     mit VisibleProperty.
    /// </summary>
    public sealed class SightingProperty : ItemProperty
    {
        private readonly VisibleEnvironment environment = new VisibleEnvironment();
        private readonly List<VisibleProperty> visibleItems = new List<VisibleProperty>();
        private Angle viewDirection;
        private float viewangle;
        private float viewrange;

        public SightingProperty(Item item) : base(item)
        {
            //ViewRange = range;
            //ViewAngle = 360;
            //ViewDirection = Angle.Right;
            //ViewAngle = angle;
            //ViewDirection = direction;
        }

        /// <summary>
        ///     Liefert den Sichtradius des Elementes.
        /// </summary>
        [DisplayName("View Range")]
        [Description("")]
        public float ViewRange
        {
            get { return viewrange; }
            set
            {
                viewrange = Math.Max(0f, value);
                if (OnViewRangeChanged != null)
                    OnViewRangeChanged(Item, viewrange);
            }
        }

        /// <summary>
        ///     Liefert die Sichtrichtung des Elements.
        /// </summary>
        [DisplayName("View Direction")]
        [Description("")]
        public Angle ViewDirection
        {
            get { return viewDirection; }
            set
            {
                viewDirection = value;
                if (OnViewDirectionChanged != null)
                    OnViewDirectionChanged(Item, value);
            }
        }

        /// <summary>
        ///     Liefert den Öffnungswinkel des Sichtkegels.
        ///     0 = Element kann nichts sehen
        ///     90 = Sieht Elemente die sich zwischen -45 bis 45 Grad seiner Blickrichtung befinden
        ///     360 = Sieht alle Elemente innerhalb des Sichtradius
        /// </summary>
        [DisplayName("View Angle")]
        [Description("")]
        public float ViewAngle
        {
            get { return viewangle; }
            set
            {
                viewangle = Math.Max(0f, Math.Min(360, value));
                if (OnViewAngleChanged != null)
                    OnViewAngleChanged(Item, viewangle);
            }
        }

        /// <summary>
        ///     Liefert die sichtbare Umgebung in Form eines Zellengrids. Ist die
        ///     Zelle null, befindet sich an dieser Stelle der Abgrund. Ansonsten
        ///     enthält die Zelle Informationen zur Höhe und zum Fortbewegungs-
        ///     modifikator.
        /// </summary>
        [DisplayName("Environment")]
        [Description("")]
        public VisibleEnvironment Environment
        {
            get { return environment; }
        }

        /// <summary>
        ///     Öffentlich sichtbare readonly list von visibleItems
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<VisibleProperty> VisibleItems
        {
            get { return visibleItems.AsReadOnly(); }
        }

        #region Internal Calls

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn sich ein Element in den
        ///     sichtbaren Radius bewegt.
        /// </summary>
        /// <param name="item">Neu sichtbares Element</param>
        internal void AddVisibleItem(VisibleProperty item)
        {
            if (!visibleItems.Contains(item))
            {
                visibleItems.Add(item);

                if (OnNewVisibleItem != null)
                    OnNewVisibleItem(item);
            }
        }

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn ein Element sich aus dem
        ///     sichtbaren Radius entfernt.
        /// </summary>
        /// <param name="item">Nicht mehr sichtbares Element</param>
        internal void RemoveVisibleItem(VisibleProperty item)
        {
            if (visibleItems.Contains(item))
            {
                visibleItems.Remove(item);

                if (OnLostVisibleItem != null)
                    OnLostVisibleItem(item);
            }
        }

        /// <summary>
        ///     Wird von der Extension in jeder Runde für jedes sichtbare Element aufgerufen.
        /// </summary>
        /// <param name="item">Sichtbares Element</param>
        internal void NoteVisibleItem(VisibleProperty item)
        {
            if (OnVisibleItem != null)
                OnVisibleItem(item);
        }

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn die Zelle sich ändert und
        ///     die Umgebung von der Extension geändert wurde.
        /// </summary>
        internal void RefreshEnvironment()
        {
            if (OnEnvironmentChanged != null)
                OnEnvironmentChanged(Item, Environment);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Event, das bei Änderung der Viewrange geworfen werden muss.
        /// </summary>
        public event ValueChanged<float> OnViewRangeChanged;

        /// <summary>
        ///     Event, das bei Änderung der Sichtrichtung geworfen werden muss.
        /// </summary>
        public event ValueChanged<Angle> OnViewDirectionChanged;

        /// <summary>
        ///     Event, das bei Änderung des Sichtkegels geworfen werden muss.
        /// </summary>
        public event ValueChanged<float> OnViewAngleChanged;

        /// <summary>
        ///     Event, das die Änderung der Umgebung (durch Zellenwechsel) ankündigt.
        /// </summary>
        public event ValueChanged<VisibleEnvironment> OnEnvironmentChanged;

        /// <summary>
        ///     Event, das über ein sichtbares Item informiert, das neu in die
        ///     Liste gekommen ist.
        /// </summary>
        public event ChangeItem<VisibleProperty> OnNewVisibleItem;

        /// <summary>
        ///     Event, das über ein sichtbares Item informiert, das aus der Liste
        ///     geflogen ist.
        /// </summary>
        public event ChangeItem<VisibleProperty> OnLostVisibleItem;

        /// <summary>
        ///     Event, das über ein sichtbares Item informiert, jeden Tick in dem es sichtbar ist.
        /// </summary>
        public event ChangeItem<VisibleProperty> OnVisibleItem;

        #endregion
    }
}