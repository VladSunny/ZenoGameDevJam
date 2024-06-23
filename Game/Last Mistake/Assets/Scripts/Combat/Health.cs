using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100f;
        public UnityEvent OnDead;
        
        private float _currentHealth;
        private bool _isDead = false;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            _currentHealth -= damage;

            if (_currentHealth <= 0) {
                OnDead.Invoke();
                _isDead = true;
            }

            Debug.Log($"Health: {_currentHealth}");
        }
    }
}
