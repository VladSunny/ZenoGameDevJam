using Scripts.Combat;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class RifleCombat : GunBase
    {
        private PlayerInput _playerInput;
        private InputAction _shootAction;
        private InputAction _reloadAction;
        private bool _fire = false;

        protected override void Awake() {
            base.Awake();

            _playerInput = GetComponentInParent<PlayerInput>();

            if (_playerInput != null) {
                _shootAction = _playerInput.actions["Shoot"];
                _reloadAction = _playerInput.actions["Reload"];
            }
        }

        private void OnEnable() {
            if (_shootAction != null && _reloadAction != null) {
                _shootAction.started += OnActionStarted;
                _shootAction.canceled += OnActionCanceled;
                _reloadAction.performed += ctx => StartReload();
            }
        }

        private void OnDisable() {
            if (_shootAction != null && _reloadAction != null) {
                _shootAction.started -= OnActionStarted;
                _shootAction.canceled -= OnActionCanceled;
                _reloadAction.performed -= ctx => StartReload();
            }
        }

        private void Update() {
            if (_fire) Shoot();
        }

        private void OnActionStarted(InputAction.CallbackContext context) {
            _fire = true;
        }

        private void OnActionCanceled(InputAction.CallbackContext context) {
            _fire = false;
        }
    }
}
