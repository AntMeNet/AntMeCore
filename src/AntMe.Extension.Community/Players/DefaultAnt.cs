using AntMe.Basics.Factions.Ants;
using AntMe.Basics.Factions.Ants.Interop;
using AntMe.Basics.ItemProperties;
using AntMe.Basics.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Extension.Community.Players
{
    public class DefaultAnt : AntUnit
    {
        private AntUnitInterop interop;
        private AntMovementInterop movement;
        private RecognitionInterop recognition;
        private InteractionInterop interaction;

        public override void Init(UnitInterop interop)
        {
            this.interop = interop as AntUnitInterop;

            movement = interop.GetProperty<AntMovementInterop>();
            if (movement == null)
                throw new ArgumentException("No Movement Interop");

            recognition = interop.GetProperty<RecognitionInterop>();
            if (recognition == null)
                throw new ArgumentException("No Recognition Interop");

            interaction = interop.GetProperty<InteractionInterop>();
            if (interaction == null)
                throw new ArgumentException("No Interaction Interop");

            this.interop.Tick += Interop_Tick;

            movement.OnWaits += Movement_OnWaits;
            movement.OnHitWall += Movement_OnHitWall;
            movement.OnCollision += Movement_OnCollision;
            movement.OnTargetReched += Movement_OnTargetReched;

            recognition.OnEnvironmentChanged += Recognition_OnEnvironmentChanged;
            recognition.Smells += Recognition_Smells;
            recognition.Spots += Recognition_Spots;

            interaction.OnHit += Interaction_OnHit;
            interaction.OnKill += Interaction_OnKill;
        }

        private void Interop_Tick()
        {
        }

        private void Interaction_OnKill()
        {
        }

        private void Interaction_OnHit(int parameter)
        {
            
        }

        private void Recognition_Spots()
        {
            var sugar = recognition.VisibleItems.OfType<SugarInfo>().FirstOrDefault();
            Console.WriteLine("Spotted " + recognition.VisibleItems.Count());
            if (sugar != null && movement.CurrentDestination == null)
                movement.GoTo(sugar);

            var apple = recognition.VisibleItems.OfType<AppleInfo>().FirstOrDefault();
            if (apple != null && movement.CurrentDestination == null)
                movement.GoTo(apple);
        }

        private void Recognition_Smells()
        {
        }

        private void Recognition_OnEnvironmentChanged(VisibleEnvironment environment)
        {
            
        }

        private void Movement_OnTargetReched(ItemInfo target)
        {
            
        }

        private void Movement_OnCollision()
        {
            
        }

        private void Movement_OnHitWall(Compass parameter)
        {
            Console.WriteLine(string.Format("Ant {0} hits wall in {1} direction", interop.Id, parameter));
        }

        private void Movement_OnWaits()
        {
            movement.Turn(interop.Random.Next(-50, 50));
            movement.Goahead(200);
        }
    }
}
