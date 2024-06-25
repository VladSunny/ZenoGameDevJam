using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts
{
    public class Shop : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Button _openShopButton;
        [SerializeField] private RectTransform _shopUI;

        [Header("Effects")]
        [SerializeField] private float _shopAnimationInDuration = 1f;

        private WaveController _waveController;
        private GameObject _openShopButtonObject;
        private GameObject _shopUIObject;

        private Vector3 _finalShopUIPosition;

        private void Awake() {
            _waveController = GameObject.FindWithTag("WaveController").GetComponent<WaveController>();

            _openShopButtonObject = _openShopButton.gameObject;
            _shopUIObject = _shopUI.gameObject;

            _waveController.stateChanged.AddListener(StateChanged);
            _openShopButton.onClick.AddListener(ShopButtonHandler);

            _finalShopUIPosition = _shopUI.localPosition;

            _openShopButtonObject.SetActive(false);
            _shopUIObject.SetActive(false);
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
            buttonTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        }

        private void OpenShopButtonOut() {
            RectTransform buttonTransform = _openShopButton.GetComponent<RectTransform>();
            
            buttonTransform.DOScale(0f, 0.5f).OnComplete(
                () => _openShopButtonObject.SetActive(false)
            );
        }

        private void ShopButtonHandler() {
            if (_shopUIObject.activeSelf) {
                ShopUIOut();
            }
            else {
                ShopUIIn();
            }
        }

        private void ShopUIIn() {
            _shopUIObject.SetActive(true);

            _shopUI.localPosition = new Vector3(_finalShopUIPosition.x, 500f, _finalShopUIPosition.z);

            _shopUI.DOLocalMoveY(_finalShopUIPosition.y, _shopAnimationInDuration).SetEase(Ease.OutElastic);
        }

        private void ShopUIOut() {
            _shopUI.DOLocalMoveY(500f, 1f).OnComplete(() => _shopUIObject.SetActive(false));
        }

        private void StateChanged() {
            if (_waveController.GetGameState() == WaveController.GameState.Wave) {
                OpenShopButtonOut();
                ShopUIOut();
            }
        }
    }
}
