using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100f;
        
        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0) _currentHealth = 0;

            Debug.Log($"Health: {_currentHealth}");
        }
    }
}
