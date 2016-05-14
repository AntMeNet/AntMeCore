using AntMe.Basics.Factions.Ants;
using AntMe.Basics.Factions.Ants.Interop;

namespace AntMe.Extension.Community.Players
{
    public class DefaultAnt : AntUnit
    {
        private AntUnitInterop interop;
        private AntMovementInterop movement;

        public override void Init(UnitInterop interop)
        {
            this.interop = interop as AntUnitInterop;
            movement = interop.GetProperty<AntMovementInterop>();

            movement.OnWaits += Movement_OnWaits;
        }

        private void Movement_OnWaits()
        {
            movement.Turn(interop.Random.Next(-30, 30));
            movement.Goahead(200);
        }
    }
}
