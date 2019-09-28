using System.Threading.Tasks;

namespace AntMe.Runtime.Communication
{
    public interface ISimulationCallback
    {
        void MasterChanged(int id);

        void UserlistChanged(UserProfile[] users);

        void UserAdded(UserProfile user);

        void UserDropped(int id);

        void UsernameChanged(UserProfile user);

        void MessageReceived(UserProfile sender, string message);

        void LevelChanged(TypeInfo level);

        void PlayerReset(Slot[] slots);

        void PlayerChanged(Slot slot);

        void SimulationChanged(SimulationState state, byte framerate);

        void SimulationState(byte[] state);
    }
}
