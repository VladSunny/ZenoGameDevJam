using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Combat;
using Scripts.Movement;
using TMPro;
using System.Collections;
using Scripts.Config;

namespace Scripts
{
    public class PistolCombat : GunBase
    {
        private PlayerInput _playerInput;
        private InputAction _shootAction;
        private InputAction _reloadAction;

        private bool _readyForShoot = true;

        protected override void Awake() {
            base.Awake();

            _playerInput = GetComponentInParent<PlayerInput>();

            if (_playerInput != null) {
                _shootAction = _playerInput.actions["Shoot"];
                _reloadAction = _playerInput.actions["Reload"];

                _shootAction.performed += (InputAction.CallbackContext context) => Shoot();
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

        protected override void Shoot()
        {
            base.Shoot();

            Invoke("ResetShoot", _settings.shootCooldown);
        }

        private void ResetShoot() => _readyForShoot = true;
    }
}
