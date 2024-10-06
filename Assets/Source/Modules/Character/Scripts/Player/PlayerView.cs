using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        private bool IsOwner => _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner; 
        
        private const string IsGrounded = "IsGrounded";
        private const string IsMoving = "IsMoving";
        private const string IsStaticAction = "IsStaticAction";
        private const string IsLanding = "IsLanding";
        private const string IsAirborne = "IsAirborne";
        private const string IsIdling = "IsIdling";
        private const string IsWalking = "IsWalking";
        private const string IsRunning = "IsRunning";
        private const string IsSprinting = "IsSprinting";
        private const string IsDodging = "IsDodging";
        private const string IsBackStepping = "IsBackStepping";
        private const string IsLightLanding = "IsLightLanding";
        private const string IsJumping = "IsJumping";
        private const string IsFalling = "IsFalling";
        
        public Animator Animator;
        
        public void Initialize() => Animator = GetComponent<Animator>();
        
        public void UpdateNetworkTransform()
        {
            var playerNetworkSynchronizer = _playerCompositionRoot.PlayerNetworkSynchronizer;
            var playerView = _playerCompositionRoot.PlayerView;
            
            if (IsOwner)
            {
                playerNetworkSynchronizer.NetworkPosition.Value = playerView.transform.position;
                playerNetworkSynchronizer.NetworkRotation.Value = playerView.transform.rotation;
            }
            else
            {
                playerView.transform.position = Vector3.SmoothDamp(
                    playerView.transform.position,
                    playerNetworkSynchronizer.NetworkPosition.Value,
                    ref playerNetworkSynchronizer.NetworkPositionVelocity,
                    playerNetworkSynchronizer.NetworkPositionSmoothTime);

                playerView.transform.rotation = Quaternion.Slerp(
                    playerView.transform.rotation,
                    playerNetworkSynchronizer.NetworkRotation.Value,
                    playerNetworkSynchronizer.NetworkRotationSmoothTime);
            }
        }
        
        public void UpdateAnimatorMovementParameters()
        {
            int vertical = Animator.StringToHash("Vertical");
            int horizontal = Animator.StringToHash("Horizontal");
            Animator.SetFloat(horizontal, 0f, 0.1f, Time.deltaTime);
            Animator.SetFloat(
                vertical, 
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value, 
                0.1f, 
                Time.deltaTime);
        }
        
        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerView: No player composition root");
            }
        }

        private void OnEnable()
        {
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsGrounded.OnValueChanged += OnIsGroundedChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsMoving.OnValueChanged += OnIsMovingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsStaticAction.OnValueChanged += OnIsStaticActionChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsLanding.OnValueChanged += OnIsLandingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsAirborne.OnValueChanged += OnIsAirborneChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsIdling.OnValueChanged += OnIsIdlingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsWalking.OnValueChanged += OnIsWalkingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsRunning.OnValueChanged += OnIsRunningChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsSprinting.OnValueChanged += OnIsSprintingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsDodging.OnValueChanged += OnIsDodgingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsBackStepping.OnValueChanged += OnIsBackSteppingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsLightLanding.OnValueChanged += OnIsLightLandingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsJumping.OnValueChanged += OnIsJumpingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsFalling.OnValueChanged += OnIsFallingChanged;
        }

        private void OnDisable()
        {
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsGrounded.OnValueChanged -= OnIsGroundedChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsMoving.OnValueChanged -= OnIsMovingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsStaticAction.OnValueChanged -= OnIsStaticActionChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsLanding.OnValueChanged -= OnIsLandingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsAirborne.OnValueChanged -= OnIsAirborneChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsIdling.OnValueChanged -= OnIsIdlingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsWalking.OnValueChanged -= OnIsWalkingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsRunning.OnValueChanged -= OnIsRunningChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsSprinting.OnValueChanged -= OnIsSprintingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsDodging.OnValueChanged -= OnIsDodgingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsBackStepping.OnValueChanged -= OnIsBackSteppingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsLightLanding.OnValueChanged -= OnIsLightLandingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsJumping.OnValueChanged -= OnIsJumpingChanged;
            _playerCompositionRoot.PlayerNetworkSynchronizer.IsFalling.OnValueChanged -= OnIsFallingChanged;
        }
        
        public void StartGrounded()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsGrounded.Value = true;
            }
        }
        
        public void StopGrounded()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsGrounded.Value = false;
            }
        }

        public void StartMoving()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsMoving.Value = true;
            }
        }

        public void StopMoving()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsMoving.Value = false;
            }
        }
        
        public void StartStaticAction()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsStaticAction.Value = true;
            }
        }
        
        public void StopStaticAction()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsStaticAction.Value = false;
            }
        }

        public void StartLanding()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLanding.Value = true;
            }
        }

        public void StopLanding()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLanding.Value = false;
            }
        }

        public void StartAirborne()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsAirborne.Value = true;
            }
        }

        public void StopAirborne()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsAirborne.Value = false;
            }
        }

        public void StartIdling()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsIdling.Value = true;
            }
        }

        public void StopIdling()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsIdling.Value = false;
            }
        }

        public void StartWalking()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsWalking.Value = true;
            }
        }

        public void StopWalking()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsWalking.Value = false;
            }
        }

        public void StartRunning()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsRunning.Value = true;
            }
        }

        public void StopRunning()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsRunning.Value = false;
            }
        }

        public void StartSprinting()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsSprinting.Value = true;
            }
        }

        public void StopSprinting()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsSprinting.Value = false;
            }
        }

        public void StartDodging()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsDodging.Value = true;
            }
        }

        public void StopDodging()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsDodging.Value = false;
            }
        }

        public void StartBackStepping()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsBackStepping.Value = true;
            }
        }
        
        public void StopBackStepping()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsBackStepping.Value = false;
            }
        }

        public void StartLightLanding()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLightLanding.Value = true;
            }
        }

        public void StopLightLanding()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLightLanding.Value = false;
            }
        }
        
        public void StartJumping()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsJumping.Value = true;
            }
        }

        public void StopJumping()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsJumping.Value = false;
            }
        }
        
        public void StartFalling()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsFalling.Value = true;
            }
        }

        public void StopFalling()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsFalling.Value = false;
            }
        }
        
        private void OnIsGroundedChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsGrounded, newValue);
        }
        
        private void OnIsMovingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsMoving, newValue);
        }

        private void OnIsStaticActionChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsStaticAction, newValue);
        }

        private void OnIsLandingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsLanding, newValue);
        }

        private void OnIsAirborneChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsAirborne, newValue);
        }

        private void OnIsIdlingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsIdling, newValue);
        }

        private void OnIsWalkingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsWalking, newValue);
        }

        private void OnIsRunningChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsRunning, newValue);
        }

        private void OnIsSprintingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsSprinting, newValue);
        }

        private void OnIsDodgingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsDodging, newValue);
        }

        private void OnIsBackSteppingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsBackStepping, newValue);
        }

        private void OnIsLightLandingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsLightLanding, newValue);
        }

        private void OnIsJumpingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsJumping, newValue);
        }

        private void OnIsFallingChanged(bool previousValue, bool newValue)
        {
            Animator.SetBool(IsFalling, newValue);
        }
    }
}