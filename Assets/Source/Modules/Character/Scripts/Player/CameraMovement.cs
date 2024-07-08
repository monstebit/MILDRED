using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class CameraMovement : MonoBehaviour
    {
        public Camera CameraObject;
        public Transform cameraPivotTransform;
        
        private void Start()
        {
            // DontDestroyOnLoad(gameObject);
        }
    }
}