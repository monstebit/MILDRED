using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Modules.Character.Scripts.Player
{public class PlayerCameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPivotTransform;
        [SerializeField] private float _playerCameraXRotation;
        [SerializeField] private float _playerCameraYRotation;
        
        [Header("FOLLOW SETTINGS")]
        [SerializeField] private float _smoothing = 10f;
        [SerializeField] private Vector3 _offset;

        public Transform CameraPivotTransform => _cameraPivotTransform;
        
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
            get => _smoothing;
            set => _smoothing = value;
        }
        
        public void FollowTarget(Transform target)
        {
            var nextPosition = Vector3.Lerp(
                transform.position, 
                target.position + _offset, 
                Time.deltaTime * _smoothing);
            
            transform.position = nextPosition;
        }
    }
}