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

            HideButton(_pressStartButton);
            ShowButton(_startNewGameButton);
        }
        
        private void OnStartNewGameButtonHandleClick()
        {
            StartNewGameButtonPressed?.Invoke();
        }

        private void HideButton(Button button)
        {
            button.gameObject.SetActive(false);
        }
        
        private void ShowButton(Button button)
        {
            button.gameObject.SetActive(true);
        }
    }
}