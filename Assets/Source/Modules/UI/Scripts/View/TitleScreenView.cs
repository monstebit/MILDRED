using Source.Modules.UI.Scripts.Presenter;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.UI.Scripts.View
{
    public class TitleScreenView : MonoBehaviour
    {
        [SerializeField] private Button _pressStartButton;
        [SerializeField] private Button _pressStartAsClientButton;  //  ON TESTING
        [SerializeField] private Button _startNewGameButton;

        private TitleScreenPresenter _titleScreenPresenter;

        public void Init(TitleScreenPresenter titleScreenPresenter)
        {
            _titleScreenPresenter = titleScreenPresenter;
            _pressStartButton.onClick.AddListener(_titleScreenPresenter.OnPressStartButtonPressed);
            _pressStartAsClientButton.onClick.AddListener(_titleScreenPresenter.OnPressStartButtonAsClientPressed); //  ON TESTING
            _startNewGameButton.onClick.AddListener(_titleScreenPresenter.OnPressNewGameButtonPressed);
        }

        private void OnDisable()
        {
            if (_titleScreenPresenter != null)
            {
                _pressStartButton.onClick.RemoveListener(_titleScreenPresenter.OnPressStartButtonPressed);
                _pressStartAsClientButton.onClick.RemoveListener(_titleScreenPresenter.OnPressStartButtonAsClientPressed);  //  ON TESTING
                _startNewGameButton.onClick.RemoveListener(_titleScreenPresenter.OnPressNewGameButtonPressed);
            }
        }

        public void ShowStartNewGameButton()
        {
            HideButton(_pressStartButton);
            HideButton(_pressStartAsClientButton);  //  ON TESTING
            ShowButton(_startNewGameButton);
        }
        
        public void HideStartNewGameButton()
        {
            HideButton(_startNewGameButton);
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