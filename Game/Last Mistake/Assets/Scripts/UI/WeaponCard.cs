using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Scripts.Combat;
using Scripts.Types;

namespace Scripts.UI
{   
    public class WeaponCard : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private string _name;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameObject _upgradePrefab;
        [SerializeField] private Upgrade[] _upgrades;

        private Wallet _wallet;
        private GameObject _card;
        private TextMeshProUGUI _cardHeader;
        private TabButton _cardTabButton;
        private GunBase _weapon;
        private Transform _cardParent;
        private Transform _upgradeParent;

        private GameObject _cardParentObject;
        private GameObject _upgradeParentObject;

        private void Awake() {
            _weapon = GetComponent<GunBase>();
            _wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<Wallet>();
            _cardParentObject = FindObjectByComponent<CardParent>();
            _upgradeParentObject = FindObjectByComponent<UpgradesParent>();

            _cardParent = _cardParentObject.transform;
            _upgradeParent = _upgradeParentObject.transform;

            _card = Instantiate(_cardPrefab, _cardParent);
            _cardHeader = _card.GetComponentInChildren<TextMeshProUGUI>();
            _cardTabButton = _card.GetComponent<TabButton>();

            _cardTabButton.OnTabSelected.AddListener(ShowUpgrades);
            _cardTabButton.OnTabDeselected.AddListener(HideUpgrades);

            _cardHeader.text = _name;

            CreateUpgrades();
        }

        private GameObject FindObjectByComponent<T>() where T : Component {
            T component = Resources.FindObjectsOfTypeAll<T>()[0];
            if (component != null) {
                return component.gameObject;
            }
            return null;
        }

        private void CreateUpgrades() {
            foreach (Upgrade upgrade in _upgrades) {
                upgrade.Initialize(_upgradePrefab, _upgradeParent, () => UpgradeHandler(upgrade));
            }

            HideUpgrades();
        }

        private void UpgradeHandler(Upgrade upgrade) {
            if (_wallet.Balance() < upgrade.Price() || !upgrade.CanUpgrade()) return;

            _wallet.AddCoins(-upgrade.Price());

            upgrade.LevelUp();

            UpgradeType upgradeType = upgrade.GetUpgradeType();
            Debug.Log(upgradeType);

            if (upgradeType == UpgradeType.FullReload) {
                _weapon.ResetWeapon();
            }
            if (upgradeType == UpgradeType.MaxBullets) {
                _weapon._settings.maxBullets += 5;
            }
            if (upgradeType == UpgradeType.FireRate) {
                _weapon._settings.shootCooldown /= 1.3f;
            }
            if (upgradeType == UpgradeType.Spread) {
                _weapon._settings.spread /= 1.3f;
            }
            if (upgradeType == UpgradeType.Damage) {
                _weapon._settings.damage += 5;
            }
            if (upgradeType == UpgradeType.MaxBulletsInClip) {
                _weapon._settings.bulletsInClip += 5;
            }
        }

        private void HideUpgrades() {
            foreach (Upgrade upgrade in _upgrades) {
                upgrade.Hide();
            }
        }

        private void ShowUpgrades() {
            foreach (Upgrade upgrade in _upgrades) {
                upgrade.Show();
            }
        }

        private void OnCardSelect(BaseEventData eventData) {
            ShowUpgrades();
        }

        private void OnCardDeselect(BaseEventData eventData) {
            HideUpgrades();
        }
    }
}
