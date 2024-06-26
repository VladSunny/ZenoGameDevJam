using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using Scripts.Audio;

namespace Scripts.Combat
{
    public class Health : MonoBehaviour
    {
        [Header("Main Settings")]
        [SerializeField] private float _maxHealth = 100f;

        [Header("Effects")]
        [SerializeField] private float _hideDuration = 0.5f;
        [SerializeField] private Color _corpseColor;
        [SerializeField] private float _colorChangeDuration = 0.5f;

        public UnityEvent OnDead;

        private AudioManager _audioManager;
        private float _currentHealth;
        private bool _isDead = false;

        private void Awake()
        {
            _audioManager = GetComponent<AudioManager>();
            _currentHealth = _maxHealth;
            OnDead.AddListener(Dead);
        }

        public void TakeDamage(float damage)
        {
            _audioManager.Play("Hit");

            if (_isDead) return;

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                OnDead.Invoke();
                _isDead = true;
            }
        }

        private async void Dead()
        {
            GetComponent<Renderer>().material.DOColor(_corpseColor, _colorChangeDuration);

            await UniTask.Delay(2000);

            await transform.DOScale(0, _hideDuration).AsyncWaitForCompletion();

            Destroy(gameObject, 5f);
        }

        public bool IsDead() => _isDead;
    }
}
