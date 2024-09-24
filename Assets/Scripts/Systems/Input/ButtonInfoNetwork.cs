namespace Network.Systems.Input
{
    public struct ButtonInfoNetwork
    {
        public EActions Action;
        public byte ClicksCount;
        public bool WasClicked => ClicksCount > 0;
    }
}