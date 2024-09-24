using System;
using UnityEngine;
using UnityEngine.UI;

namespace Network.Systems.Input
{
    public class InputUI : MonoBehaviour, INetworkInputUI
    {
        [SerializeField] 
        private Button _levelUpButton;

        private ButtonInfoNetwork _levelUpButtonInfoNetwork;
        public ButtonInfoNetwork LevelUpButtonInfo => _levelUpButtonInfoNetwork;

        private void Awake()
        {
            if (_levelUpButton == null) throw new NullReferenceException($"{nameof(_levelUpButton)} must be initialized!");
            _levelUpButtonInfoNetwork.Action = EActions.LevelUpClicked;
            _levelUpButton.onClick.AddListener(OnLevelUpButtonClicked);
        }

        private void OnLevelUpButtonClicked() => _levelUpButtonInfoNetwork.ClicksCount++;
        public void OnInputReset()
        {
            _levelUpButtonInfoNetwork.ClicksCount = 0;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}