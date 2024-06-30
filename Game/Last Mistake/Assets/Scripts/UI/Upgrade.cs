using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace Scripts.UI
{
    public enum UpgradeType
    {
        MaxBullets,
        FireRate,
        Spread,
        Damage,
        FullReload,
        MaxBulletsInClip
    }

    [System.Serializable]
    public class Upgrade
    {
        [SerializeField] private string _name;
        [SerializeField] private UpgradeType _type;
        [SerializeField] private int _cost;
        [SerializeField] private int _costIncrement;
        [SerializeField] bool _hasMaxLevel = true;
        [SerializeField] private int _maxLevel = 5;

        private GameObject UpgradeGameObject;
        private Button UpgradeButton;
        private TextMeshProUGUI UpgradeText;
        private TextMeshProUGUI CostText;
        private int Level = 1;

        public void Initialize(GameObject prefab, Transform parent, UnityAction callback)
        {
            UpgradeGameObject = CreateUpgrade(prefab, parent);

            UpgradeButton = UpgradeGameObject.GetComponentInChildren<Button>();
            UpgradeText = UpgradeGameObject.GetComponentInChildren<TextMeshProUGUI>();
            CostText = UpgradeButton.GetComponentInChildren<TextMeshProUGUI>();

            SetButtonListener(callback);

            UpdateUI();
        }

        public UpgradeType GetUpgradeType() => _type;

        public void SetButtonListener(UnityAction callback) => UpgradeButton.onClick.AddListener(callback);

        public void Hide() => UpgradeGameObject.SetActive(false);

        public void Show() {
            UpgradeGameObject.SetActive(true);
            UpgradeGameObject.transform.localScale = Vector3.zero;
            UpgradeGameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        }

        public void UpdateUI() {
            UpgradeText.text = $"{_name}\nLevel: {Level}";
            CostText.text = $"${_cost}";
        }

        public bool LevelUp() {
            if (!CanUpgrade()) return false;

            Level++;
            _cost += _costIncrement;
            UpdateUI();
            return true;
        }

        public bool CanUpgrade() => !(_hasMaxLevel && Level >= _maxLevel);

        public int Price() => _cost;

        private GameObject CreateUpgrade(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }
    }
}
