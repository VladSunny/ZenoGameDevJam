using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;

        private Rigidbody _rb;
        private PlayerInput _playerInput;
        private InputAction _moveAction;

        private Vector2 _moveInput;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();

            _moveAction = _playerInput.actions["Movement"];

            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
        }

        private void OnEnable() {
            _moveAction.Enable();
        }

        private void OnDisable() {
            _moveAction.Disable();
        }

        private void Update() {
            SpeedControl();
        }

        private void FixedUpdate() {
            Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveSpeed;

            _rb.AddForce(movement, ForceMode.Force);
        }

        private void OnMove(InputAction.CallbackContext context) {
            _moveInput = context.ReadValue<Vector2>();

            Debug.Log(_moveInput);
        }

        private void SpeedControl()
        {
            Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVelocity.magnitude > _moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * _moveSpeed;
                _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            }
        }
    }
}
