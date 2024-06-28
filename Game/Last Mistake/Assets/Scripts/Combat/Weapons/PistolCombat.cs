using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Combat;
using Scripts.Movement;
using TMPro;
using System.Collections;
using Scripts.Config;

namespace Scripts
{
    public class PistolCombat : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform _muzzle;

        [Header("Settings")]
        [SerializeField] private PistolConfig _config;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _bulletsText;

        [Header("Effects")]
        [SerializeField] private TrailRenderer _bulletTrail;
        [SerializeField] private ParticleSystem _bulletParticle;

        [HideInInspector] public PistolConfig _settings;

        private PlayerInput _playerInput;
        private InputAction _shootAction;
        private InputAction _reloadAction;
        private Animator _animator;
        private WaveController _waveController;

        private bool _canShoot = true;
        private bool _reloading = false;
        private int _curBullets = 0;
        private int _rememainingBullets = 0;

        private void Awake() {
            _playerInput = GetComponentInParent<PlayerInput>();
            _animator = GetComponent<Animator>();
            _waveController = GameObject.FindGameObjectWithTag("WaveController").GetComponent<WaveController>();

            _settings = Instantiate(_config);

            if (_playerInput != null) {
                _shootAction = _playerInput.actions["Shoot"];
                _reloadAction = _playerInput.actions["Reload"];

                _shootAction.performed += Shoot;
                _reloadAction.performed += (InputAction.CallbackContext context) => StartReload();
            }

            ResetPistol();
        }

        private void OnEnable() {
            if (_shootAction != null)
                _shootAction.Enable();
        }

        private void OnDisable() {
            if (_shootAction != null)
                _shootAction.Disable();
        }

        private bool CanShoot() {
            if (_curBullets <= 0) {
                StartReload();
                return false;
            }
            return !(
                !_canShoot ||
                _reloading ||
                _curBullets <= 0 ||
                _waveController.GetGameState() != WaveController.GameState.Wave ||
                _waveController.isPaused ||
                _waveController.IsGameOver()
            );
        }

        private void Shoot(InputAction.CallbackContext context) {
            if (!CanShoot()) return;

            _curBullets--;

            Vector3 direction = GetDirection();

            RaycastHit hit;
            if (Physics.Raycast(_muzzle.position, direction, out hit, 100f))
            {
                if (_bulletTrail != null) {
                    TrailRenderer trail = Instantiate(_bulletTrail, _muzzle.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                }

                Health enemyHealth = hit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(_settings.damage);
                }

                Rigidbody rigidbody = hit.collider.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce(transform.parent.forward * _settings.force, ForceMode.Impulse);
                }

                EnemyMovement enemyMovement = hit.collider.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.Concussion(_settings.concussion);
                }
            }

            _canShoot = false;
            UpdateUI();
            Invoke("ResetShoot", _settings.shootCooldown);
            _animator.SetTrigger("shoot");

            Debug.DrawRay(_muzzle.position, direction * 100f, Color.red, 2f);
        }

        private Vector3 GetDirection() {
            Vector3 direction = transform.parent.forward;

            direction += new Vector3(
                Random.Range(-_settings.spread, _settings.spread),
                0f,
                Random.Range(-_settings.spread, _settings.spread)
            );

            direction.Normalize();

            return direction;
        }
        
        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit) {
            float time = 0f;
            Vector3 startPosition = trail.transform.position;

            while (time < 1f) {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            trail.transform.position = hit.point;

            Instantiate(_bulletParticle, hit.point + hit.normal * 0.5f, Quaternion.LookRotation(hit.normal));

            Destroy(trail.gameObject, trail.time);
        }

        private void ResetShoot() => _canShoot = true;

        private void StartReload() {
            if (_rememainingBullets <= 0 || _curBullets == _settings.bulletsInClip || _reloading) return;

            _reloading = true;
            _animator.SetTrigger("reload");
        }

        public void Reload() {
            int needBullets = _settings.bulletsInClip - _curBullets;

            if (needBullets > _rememainingBullets) {
                _curBullets += _rememainingBullets;
                _rememainingBullets = 0;
            }
            else {
                _curBullets += needBullets;
                _rememainingBullets -= needBullets;
            }

            _reloading = false;
            UpdateUI();
        }

        public void ResetPistol() {
            if (_settings.bulletsInClip <= _settings.maxBullets) {
                _curBullets = _settings.bulletsInClip;
                _rememainingBullets = _settings.maxBullets - _settings.bulletsInClip;
            }
            else {
                _curBullets = _settings.maxBullets;
                _rememainingBullets = 0;
            }

            UpdateUI();
        }

        private void UpdateUI() {
            if (_bulletsText == null) return;
            _bulletsText.text = $"Pistol\n{_curBullets} / {_rememainingBullets}";
        }
    }
}
