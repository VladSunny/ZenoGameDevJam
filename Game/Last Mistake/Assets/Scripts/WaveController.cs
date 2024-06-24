using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts
{
    public class WaveController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI _waveInfoText;
        [SerializeField] private Button _nextWaveButton;

        [Header("Settings")]
        [SerializeField] private int _currentWaveEnemyCount = 10;
        [SerializeField] private float _restTime = 60f;
        
        public enum GameState {
            Wave,
            Resting
        }

        private Spawner _spawner;
        private GameObject _nextWaveButtonGameObject;
        private GameState _gameState = GameState.Wave;

        private int _enemiesLeft;
        private int _waveCount = 1;
        private float _restTimer = 0f;
        private bool _rest = false;

        public GameState GetGameState() => _gameState;

        private void Awake() {
            _spawner = GetComponent<Spawner>();

            _spawner.onEnemyDied.AddListener(EnemyDeathHandler);

            _spawner.StartWave(_currentWaveEnemyCount);
            _enemiesLeft = _currentWaveEnemyCount;

            _nextWaveButton.onClick.AddListener(SkipResting);
            _nextWaveButtonGameObject = _nextWaveButton.gameObject;
            _nextWaveButtonGameObject.SetActive(false);

            UpdateWaveUI();
        }

        private void Update() {
            if (_restTimer > 0) {
                _restTimer -= Time.deltaTime;
                UpdateTimeoutUI();
            }
            else if (_rest) {
                _rest = false;
                _spawner.StartWave(_currentWaveEnemyCount);
                _enemiesLeft = _currentWaveEnemyCount;
                _gameState = GameState.Wave;

                UpdateWaveUI();
            }
        }

        private void EnemyDeathHandler() {
            _enemiesLeft--;

            if (_enemiesLeft <= 0) {
                _restTimer = _restTime;
                _waveCount++;
                _rest = true;

                _gameState = GameState.Resting;
                UpdateTimeoutUI();
                NextWaveButtonAnimationIn();
            }
            else UpdateWaveUI();
        }

        private void UpdateWaveUI() {
            _nextWaveButtonGameObject.SetActive(false);
            _waveInfoText.text = $"Wave: {_waveCount}\nEnemies left: {_enemiesLeft}";
        }

        private void UpdateTimeoutUI() {
            _nextWaveButtonGameObject.SetActive(true);
            _waveInfoText.text = $"Next wave: {_waveCount}\nTime for rest left: {(int)_restTimer}";
        }

        private void SkipResting() {
            _restTimer = 0f;
        }

        private void NextWaveButtonAnimationIn() {
            RectTransform buttonTransform = _nextWaveButton.GetComponent<RectTransform>();
            
            buttonTransform.localScale = Vector3.zero;
            buttonTransform.DOScale(1f, 0.5f);
        }
    }
}
