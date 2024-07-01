using Source.Modules.Character.Scripts;
using Source.Modules.UI.Scripts.Model;
using Source.Modules.UI.Scripts.Presenter;
using Source.Modules.UI.Scripts.View;
using UnityEngine;

namespace Source.Modules.UI.Scripts
{
    public class TitleScreenCompositionRoot : MonoBehaviour
    {
        [SerializeField] private TitleScreenView _titleScreenView;
        [SerializeField] private NetworkLauncher _networkLauncher;
        [SerializeField] private SceneLoader _sceneLoader;

        private TitleScreenPresenter _titleScreenPresenter;

        private void Awake()
        {
            _titleScreenPresenter = new TitleScreenPresenter(_titleScreenView, _sceneLoader, _networkLauncher);
        }

        private void OnDisable()
        {
            // Если требуется, можете добавить код для очистки ресурсов
            _titleScreenPresenter = null;
        }
    }
}