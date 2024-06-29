using System.Collections;
using Scripts.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class RifleCombat : GunBase
    {
        private PlayerInput _playerInput;
        private InputAction _shootAction;
        private InputAction _reloadAction;
        private Coroutine _fireCoroutine;
        private bool _readyForShoot = true;

        protected override void Awake() {
            base.Awake();

            _playerInput = GetComponentInParent<PlayerInput>();

            if (_playerInput != null) {
                _shootAction = _playerInput.actions["Shoot"];
                _reloadAction = _playerInput.actions["Reload"];

                _shootAction.started += OnActionStarted;
                _shootAction.canceled += OnActionCanceled;
                
                _reloadAction.performed += (InputAction.CallbackContext context) => StartReload();
            }
        }

        private void OnEnable() {
            if (_shootAction != null)
                _shootAction.Enable();
        }

        private void OnDisable() {
            if (_shootAction != null)
                _shootAction.Disable();
        }

        protected override bool CanShoot()
        {
            if (!base.CanShoot()) return false;

            return _readyForShoot;
        }

        private void OnActionStarted(InputAction.CallbackContext context) {
            if (_fireCoroutine == null) _fireCoroutine = StartCoroutine(Fire());
        }

        private void OnActionCanceled(InputAction.CallbackContext context) {
            if (_fireCoroutine != null) {
                StopCoroutine(_fireCoroutine);
                _fireCoroutine = null;
            }
        }

        private IEnumerator Fire() {
            while (true) {
                if (CanShoot()) {
                    Shoot();
                }
                yield return new WaitForSeconds(_settings.shootCooldown);
            }
        }
    }
}
