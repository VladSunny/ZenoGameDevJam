using System.Collections;
using System.Collections.Generic;
using NiceIO.Sysroot;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Combat
{
    public class KnifeCombat : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _cooldown = 2f;

        private HitBoxesGenerator _hitBoxesGenerator;
        private PlayerInput _playerInput;
        private InputAction _slashAction;
        private Animator _animator;

        private bool _canSlash = true;

        private void Awake() {
            _hitBoxesGenerator = GetComponentInParent<HitBoxesGenerator>();
            _playerInput = GetComponentInParent<PlayerInput>();
            _animator = GetComponent<Animator>();

            _slashAction = _playerInput.actions["KnifeSlash"];

            _slashAction.performed += Slash;
            _slashAction.canceled += Slash;
        }

        private void OnEnable() {
            _slashAction.Enable();
        }

        private void OnDisable() {
            _slashAction.Disable();
        }

        private void Slash(InputAction.CallbackContext context) {
            if (!_canSlash) return;
            
            _canSlash = false;
            _animator.SetTrigger("slash");
            
            Invoke("ResetSlash", _cooldown);
        }

        private void ResetSlash() {
            _canSlash = true;
        }

        public void Attack() {
            Transform playerTransform = transform.parent;
            _hitBoxesGenerator.CreateHitBox(playerTransform.forward * 1f, new Vector3(1f, 1f, 1f), null, true);
        }
    }
}
