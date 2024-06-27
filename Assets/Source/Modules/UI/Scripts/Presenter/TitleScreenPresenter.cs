using System;
using UnityEngine.SceneManagement;
using UU.MILDRED.Character;

namespace UU.MILDRED.UI
{
    public class TitleScreenPresenter : IDisposable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly NetworkLauncher _networkLauncher;
        private readonly TitleScreenView _titleScreenView;

        public TitleScreenPresenter(SceneLoader sceneLoader, NetworkLauncher networkLauncher,
            TitleScreenView titleScreenView)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _networkLauncher = networkLauncher ?? throw new ArgumentNullException(nameof(networkLauncher));
            _titleScreenView = titleScreenView ?? throw new ArgumentNullException(nameof(titleScreenView));

            _titleScreenView.PressStartButtonPressed += OnPressStartButtonPressed;
            _titleScreenView.StartNewGameButtonPressed += OnPressNewGameButtonPressed;
        }

        public void Dispose()
        {
            _titleScreenView.PressStartButtonPressed -= OnPressStartButtonPressed;
            _titleScreenView.StartNewGameButtonPressed -= OnPressNewGameButtonPressed;
        }

        private void OnPressStartButtonPressed()
        {
            _networkLauncher.StartNetworkAsHost();
        }

        private void OnPressNewGameButtonPressed()
        {
            _sceneLoader.StartWork();
        }
    }
}