using UnityEngine;
using UU.MILDRED.Character;

namespace UU.MILDRED.UI
{
    public class TitleScreenCompositionRoot : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private NetworkLauncher _networkLauncher;
        [SerializeField] private TitleScreenView _titleScreenView;
        
        private TitleScreenPresenter _titleScreenPresenter;

        private void OnEnable()
        {
            _titleScreenPresenter = new TitleScreenPresenter(_sceneLoader, _networkLauncher, _titleScreenView);
        }

        private void OnDisable()
        {
            if (_titleScreenPresenter is not null)
            {
                _titleScreenPresenter.Dispose();
                _titleScreenPresenter = null;
            }
        }
    }
}