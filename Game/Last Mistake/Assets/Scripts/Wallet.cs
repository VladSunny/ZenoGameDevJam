using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private int coins;
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI coinsText;

        private void Awake() {
            UpdateUI();
        }

        public void AddCoins(int amount) {
            coins += amount;
            UpdateUI();
        }

        public int Balance() => coins;

        public void UpdateUI() {
            coinsText.text = $"${coins}";
        }
    }
}
