using UnityEngine;
using UnityEngine.UI;

namespace UU.MILDRED.UI
{
    public class TitleScreenView : MonoBehaviour
    {
        [SerializeField] private Button _pressStartButton;
        [SerializeField] private Button _startNewGameButton;

        private TitleScreenPresenter _titleScreenPresenter;

        public void Init(TitleScreenPresenter titleScreenPresenter)
        {
            _titleScreenPresenter = titleScreenPresenter;
            _pressStartButton.onClick.AddListener(_titleScreenPresenter.OnPressStartButtonPressed);
            _startNewGameButton.onClick.AddListener(_titleScreenPresenter.OnPressNewGameButtonPressed);
        }

        private void OnDisable()
        {
            if (_titleScreenPresenter != null)
            {
                _pressStartButton.onClick.RemoveListener(_titleScreenPresenter.OnPressStartButtonPressed);
                _startNewGameButton.onClick.RemoveListener(_titleScreenPresenter.OnPressNewGameButtonPressed);
            }
        }

        public void ShowStartNewGameButton()
        {
            HideButton(_pressStartButton);
            ShowButton(_startNewGameButton);
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