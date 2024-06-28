using UnityEngine;
using UnityEngine.AI;
using Scripts.Combat;

namespace Scripts.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _pathUpdateInterval = 0.5f;
        [SerializeField] private float _maxConcussionTime = 3f;
        [SerializeField] private bool _initializeFromEditor = false;
        [SerializeField] private float _rotationSpeed = 5f;

        private NavMeshAgent _agent;
        private NavMeshPath _path;
        private Rigidbody _rb;
        private Health _health;

        private int _currentPathIndex = 0;
        private float _pathUpdateTime = 0f;
        private float _concussionTime = 0f;
        private bool _isDead = false;

        private void Awake() {
            if (_initializeFromEditor) {
                Initialize(_playerTransform);
            }
        }

        public void Initialize(Transform playerTransform) {
            _agent = GetComponentInChildren<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _health = GetComponent<Health>();

            _rb.freezeRotation = true;
            _path = new NavMeshPath();
            _health.OnDead.AddListener(OnDead);

            _playerTransform = playerTransform;
            CalculatePath();
        }

        void CalculatePath() {
            if (_playerTransform == null || _agent == null || _path == null) return;

            if (_agent.CalculatePath(_playerTransform.position, _path)) {
                _currentPathIndex = 0;
            }
            else {
                Debug.LogError("Failed to calculate path");
            }
        }

        private void Update() {
            if (_playerTransform == null || _agent == null || _path == null) return;

            if (_isDead) return;

            if (Time.time > _pathUpdateTime) {
                CalculatePath();
                _pathUpdateTime = Time.time + _pathUpdateInterval;
            }

            if (_concussionTime > 0) {
                _concussionTime -= Time.deltaTime;
                return;
            }

            if (_path.status == NavMeshPathStatus.PathComplete && _currentPathIndex < _path.corners.Length) {
                Vector3 targetPosition = _path.corners[_currentPathIndex];
                Vector3 direction = (targetPosition - transform.position).normalized;
                Vector3 directionForRotate = new Vector3(direction.x, 0, direction.z);

                _rb.AddForce(direction * _speed, ForceMode.Force);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionForRotate), _rotationSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f + _agent.height / 2) {
                    _currentPathIndex++;
                }
            }

            SpeedControl();
        }

        private void SpeedControl()
        {
            Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVelocity.magnitude > _speed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * _speed;
                _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            }
        }

        public void Concussion(float time) {
            _concussionTime += time;

            if (_concussionTime > _maxConcussionTime) _concussionTime = _maxConcussionTime;
        }

        private void OnDead() {
            _rb.freezeRotation = false;
            _agent.enabled = false;

            _isDead = true;

            _rb.drag = 1f;
            _rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
}
