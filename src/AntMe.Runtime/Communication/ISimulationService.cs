using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(ISimulationCallback)
    )]
    internal interface ISimulationService
    {
        [OperationContract(
            IsInitiating = true,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        int Hello(string username);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = true
        )]
        [FaultContract(typeof(AntMeFault))]
        void Goodbye();

        #region Methoden

        #region Master Handling

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void AquireMaster();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void FreeMaster();

        #endregion

        #region User Handling

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void ChangeUsername(string name);

        #endregion

        #region Chat Handling

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void SendMessage(string message);

        #endregion

        #region Settings Handling

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void UploadLevel(TypeInfo level);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void UploadPlayer(TypeInfo player);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void UploadMaster(byte slot, TypeInfo player);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void SetPlayerState(byte slot, PlayerColor color, byte team, bool ready);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void UnsetPlayerState();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void SetMasterState(byte slot, PlayerColor color, byte team, bool ready);

        #endregion

        #region Flow Handling

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void StartSimulation();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void PauseSimulation();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void ResumeSimulation();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void StopSimulation();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false
        )]
        [FaultContract(typeof(AntMeFault))]
        void PitchSimulation(byte frames);

        #endregion

        #endregion
    }
}