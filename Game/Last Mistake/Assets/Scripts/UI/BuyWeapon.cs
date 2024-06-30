using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class BuyWeapon : MonoBehaviour
    {
        [SerializeField] private GameObject _weapon;
        [SerializeField] private Transform _position;
        [SerializeField] private int _price;

        private Button _button;
        private Wallet _wallet;
        private TextMeshProUGUI _priceText;
        private GameObject _player;

        private void Awake() {
            _player = GameObject.FindWithTag("Player");

            _button = GetComponent<Button>();
            _wallet = _player.GetComponent<Wallet>();
            _priceText = GetComponentInChildren<TextMeshProUGUI>();

            _button.onClick.AddListener(Buy);

            _priceText.text = $"{_weapon.name}\n${_price}";
        }

        public void Buy() {
            if (_wallet.Balance() >= _price) {
                _wallet.AddCoins(-_price);

                GameObject weapon = Instantiate(
                    _weapon,
                    _position.position,
                    _player.transform.rotation,
                    GameObject.FindGameObjectWithTag("WeaponParent").transform
                );
                weapon.name = _weapon.name;
                weapon.SetActive(false);

                Destroy(gameObject);
            }
        }
    }
}
