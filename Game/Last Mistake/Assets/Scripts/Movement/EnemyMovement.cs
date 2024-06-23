using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _pathUpdateInterval = 0.5f;

        private NavMeshAgent _agent;
        private NavMeshPath _path;
        private Rigidbody _rb;

        private int _currentPathIndex = 0;
        private float _pathUpdateTime = 0f;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _path = new NavMeshPath();

            CalculatePath();
        }

        void CalculatePath() {
            if (_agent.CalculatePath(_playerTransform.position, _path)) {
                _currentPathIndex = 0;
            }
            else {
                Debug.LogError("Failed to calculate path");
            }
        }

        private void Update() {
            if (Time.time > _pathUpdateTime) {
                CalculatePath();
                _pathUpdateTime = Time.time + _pathUpdateInterval;
            }

            if (_path.status == NavMeshPathStatus.PathComplete && _currentPathIndex < _path.corners.Length) {
                Vector3 targetPosition = _path.corners[_currentPathIndex];
                Vector3 direction = (targetPosition - transform.position).normalized;

                _rb.AddForce(direction * _speed, ForceMode.Force);

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
    }
}
