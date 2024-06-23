using UnityEngine;
using Cysharp.Threading.Tasks;
using Scripts.Movement;

namespace Scripts
{
    public class Spawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform _playerTransform;

        [Header("Settings")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector2 _minMaxDelay = new Vector2(1f, 3f);
        [SerializeField] private float _spawnCount = 10f;

        private Transform[] _spawnPoints;

        private void Awake() {
            _spawnPoints = GetComponentsInChildren<Transform>();

            StartWave();
        }

        public async void StartWave() {
            for (int i = 0; i < _spawnCount; i++) {
                await UniTask.Delay((int)Random.Range(_minMaxDelay.x, _minMaxDelay.y) * 1000);

                int spawnPointIndex = Random.Range(0, _spawnPoints.Length);
                GameObject obj = Instantiate(_prefab, _spawnPoints[spawnPointIndex].position, Quaternion.identity);
                obj.GetComponent<EnemyMovement>().Initialize(_playerTransform);
            }
        }
    }
}
