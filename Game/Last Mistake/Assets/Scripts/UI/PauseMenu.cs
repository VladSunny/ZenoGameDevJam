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

        private void Awake() {
            _pauseAction = GetComponent<PlayerInput>().actions["PauseMenu"];

            _pauseMenuUI.SetActive(false);

            _pauseAction.performed += PressHandler;
            _resumeButton.onClick.AddListener(Resume);
            _quitButton.onClick.AddListener(() => Application.Quit());
        }

        private void OnEnable() {
            _pauseAction.Enable();
        }

        private void OnDisable() {
            _pauseAction.Disable();
        }

        private void PressHandler(InputAction.CallbackContext context)
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        private void Resume() {
            GameIsPaused = false;
            _pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }

        private void Pause() {
            GameIsPaused = true;
            _pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
