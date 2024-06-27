using System;
using UnityEngine;
using UnityEngine.UI;

namespace UU.MILDRED.UI
{
    public class TitleScreenView : MonoBehaviour
    {
        [SerializeField] private Button _pressStartButton;
        [SerializeField] private Button _startNewGameButton;
        
        public event Action PressStartButtonPressed;
        public event Action StartNewGameButtonPressed;
        
        private void Awake()
        {
            _pressStartButton.onClick.AddListener(OnPressStartButtonPressed);
            _startNewGameButton.onClick.AddListener(OnStartNewGameButtonHandleClick);
        }

        private void OnDisable()
        {
            _pressStartButton.onClick.RemoveListener(OnPressStartButtonPressed);
        }

        private void OnPressStartButtonPressed()
        {
            PressStartButtonPressed?.Invoke();
        }
        
        private void OnStartNewGameButtonHandleClick()
        {
            StartNewGameButtonPressed?.Invoke();
        }
    }
}