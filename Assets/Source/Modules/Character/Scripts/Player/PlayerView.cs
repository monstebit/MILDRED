using System;
using Unity.Netcode;
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
        //  IsStopping
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

        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerView: No player composition root");
            }
        }

        public void Initialize() => Animator = GetComponent<Animator>();
        
        public void StartGrounded() => Animator.SetBool(IsGrounded, true);
        public void StopGrounded() => Animator.SetBool(IsGrounded, false);
        
        public void StartMoving() => Animator.SetBool(IsMoving, true);
        public void StopMoving() => Animator.SetBool(IsMoving, false);
        
        public void StartStaticAction() => Animator.SetBool(IsStaticAction, true);
        public void StopStaticAction() => Animator.SetBool(IsStaticAction, false);
        
        public void StartLanding() => Animator.SetBool(IsLanding, true);
        public void StopLanding() => Animator.SetBool(IsLanding, false);
        
        public void StartAirborne() => Animator.SetBool(IsAirborne, true);
        public void StopAirborne() => Animator.SetBool(IsAirborne, false);

        public void StartIdling()
        {
            Animator.SetBool(IsIdling, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "unarmed_main_idle_01");
        }
        public void StopIdling() => Animator.SetBool(IsIdling, false);

        public void StartWalking()
        {
            Animator.SetBool(IsWalking, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
                // NetworkManager.Singleton.LocalClientId, "core_oh_walk_F_01");
        }
        public void StopWalking() => Animator.SetBool(IsWalking, false);

        public void StartRunning()
        {
            Animator.SetBool(IsRunning, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_oh_run_F_01");
        }
        public void StopRunning() => Animator.SetBool(IsRunning, false);

        public void StartSprinting()
        {
            Animator.SetBool(IsSprinting, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_oh_sprint_F_01");
        }
        public void StopSprinting() => Animator.SetBool(IsSprinting, false);

        public void StartDodging()
        {
            Animator.SetBool(IsDodging, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_main_roll_to_idle_F_01");
        }
        public void StopDodging() => Animator.SetBool(IsDodging, false);

        public void StartBackStepping()
        {
            Animator.SetBool(IsBackStepping, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_main_back_step_medium_01");
        }
        public void StopBackStepping() => Animator.SetBool(IsBackStepping, false);

        public void StartLightLanding()
        {
            Animator.SetBool(IsLightLanding, true);
            
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_main_jump_01_end");
        }
        public void StopLightLanding() => Animator.SetBool(IsLightLanding, false);
        
        public void StartJumping()
        {
            Animator.SetBool(IsJumping, true);

            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_main_jump_slow_01_start");
        }
        
        public void StopJumping() => Animator.SetBool(IsJumping, false);
        
        public void StartFalling()
        {
            Animator.SetBool(IsFalling, true);

            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "core_main_jump_01_lift");
        }
        public void StopFalling() => Animator.SetBool(IsFalling, false);
    }
}