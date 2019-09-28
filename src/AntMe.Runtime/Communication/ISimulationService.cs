using System.Threading.Tasks;

namespace AntMe.Runtime.Communication
{
    public interface ISimulationService
    {
        Task<int> Hello(string username);
        Task Goodbye();

        #region Methoden

        #region Master Handling

        Task AquireMaster();

        Task FreeMaster();

        #endregion

        #region User Handling

        Task ChangeUsername(string name);

        #endregion

        #region Chat Handling

        Task SendMessage(string message);

        #endregion

        #region Settings Handling

        Task UploadLevel(TypeInfo level);

        Task UploadPlayer(TypeInfo player);

        Task UploadMaster(byte slot, TypeInfo player);

        Task SetPlayerState(byte slot, PlayerColor color, byte team, bool ready);

        Task UnsetPlayerState();

        Task SetMasterState(byte slot, PlayerColor color, byte team, bool ready);

        #endregion

        #region Flow Handling

        Task StartSimulation();

        Task PauseSimulation();

        Task ResumeSimulation();

        Task StopSimulation();

        Task PitchSimulation(byte frames);

        #endregion

        #endregion

    }
}