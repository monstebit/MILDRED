using System;
using UU.MILDRED.Character;

namespace UU.MILDRED.UI
{
    public class TitleScreenPresenter
    {
        private readonly SceneLoader _sceneLoader;
        private readonly NetworkLauncher _networkLauncher;
        private readonly TitleScreenView _titleScreenView;

        public TitleScreenPresenter(
            TitleScreenView titleScreenView,
            SceneLoader sceneLoader, 
            NetworkLauncher networkLauncher)
        {
            _titleScreenView = titleScreenView ?? throw new ArgumentNullException(nameof(titleScreenView));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _networkLauncher = networkLauncher ?? throw new ArgumentNullException(nameof(networkLauncher));
            
            _titleScreenView.Init(this);
        }

        public void OnPressStartButtonPressed()
        {
            _networkLauncher.StartNetworkAsHost();
            _titleScreenView.ShowStartNewGameButton();
        }

        public void OnPressNewGameButtonPressed()
        {
            _sceneLoader.StartWork();
        }
    }
}