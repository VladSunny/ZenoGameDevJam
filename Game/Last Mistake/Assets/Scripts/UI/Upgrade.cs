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
        [SerializeField] private string Name;
        [SerializeField] private UpgradeType Type;
        [SerializeField] private int Cost;

        private GameObject UpgradeGameObject;
        private Button UpgradeButton;
        private TextMeshProUGUI UpgradeText;
        private TextMeshProUGUI CostText;
        private int Level = 1;

        public void Initialize(GameObject prefab, Transform parent, UnityAction callback)
        {
            CreateUpgrades createUpgrades = new CreateUpgrades();

            UpgradeGameObject = createUpgrades.CreateUpgrade(prefab, parent);

            UpgradeButton = UpgradeGameObject.GetComponentInChildren<Button>();
            UpgradeText = UpgradeGameObject.GetComponentInChildren<TextMeshProUGUI>();
            CostText = UpgradeButton.GetComponentInChildren<TextMeshProUGUI>();

            SetButtonListener(callback);

            UpdateUI();
        }

        public UpgradeType GetUpgradeType() => Type;

        public void SetButtonListener(UnityAction callback) => UpgradeButton.onClick.AddListener(callback);

        public void Hide() => UpgradeGameObject.SetActive(false);

        public void Show() {
            UpgradeGameObject.SetActive(true);
            UpgradeGameObject.transform.localScale = Vector3.zero;
            UpgradeGameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        }

        public void UpdateUI() {
            UpgradeText.text = $"{Name}\nLevel: {Level}";
            CostText.text = $"${Cost}";
        }

        public void LevelUp() {
            Level++;
            UpdateUI();
        }
    }

    public class CreateUpgrades : MonoBehaviour
    {
        public GameObject CreateUpgrade(GameObject prefab, Transform parent) => Instantiate(prefab, parent);
    }
}
