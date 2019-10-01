using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    [CallbackBehavior(
        ConcurrencyMode = ConcurrencyMode.Single,
        UseSynchronizationContext = false
    )]
    internal sealed class WcfSimulationCallback : ISimulationCallback
    {
        public delegate void CallbackDelegate<T>(T parameter);

        public delegate void CallbackDelegate<T, V>(T parameter1, V parameter2);

        public void MasterChanged(int id)
        {
            OnMasterChanged?.Invoke(id);
        }

        public void UserlistChanged(UserProfile[] users)
        {
            OnUserlistChanged?.Invoke(users);
        }

        public void UserAdded(UserProfile user)
        {
            OnUserAdded?.Invoke(user);
        }

        public void UserDropped(int id)
        {
            OnUserDropped?.Invoke(id);
        }

        public void UsernameChanged(UserProfile user)
        {
            OnUsernameChanged?.Invoke(user);
        }

        public void MessageReceived(UserProfile sender, string message)
        {
            OnMessageReceived?.Invoke(sender, message);
        }

        public void LevelChanged(TypeInfo level)
        {
            OnLevelChanged?.Invoke(level);
        }

        public void PlayerReset(Slot[] slots)
        {
            OnPlayerReset?.Invoke(slots);
        }

        public void PlayerChanged(Slot slot)
        {
            OnPlayerChanged?.Invoke(slot);
        }

        public void SimulationChanged(SimulationState state, byte framerate)
        {
            OnSimulationChanged?.Invoke(state, framerate);
        }

        public void SimulationState(byte[] state)
        {
            OnSimulationState?.Invoke(state);
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
    }
}