using AntMe.Simulation.Factions.Ants;
using AntMe.Simulation.Factions.Ants.Interop;
using AntMe.Simulation.Items;
using System;
using System.Linq;

namespace AntMe.Simulation.Debug
{
    public sealed class RawAnt : PrimordialAnt
    {
        private AntInterop _interop;
        private PhysicsInterop _physics;
        private InteractionInterop _interaction;
        private RecognitionInterop _recognition;

        public override void Init(AntInterop interop)
        {
            _interop = interop;
            _recognition = interop.GetProperty<RecognitionInterop>();
            _physics = interop.GetProperty<PhysicsInterop>();
            _interaction = interop.GetProperty<InteractionInterop>();

            _physics.OnWaits += interop_Waits;
            _interop.Tick += interop_Tick;
            _recognition.Spots += interop_Spots;
            _physics.OnCollision += interop_Collided;
            _recognition.Smells += interop_Smells;
            _physics.OnTargetReched += interop_Reached;
        }

        void interop_Reached(ItemInfo parameter)
        {
            if (_physics.CurrentTarget is AnthillInfo)
            {
                var hill = _physics.CollidedItems.FirstOrDefault(i => i is AnthillInfo);
                if (hill != null)
                {
                    _physics.Stop();
                }
            }

            var sugar = _physics.CollidedItems.FirstOrDefault(i => i is SugarInfo);
            if (sugar != null)
            {
                _interaction.Collect(sugar);
                _physics.GoToAnthill();
            }
        }

        void interop_Smells()
        {
        }

        void interop_Collided()
        {
            
        }

        void interop_Spots()
        {
            if (_physics.CurrentTarget == null)
            {
                var sugar = _recognition.VisibleItems.FirstOrDefault(i => i is SugarInfo);
                if (sugar != null)
                {
                    _physics.GoTo(sugar);
                }
            }
        }

        private void interop_Waits()
        {
            _physics.Turn(_interop.Random.Next(-60, 60));
            _physics.Goahead(_interop.Random.Next(20, 50));
        }

        void interop_Tick()
        {
        }
    }
}
