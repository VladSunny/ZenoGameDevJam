using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Scripts.UI
{   
    public class WeaponCard : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameObject _upgradePrefab;
        [SerializeField] private Transform _cardParent;
        [SerializeField] private Transform _upgradeParent;
        [SerializeField] private Upgrade[] _upgrades;


        private GameObject _card;
        private TextMeshProUGUI _cardHeader;
        private TabButton _cardTabButton;
        private PistolCombat _pistolCombat;

        private void Awake() {
            _card = Instantiate(_cardPrefab, _cardParent);
            _cardHeader = _card.GetComponentInChildren<TextMeshProUGUI>();
            _cardTabButton = _card.GetComponent<TabButton>();
            _pistolCombat = GetComponent<PistolCombat>();

            _cardTabButton.OnTabSelected.AddListener(ShowUpgrades);
            _cardTabButton.OnTabDeselected.AddListener(HideUpgrades);

            _cardHeader.text = gameObject.name;

            CreateUpgrades();
        }

        private void CreateUpgrades() {
            foreach (Upgrade upgrade in _upgrades) {
                upgrade.Initialize(_upgradePrefab, _upgradeParent, () => UpgradeHandler(upgrade));
            }

            HideUpgrades();
        }

        private void UpgradeHandler(Upgrade upgrade) {
            UpgradeType upgradeType = upgrade.GetUpgradeType();
            Debug.Log(upgradeType);

            if (upgradeType == UpgradeType.FullReload) {
                _pistolCombat.ResetPistol();
            }
            if (upgradeType == UpgradeType.MaxBullets) {
                _pistolCombat._settings.maxBullets += 5;
            }
            if (upgradeType == UpgradeType.FireRate) {
                _pistolCombat._settings.shootCooldown /= 1.3f;
            }
            if (upgradeType == UpgradeType.Spread) {
                _pistolCombat._settings.spread /= 1.3f;
            }
            if (upgradeType == UpgradeType.Damage) {
                _pistolCombat._settings.damage += 5;
            }
            if (upgradeType == UpgradeType.MaxBulletsInClip) {
                _pistolCombat._settings.bulletsInClip += 5;
            }

            upgrade.LevelUp();
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
