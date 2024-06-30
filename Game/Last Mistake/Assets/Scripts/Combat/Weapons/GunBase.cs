using UnityEngine;
using Scripts.Config;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using Scripts.Movement;

namespace Scripts.Combat
{
    public class GunBase : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform _muzzle;
        [SerializeField] private LayerMask _layerMask;

        [Header("Settings")]

        [SerializeField] private PistolConfig _config;

        // [Header("UI")]

        [Header("Effects")]
        [SerializeField] private TrailRenderer _bulletTrail;
        [SerializeField] private ParticleSystem _bulletParticle;
        [SerializeField] private float _particleOffset = 0.3f;

        [HideInInspector] public PistolConfig _settings;

        private Animator _animator;
        private WaveController _waveController;
        private TextMeshProUGUI _bulletsText;

        private bool _reloading = false;
        private int _curBullets = 0;
        private int _rememainingBullets = 0;
        private float _nextTimeToFire = 0f;

        protected virtual void Awake() {
            _animator = GetComponent<Animator>();
            _waveController = GameObject.FindGameObjectWithTag("WaveController").GetComponent<WaveController>();
            _bulletsText = GameObject.FindGameObjectWithTag("WeaponInfo").GetComponent<TextMeshProUGUI>();

            _settings = Instantiate(_config);
            ResetWeapon();
        }

        private void OnDisable() {
            _reloading = false;
        }

        public bool IsReloading() => _reloading;

        protected virtual bool CanShoot() {
            if (_curBullets <= 0) {
                StartReload();
                return false;
            }
            return !(
                _reloading ||
                _curBullets <= 0 ||
                _waveController.GetGameState() != WaveController.GameState.Wave ||
                _waveController.isPaused ||
                _waveController.IsGameOver() ||
                Time.time < _nextTimeToFire ||
                gameObject.activeSelf == false
            );
        }

        protected virtual void Shoot() {
            if (!CanShoot()) return;

            _nextTimeToFire = Time.time + _settings.shootCooldown;
            _curBullets--;

            Vector3 direction = GetDirection();

            RaycastHit hit;
            if (Physics.Raycast(_muzzle.position, direction, out hit, 100f, _layerMask, QueryTriggerInteraction.Ignore))
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

            UpdateUI();
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

            Instantiate(_bulletParticle, hit.point + hit.normal * _particleOffset, Quaternion.LookRotation(hit.normal));

            Destroy(trail.gameObject, trail.time);
        }

        private bool CanReload() {
            return !(
                _rememainingBullets <= 0 ||
                _curBullets == _settings.bulletsInClip ||
                _reloading ||
                gameObject.activeSelf == false);
        }

        protected void StartReload() {
            if (!CanReload()) return;

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

        public void ResetWeapon() {
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

        public void UpdateUI() {
            if (_bulletsText == null || this.isActiveAndEnabled == false) return;
            _bulletsText.text = $"{gameObject.name}\n{_curBullets} / {_rememainingBullets}";
        }
    }
}
