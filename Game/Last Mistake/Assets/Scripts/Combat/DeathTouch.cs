using System.Collections;
using System.Collections.Generic;
using Scripts.Combat;
using UnityEngine;

namespace Scripts
{
    public class DeathTouch : MonoBehaviour
    {
        private Health _health;
        private WaveController _waveController;

        private void Awake() {
            _health = GetComponent<Health>();
            _waveController = GameObject.FindWithTag("WaveController").GetComponent<WaveController>();
        }

        private void OnCollisionEnter(Collision other) {
            if (_health.IsDead()) return;

            if (other.gameObject.tag == "Player") {
                _waveController.onGameOver.Invoke();
            }
        }
    }
}
