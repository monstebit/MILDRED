using UnityEngine;
using UU.MILDRED.Character;

namespace UU.MILDRED.UI
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