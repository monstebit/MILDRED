using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] PlayerCompositionRoot _playerCompositionRoot;
        
        [Header("State Group Parameter Names")]
        private const string IsGrounded = "IsGrounded";
        private const string IsMoving = "IsMoving";
        private const string IsStaticAction = "IsStaticAction";
        private const string IsLanding = "IsLanding";
        private const string IsAirborne = "IsAirborne";
        
        [Header("Grounded Parameter Names")]
        private const string IsIdling = "IsIdling";
        private const string IsWalking = "IsWalking";
        private const string IsRunning = "IsRunning";
        private const string IsSprinting = "IsSprinting";
        
        [Header("Static Action Parameter Names")]
        private const string IsDodging = "IsDodging";
        private const string IsBackStepping = "IsBackStepping";
        
        [Header("Landing Parameter Names")]
        private const string IsLightLanding = "IsLightLanding";
        
        [Header("Airborne Parameter Names")]
        private const string IsJumping = "IsJumping";
        private const string IsFalling = "IsFalling";
        
        public Animator Animator;
        
        public void Initialize() => Animator = GetComponent<Animator>();

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
            // Animator.SetBool(IsGrounded, true);
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsGrounded.Value = true;
            }
            else
            {
                Animator.SetBool(IsGrounded, true); // ENABLE PLAYER START STATE
            }
        }
        public void StopGrounded()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsGrounded.Value = false;
            }
        }

        public void StartMoving()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsMoving.Value = true;
            }
        }

        public void StopMoving()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsMoving.Value = false;
            }
        }
        
        public void StartStaticAction()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsStaticAction.Value = true;
            }
        }
        public void StopStaticAction()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsStaticAction.Value = false;
            }
        }

        public void StartLanding()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLanding.Value = true;
            }
        }

        public void StopLanding()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLanding.Value = false;
            }
        }

        public void StartAirborne()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsAirborne.Value = true;
            }
        }

        public void StopAirborne()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsAirborne.Value = false;
            }
        }

        public void StartIdling()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsIdling.Value = true;
            }
        }

        public void StopIdling()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsIdling.Value = false;
            }
        }

        public void StartWalking()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsWalking.Value = true;
            }
        }

        public void StopWalking()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsWalking.Value = false;
            }
        }

        public void StartRunning()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsRunning.Value = true;
            }
        }

        public void StopRunning()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsRunning.Value = false;
            }
        }

        public void StartSprinting()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsSprinting.Value = true;
            }
        }

        public void StopSprinting()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsSprinting.Value = false;
            }
        }

        public void StartDodging()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsDodging.Value = true;
            }
        }

        public void StopDodging()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsDodging.Value = false;
            }
        }

        public void StartBackStepping()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsBackStepping.Value = true;
            }
        }
        public void StopBackStepping()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsBackStepping.Value = false;
            }
        }

        public void StartLightLanding()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLightLanding.Value = true;
            }
        }

        public void StopLightLanding()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsLightLanding.Value = false;
            }
        }
        
        public void StartJumping()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsJumping.Value = true;
            }
        }

        public void StopJumping()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsJumping.Value = false;
            }
        }
        
        public void StartFalling()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.IsFalling.Value = true;
            }
        }

        public void StopFalling()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
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