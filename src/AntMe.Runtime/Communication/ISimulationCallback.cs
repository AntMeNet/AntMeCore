using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    internal interface ISimulationCallback
    {
        [OperationContract(IsOneWay = true)]
        void MasterChanged(int id);

        [OperationContract(IsOneWay = true)]
        void UserlistChanged(UserProfile[] users);

        [OperationContract(IsOneWay = true)]
        void UserAdded(UserProfile user);

        [OperationContract(IsOneWay = true)]
        void UserDropped(int id);

        [OperationContract(IsOneWay = true)]
        void UsernameChanged(UserProfile user);

        [OperationContract(IsOneWay = true)]
        void MessageReceived(UserProfile sender, string message);

        [OperationContract(IsOneWay = true)]
        void LevelChanged(TypeInfo level);

        [OperationContract(IsOneWay = true)]
        void PlayerReset(Slot[] slots);

        [OperationContract(IsOneWay = true)]
        void PlayerChanged(Slot slot);

        [OperationContract(IsOneWay = true)]
        void SimulationChanged(SimulationState state, byte framerate);

        [OperationContract(IsOneWay = true)]
        void SimulationState(byte[] state);
    }
}