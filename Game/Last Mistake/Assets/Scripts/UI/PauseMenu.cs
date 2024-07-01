using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenuUI;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;

        public static bool GameIsPaused = false;

        private InputAction _pauseAction;
        private Image _pauseMenuImage;
        private Color _normalMenuColor;
        private WaveController _waveController;

        private bool _loading = false;

        private void Awake()
        {
            _pauseAction = GetComponent<PlayerInput>().actions["PauseMenu"];
            _pauseMenuImage = _pauseMenuUI.GetComponent<Image>();
            _waveController = GameObject.FindGameObjectWithTag("WaveController").GetComponent<WaveController>();

            _normalMenuColor = _pauseMenuUI.GetComponent<Image>().color;

            // _pauseMenuUI.SetActive(false);

            _pauseAction.performed += PressHandler;

            _resumeButton.onClick.AddListener(Resume);
            _quitButton.onClick.AddListener(() => Application.Quit());

            Pause();
        }

        private void OnEnable()
        {
            _pauseAction.Enable();
        }

        private void OnDisable()
        {
            _pauseAction.Disable();
        }

        private void PressHandler(InputAction.CallbackContext context)
        {
            if (_loading) return;

            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        private void Resume()
        {
            GameIsPaused = false;
            _waveController.isPaused = false;
            _loading = true;

            _pauseMenuImage.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(() =>
            {
                _pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                _loading = false;
            });
        }

        private void Pause()
        {
            if (_waveController.IsGameOver()) return;

            GameIsPaused = true;
            _waveController.isPaused = true;

            _pauseMenuUI.SetActive(true);

            _pauseMenuImage.color = new Color(_normalMenuColor.r, _normalMenuColor.g, _normalMenuColor.b, 0f);

            _loading = true;

            _pauseMenuImage.DOFade(_normalMenuColor.a, 0.5f).SetUpdate(true).OnComplete(() => _loading = false);

            Time.timeScale = 0f;

            Debug.Log("Pause");
        }
    }
}
