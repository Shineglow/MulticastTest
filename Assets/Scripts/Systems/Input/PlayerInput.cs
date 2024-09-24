using UnityEngine;
using Zenject;

namespace Network.Systems.Input
{
    public class PlayerInput : INetworkPlayerInput, ITickable
    {
        public float Horizontal => _combinedAxesCached.x;
        public float Vertical => _combinedAxesCached.z;

        public Vector3 CombinedAxes => _combinedAxesCached;
        public ButtonInfoNetwork LevelUpButtonInfo => _inputUI.LevelUpButtonInfo;
        
        private Vector3 _combinedAxesCached;
        private readonly INetworkInputUI _inputUI;

        public PlayerInput(INetworkInputUI inputUI)
        {
            _inputUI = inputUI;
        }

        public void Tick()
        {
            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
                _combinedAxesCached.z = 1;

            if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
                _combinedAxesCached.z = -1;

            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
                _combinedAxesCached.x = -1;

            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
                _combinedAxesCached.x = 1;
        }

        public void OnInputReset()
        {
            _combinedAxesCached = Vector3.zero;
        }
    }
}