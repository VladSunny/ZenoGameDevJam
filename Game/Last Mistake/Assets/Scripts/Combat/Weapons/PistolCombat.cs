using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Combat;
using Scripts.Movement;
using TMPro;

namespace Scripts
{
    public class PistolCombat : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _bulletsInClip = 7;
        [SerializeField] private int _maxBullets = 35;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _force = 2f;
        [SerializeField] private float _concussion = 0.5f;
        [SerializeField] private float _shootCooldown = 0.5f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _bulletsText;

        private PlayerInput _playerInput;
        private InputAction _shootAction;
        private InputAction _reloadAction;
        private Animator _animator;

        private bool _canShoot = true;
        private int _curBullets = 0;
        private int _rememainingBullets = 0;

        private void Awake() {
            _playerInput = GetComponentInParent<PlayerInput>();
            _animator = GetComponent<Animator>();

            if (_playerInput != null) {
                _shootAction = _playerInput.actions["Shoot"];
                _reloadAction = _playerInput.actions["Reload"];

                _shootAction.performed += Shoot;
                _reloadAction.performed += Reload;
            }

            _curBullets = _bulletsInClip;
            _rememainingBullets = _maxBullets - _bulletsInClip;

            UpdateUI();
        }

        private void OnEnable() {
            if (_shootAction != null)
                _shootAction.Enable();
        }

        private void OnDisable() {
            if (_shootAction != null)
                _shootAction.Disable();
        }

        private void Shoot(InputAction.CallbackContext context) {
            if (!_canShoot || _curBullets <= 0) return;

            _curBullets--;

            RaycastHit hit;
            if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, 100f))
            {
                Health enemyHealth = hit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(_damage);
                }

                Rigidbody rigidbody = hit.collider.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce(transform.parent.forward * _force, ForceMode.Impulse);
                }

                EnemyMovement enemyMovement = hit.collider.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.Concussion(_concussion);
                }
            }

            _canShoot = false;
            UpdateUI();
            Invoke("ResetShoot", _shootCooldown);

            Debug.DrawRay(transform.parent.position, transform.parent.forward * 100f, Color.red, 2f);
        }

        private void ResetShoot() => _canShoot = true;

        private void Reload(InputAction.CallbackContext context) {
            int needBullets = _bulletsInClip - _curBullets;

            if (needBullets > _rememainingBullets) {
                _curBullets += _rememainingBullets;
                _rememainingBullets = 0;
            }
            else {
                _curBullets += needBullets;
                _rememainingBullets -= needBullets;
            }

            UpdateUI();
        }

        public void UpdateUI() {
            if (_bulletsText == null) return;
            _bulletsText.text = $"Pistol\n{_curBullets} / {_rememainingBullets}";
        }
    }
}
