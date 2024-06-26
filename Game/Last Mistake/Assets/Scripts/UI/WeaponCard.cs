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
        private Selectable _cardSelectable;

        private void Awake() {
            _card = Instantiate(_cardPrefab, _cardParent);
            _cardHeader = _card.GetComponentInChildren<TextMeshProUGUI>();
            _cardSelectable = _card.GetComponent<Selectable>();

            AddEventTrigger(_cardSelectable.gameObject, EventTriggerType.Select, OnCardSelect);
            AddEventTrigger(_cardSelectable.gameObject, EventTriggerType.Deselect, OnCardDeselect);

            _cardHeader.text = gameObject.name;

            CreateUpgrades();
        }

        private void AddEventTrigger(GameObject target, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> callback)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = target.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = eventType
            };
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
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
            Debug.Log(_upgrades);
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
