using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Movement
{
    public class Dashing : MonoBehaviour
    {
        [Header("Dashing")]
        [SerializeField] private float dashForse = 20f;
        [SerializeField] private float dashDuration = 2f;
        [SerializeField] private float dashCooldown = 5f;

        private Rigidbody _rb;
        private PlayerInput _playerInput;
        private InputAction _dashAction;
        private PlayerMovement _playerMovement;

        private bool _canDash = true;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();
            _playerMovement = GetComponent<PlayerMovement>();

            _dashAction = _playerInput.actions["Dashing"];

            _dashAction.performed += Dash;
        }

        private void OnEnable() {
            _dashAction.Enable();
        }

        private void OnDisable() {
            _dashAction.Disable();
        }

        private void Dash(InputAction.CallbackContext context) {
            Debug.Log("Dash");

            if (!_canDash) return;

            _canDash = false;
            _playerMovement.ChangeState(PlayerMovement.MovementState.dashing);

            Vector3 forceToApply = _playerMovement.LastMoveDirection * dashForse;
            _rb.AddForce(forceToApply, ForceMode.Impulse);

            Invoke("ResetDash", dashDuration);
            Invoke("ResetDashCooldown", dashCooldown);
        }

        private void ResetDash() {
            _playerMovement.ChangeState(PlayerMovement.MovementState.walking);
        }

        private void ResetDashCooldown() {
            _canDash = true;
        }

    }
}
