using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class CameraMovement : MonoBehaviour
    {
        public static CameraMovement instance;

        public Camera CameraObject;
        public Transform cameraPivotTransform;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}