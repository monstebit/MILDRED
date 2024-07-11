using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerCameraMovement : MonoBehaviour
    {
        public Camera CameraObject;
        public Transform CameraPivotTransform;
        public Vector3 CameraVelocity;
        // public Vector3 CameraObjectPosition;
        public float PlayerCameraXRotation;
        public float PlayerCameraYRotation;
        public float CameraSmoothSpeed = 0.125f;
        
        //  TODO: CAMERA COLLIDING LOGIC
        public float CameraZPosition;
        public float TargetCameraZPosition;
        
        private void Start()
        {
            // DontDestroyOnLoad(gameObject);
        }
    }
}