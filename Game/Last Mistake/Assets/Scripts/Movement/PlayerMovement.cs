using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        
        [Header("Rotation")]
        [SerializeField] private float _rotationSpeed = 5f;

        private Rigidbody _rb;
        private PlayerInput _playerInput;
        private InputAction _moveAction;

        private Vector2 _moveInput;
        private Vector3 _lastMoveDirection;
        private MovementState _state;
        
        public Vector3 LastMoveDirection => _lastMoveDirection;
        public enum MovementState {
            walking,
            dashing
        }
        
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();

            _moveAction = _playerInput.actions["Movement"];

            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;

            _lastMoveDirection = transform.forward;
            ChangeState(MovementState.walking);
        }

        private void OnEnable() {
            _moveAction.Enable();
        }

        private void OnDisable() {
            _moveAction.Disable();
        }

        private void Update() {
            SpeedControl();
            RotateTowardsMouse();
        }

        private void FixedUpdate() {
            if (_state != MovementState.walking) return;

            Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveSpeed;

            if (movement != Vector3.zero) {
                _lastMoveDirection = movement.normalized;
            }

            _rb.AddForce(movement, ForceMode.Force);
        }

        public void ChangeState(MovementState state) {
            _state = state;
        }

        private void OnMove(InputAction.CallbackContext context) {
            _moveInput = context.ReadValue<Vector2>();
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

        void RotateTowardsMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, 0);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 target = ray.GetPoint(distance);
                Vector3 direction = target - transform.position;
                direction.y = 0; 
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
