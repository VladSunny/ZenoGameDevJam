using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Scripts.UI
{
    public enum UpgradeType
    {
        MaxBullets,
        FireRate,
        Spread,
        Damage
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

            UpgradeText.text = $"{Name}\nLevel: {Level}";
            CostText.text = $"${Cost}";
        }

        public UpgradeType GetUpgradeType() => Type;

        public void SetButtonListener(UnityAction callback) => UpgradeButton.onClick.AddListener(callback);

        public void Hide() => UpgradeGameObject.SetActive(false);

        public void Show() => UpgradeGameObject.SetActive(true);
    }

    public class CreateUpgrades : MonoBehaviour
    {
        public GameObject CreateUpgrade(GameObject prefab, Transform parent) => Instantiate(prefab, parent);
    }
}
