using System.Collections;
using System.Collections.Generic;
using NiceIO.Sysroot;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Movement;

namespace Scripts.Combat
{
    public class KnifeCombat : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _cooldown = 2f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _force = 5f;
        [SerializeField] private float _concussion = 2f;

        private PlayerInput _playerInput;
        private InputAction _slashAction;
        private Animator _animator;

        private HashSet<Collider> _damagedEnemies = new HashSet<Collider>();
        private bool _canSlash = true;

        private void Awake() {
            _playerInput = GetComponentInParent<PlayerInput>();
            _animator = GetComponent<Animator>();

            _slashAction = _playerInput.actions["KnifeSlash"];

            _slashAction.performed += Slash;
            _slashAction.canceled += Slash;
        }

        private void OnEnable() {
            if (_slashAction != null)
                _slashAction.Enable();
        }

        private void OnDisable() {
            if (_slashAction != null)
                _slashAction.Disable();
        }

        private void Slash(InputAction.CallbackContext context) {
            if (!_canSlash) return;
            
            _canSlash = false;
            _animator.SetTrigger("slash");
            
            Invoke("ResetSlash", _cooldown);
        }

        private void ResetSlash() {
            _damagedEnemies.Clear();
            _canSlash = true;
        }

        void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != null && !_damagedEnemies.Contains(other)) {
                Rigidbody rigidbody = other.GetComponent<Rigidbody>();

                if (rigidbody != null)
                    rigidbody.AddForce(transform.forward * _force, ForceMode.Impulse);

                EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();

                if (enemyMovement != null) enemyMovement.Concussion(_concussion);

                other.GetComponent<Health>().TakeDamage(_damage);
                _damagedEnemies.Add(other);
            }
        }
    }
}
