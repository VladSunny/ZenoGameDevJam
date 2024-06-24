using UnityEngine;
using Cysharp.Threading.Tasks;
using Scripts.Movement;
using UnityEngine.Events;
using Scripts.Combat;

namespace Scripts
{
    public class Spawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform _playerTransform;

        [Header("Settings")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector2 _minMaxDelay = new Vector2(1f, 3f);

        public UnityEvent onEnemyDied = new UnityEvent();

        private Transform[] _spawnPoints;

        private void Awake() {
            _spawnPoints = GetComponentsInChildren<Transform>();
        }

        public async void StartWave(int spawnCount) {
            for (int i = 0; i < spawnCount; i++) {
                await UniTask.Delay((int)Random.Range(_minMaxDelay.x, _minMaxDelay.y) * 1000);

                int spawnPointIndex = Random.Range(1, _spawnPoints.Length);
                GameObject obj = Instantiate(_prefab, _spawnPoints[spawnPointIndex].position, Quaternion.identity);
                obj.GetComponent<EnemyMovement>().Initialize(_playerTransform);
                obj.GetComponent<Health>().OnDead.AddListener(EnemyDeathHandler);
            }
        }

        private void EnemyDeathHandler() {
            onEnemyDied.Invoke();
        }
    }
}
