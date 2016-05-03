using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    [CallbackBehavior(
        ConcurrencyMode = ConcurrencyMode.Single,
        UseSynchronizationContext = false
    )]
    internal sealed class WcfSimulationCallback : ISimulationCallback
    {
        public void MasterChanged(int id)
        {
            if (OnMasterChanged != null)
                OnMasterChanged(id);
        }

        public void UserlistChanged(UserProfile[] users)
        {
            if (OnUserlistChanged != null)
                OnUserlistChanged(users);
        }

        public void UserAdded(UserProfile user)
        {
            if (OnUserAdded != null)
                OnUserAdded(user);
        }

        public void UserDropped(int id)
        {
            if (OnUserDropped != null)
                OnUserDropped(id);
        }

        public void UsernameChanged(UserProfile user)
        {
            if (OnUsernameChanged != null)
                OnUsernameChanged(user);
        }

        public void MessageReceived(UserProfile sender, string message)
        {
            if (OnMessageReceived != null)
                OnMessageReceived(sender, message);
        }

        public void LevelChanged(TypeInfo level)
        {
            if (OnLevelChanged != null)
                OnLevelChanged(level);
        }

        public void PlayerReset(Slot[] slots)
        {
            if (OnPlayerReset != null)
                OnPlayerReset(slots);
        }

        public void PlayerChanged(Slot slot)
        {
            if (OnPlayerChanged != null)
                OnPlayerChanged(slot);
        }

        public void SimulationChanged(SimulationState state, byte framerate)
        {
            if (OnSimulationChanged != null)
                OnSimulationChanged(state, framerate);
        }

        public void SimulationState(byte[] state)
        {
            if (OnSimulationState != null)
                OnSimulationState(state);
        }

        public event CallbackDelegate<int> OnMasterChanged;

        public event CallbackDelegate<UserProfile[]> OnUserlistChanged;

        public event CallbackDelegate<UserProfile> OnUserAdded;

        public event CallbackDelegate<int> OnUserDropped;

        public event CallbackDelegate<UserProfile> OnUsernameChanged;

        public event CallbackDelegate<UserProfile, string> OnMessageReceived;

        public event CallbackDelegate<TypeInfo> OnLevelChanged;

        public event CallbackDelegate<Slot[]> OnPlayerReset;

        public event CallbackDelegate<Slot> OnPlayerChanged;

        public event CallbackDelegate<SimulationState, byte> OnSimulationChanged;

        public event CallbackDelegate<byte[]> OnSimulationState;

        public delegate void CallbackDelegate<T>(T parameter);
        public delegate void CallbackDelegate<T, V>(T parameter1, V parameter2);
    }
}
