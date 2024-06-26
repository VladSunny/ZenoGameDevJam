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

        private void Awake() {
            _card = Instantiate(_cardPrefab, _cardParent);
            _cardHeader = _card.GetComponentInChildren<TextMeshProUGUI>();
            _cardTabButton = _card.GetComponent<TabButton>();

            _cardTabButton.OnTabSelected.AddListener(ShowUpgrades);
            _cardTabButton.OnTabDeselected.AddListener(HideUpgrades);

            _cardHeader.text = gameObject.name;

            CreateUpgrades();
        }

        private void CreateUpgrades() {
            foreach (Upgrade upgrade in _upgrades) {
                upgrade.Initialize(_upgradePrefab, _upgradeParent, () => UpgradeHandler(upgrade.GetUpgradeType()));
            }

            HideUpgrades();
        }

        private void UpgradeHandler(UpgradeType upgradeType) {
            Debug.Log(upgradeType);
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
