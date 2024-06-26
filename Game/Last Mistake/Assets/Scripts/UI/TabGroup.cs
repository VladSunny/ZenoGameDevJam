using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private Color tabIdle;
        [SerializeField] private Color tabHover;
        [SerializeField] private Color tabActive;

        private List<TabButton> tabButtons;
        private TabButton selectedTab;

        public void Subscribe(TabButton button) {
            if (tabButtons == null) tabButtons = new List<TabButton>();
            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button) {
            ResetTabs();
            if (selectedTab == null || button != selectedTab) {
                button.ChangeColor(tabHover);
            }
        }

        public void OnTabExit(TabButton button) {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button) {
            selectedTab = button;
            ResetTabs();
            button.ChangeColor(tabActive);
        }

        public void ResetTabs() {
            foreach (TabButton button in tabButtons) {
                if (selectedTab != null && button == selectedTab) continue;
                
                button.ChangeColor(tabIdle);
                button.OnTabDeselected.Invoke();
            }
        }
    }
}
