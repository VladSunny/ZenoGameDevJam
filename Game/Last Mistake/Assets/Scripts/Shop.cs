using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class Shop : MonoBehaviour
    {
        private WaveController _waveController;

        private void Awake() {
            _waveController = GameObject.FindWithTag("WaveController").GetComponent<WaveController>();
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;

            if (_waveController.GetGameState() == WaveController.GameState.Resting) {
                Debug.Log("Shop opened");
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;

            if (_waveController.GetGameState() == WaveController.GameState.Resting) {
                Debug.Log("Shop closed");
            }
        }
    }
}
