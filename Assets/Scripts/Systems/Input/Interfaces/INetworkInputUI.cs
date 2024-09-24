using Network.UI;

namespace Network.Systems.Input
{
    public interface INetworkInputUI : IInputUI, INetworkResetable, IView
    {
        ButtonInfoNetwork LevelUpButtonInfo { get; }
        void OnInputReset();
    }
}