using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{public class PlayerCameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPivotTransform;
        [SerializeField] private Vector3 _cameraVelocity;
        [SerializeField] private float _playerCameraXRotation;
        [SerializeField] private float _playerCameraYRotation;
        [SerializeField] private float _cameraSmoothSpeed = 0.125f;

        public Transform CameraPivotTransform => _cameraPivotTransform;
        public Vector3 CameraVelocity
        {
            get => _cameraVelocity;
            set => _cameraVelocity = value;
        }

        public float PlayerCameraXRotation
        {
            get => _playerCameraXRotation;
            set => _playerCameraXRotation = value;
        }

        public float PlayerCameraYRotation
        {
            get => _playerCameraYRotation;
            set => _playerCameraYRotation = value;
        }

        public float CameraSmoothSpeed
        {
            get => _cameraSmoothSpeed;
            set => _cameraSmoothSpeed = value;
        }
        
        public void FollowTarget(Transform target)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target.position,
                ref _cameraVelocity,
                CameraSmoothSpeed * Time.deltaTime);
        }
    }
}