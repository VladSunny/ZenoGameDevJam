using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverMenuUI;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private WaveController _waveController;

        private void Awake() {
            _waveController = GameObject.FindWithTag("WaveController").GetComponent<WaveController>();

            _gameOverMenuUI.SetActive(false);

            _waveController.onGameOver.AddListener(GameOver);
            _restartButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(() => Application.Quit());
        }

        public void GameOver() {
            _gameOverMenuUI.SetActive(true);
            _waveController.ChangeState(WaveController.GameState.GameOver);
        }

        private void Restart() {
            Debug.Log("Restart");
        }
    }
}
