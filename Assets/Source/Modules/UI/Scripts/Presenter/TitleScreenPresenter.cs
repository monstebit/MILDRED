using System;
using Source.Modules.Character.Scripts;
using Source.Modules.UI.Scripts.Model;
using Source.Modules.UI.Scripts.View;

namespace Source.Modules.UI.Scripts.Presenter
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