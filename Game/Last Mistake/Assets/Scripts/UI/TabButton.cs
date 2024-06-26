using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public UnityEvent OnTabSelected;
        public UnityEvent OnTabDeselected;
        
        private TabGroup tabGroup;
        private Image background;

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
            OnTabSelected?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        public void ChangeColor(Color color) {
            background.DOColor(color, 0.5f);
            // background.color = color
        }

        private void Awake() {
            background = GetComponent<Image>();
            tabGroup = GetComponentInParent<TabGroup>();

            tabGroup.Subscribe(this);
        }
    }
}
