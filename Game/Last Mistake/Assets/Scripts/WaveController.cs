using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

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

        public UnityEvent stateChanged = new UnityEvent();

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
                ChangeState(GameState.Wave);
            }
        }

        private void EnemyDeathHandler() {
            _enemiesLeft--;

            if (_enemiesLeft <= 0) {
                ChangeState(GameState.Resting);
            }
            else UpdateWaveUI();
        }

        private void UpdateWaveUI() {
            _waveInfoText.text = $"Wave: {_waveCount}\nEnemies left: {_enemiesLeft}";
        }

        private void UpdateTimeoutUI() {
            _waveInfoText.text = $"Next wave: {_waveCount}\nTime for rest left: {(int)_restTimer}";
        }

        private void SkipResting() {
            _restTimer = 0f;
        }

        private void NextWaveButtonIn() {
            _nextWaveButtonGameObject.SetActive(true);

            RectTransform buttonTransform = _nextWaveButton.GetComponent<RectTransform>();
            
            buttonTransform.localScale = Vector3.zero;
            buttonTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        }

        private void NextWaveButtonOut() {
            RectTransform buttonTransform = _nextWaveButton.GetComponent<RectTransform>();
            
            buttonTransform.DOScale(0f, 0.5f).OnComplete(
                () => _nextWaveButtonGameObject.SetActive(false)
            );
        }

        private void ChangeState(GameState state) {
            _gameState = state;

            if (_gameState == GameState.Resting) {
                _restTimer = _restTime;
                _waveCount++;
                _rest = true;
                
                UpdateTimeoutUI();
                NextWaveButtonIn();
            }
            if (_gameState == GameState.Wave) {
                _rest = false;
                _spawner.StartWave(_currentWaveEnemyCount);
                _enemiesLeft = _currentWaveEnemyCount;
                _gameState = GameState.Wave;

                NextWaveButtonOut();
                UpdateWaveUI();
            }

            stateChanged.Invoke();
        }
    }
}
