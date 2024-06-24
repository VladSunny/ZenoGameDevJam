using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts
{
    public class Shop : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Button _openShopButton;

        private WaveController _waveController;
        private GameObject _openShopButtonObject;

        private void Awake() {
            _waveController = GameObject.FindWithTag("WaveController").GetComponent<WaveController>();
            _openShopButtonObject = _openShopButton.gameObject;

            _waveController.stateChanged.AddListener(StateChanged);

            _openShopButtonObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;

            if (_waveController.GetGameState() == WaveController.GameState.Resting) {
                OpenShopButtonIn();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;

            if (_waveController.GetGameState() == WaveController.GameState.Resting) {
                OpenShopButtonOut();
            }
        }

        private void OpenShopButtonIn() {
            _openShopButtonObject.SetActive(true);

            RectTransform buttonTransform = _openShopButton.GetComponent<RectTransform>();
            
            buttonTransform.localScale = Vector3.zero;
            buttonTransform.DOScale(1f, 0.5f);
        }

        private async void OpenShopButtonOut() {
            RectTransform buttonTransform = _openShopButton.GetComponent<RectTransform>();
            
            await buttonTransform.DOScale(0f, 0.5f).AsyncWaitForCompletion();

            _openShopButtonObject.SetActive(false);
        }

        private void StateChanged() {
            if (_waveController.GetGameState() == WaveController.GameState.Wave) {
                OpenShopButtonOut();
            }
        }
    }
}
