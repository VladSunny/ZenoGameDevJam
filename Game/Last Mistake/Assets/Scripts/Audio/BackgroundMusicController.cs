using UnityEngine;

namespace Scripts.Audio
{
    public class BackgroundMusicController : MonoBehaviour
    {
        private AudioManager _audioManager;
        private WaveController _waveController;

        private void Start()
        {
            _audioManager = GetComponent<AudioManager>();
            _waveController = GameObject.FindWithTag("WaveController").GetComponent<WaveController>();

            _waveController.stateChanged.AddListener(OnStateChanged);
        }

        private void OnStateChanged()
        {
            Debug.Log(_waveController.GetGameState());

            if (_waveController.GetGameState() == WaveController.GameState.Wave)
            {
                _audioManager.Stop("RestMusic");
                _audioManager.Stop("AfterDeathMusic");
                _audioManager.Play("WaveMusic");
            }
            else if (_waveController.GetGameState() == WaveController.GameState.Resting)
            {
                _audioManager.Stop("WaveMusic");
                _audioManager.Stop("AfterDeathMusic");
                _audioManager.Play("RestMusic");
            }
            else if (_waveController.GetGameState() == WaveController.GameState.GameOver)
            {
                Debug.Log("Game Over");
                _audioManager.Stop("WaveMusic");
                _audioManager.Stop("RestMusic");
                _audioManager.Play("AfterDeathMusic");
            }
        }
    }
}
