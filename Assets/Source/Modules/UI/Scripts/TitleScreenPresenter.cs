using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UU.MILDRED.Character;

namespace UU.MILDRED.UI
{
    public class TitleScreenPresenter : IDisposable
    {
        private readonly NetworkLauncher _networkLauncher;
        private readonly TitleScreenView _titleScreenView;

        private int _worldSceneIndex = 1;

        public TitleScreenPresenter(NetworkLauncher networkLauncher, TitleScreenView titleScreenView)
        {
            _networkLauncher = networkLauncher ?? throw new ArgumentNullException(nameof(networkLauncher));
            _titleScreenView = titleScreenView ?? throw new ArgumentNullException(nameof(titleScreenView));

            _titleScreenView.PressStartButtonPressed += OnPressStartButtonPressed;
        }
        
        public void Dispose()
        {
            _titleScreenView.PressStartButtonPressed -= OnPressStartButtonPressed;
        }
        
        private void OnPressStartButtonPressed()
        {
            _networkLauncher.StartNetworkAsHost();
            SceneManager.LoadScene(_worldSceneIndex);
        }
    }
}