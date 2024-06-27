using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts
{
    public class Wallet : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI coinsText;

        private int coins = 0;

        private void Awake() {
            UpdateUI();
        }

        public void AddCoins(int amount) {
            coins += amount;
            UpdateUI();
        }

        public void UpdateUI() {
            coinsText.text = $"${coins}";
        }
    }
}