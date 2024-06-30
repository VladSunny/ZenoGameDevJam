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
        
        private TabGroup _tabGroup;
        private Image _background;

        public void OnPointerClick(PointerEventData eventData)
        {
            _tabGroup.OnTabSelected(this);
            OnTabSelected?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tabGroup.OnTabExit(this);
        }

        public void ChangeColor(Color color) {
            _background.DOColor(color, 0.5f);
            // background.color = color
        }

        private void Awake() {
            _background = GetComponent<Image>();
            _tabGroup = GetComponentInParent<TabGroup>();

            _tabGroup.Subscribe(this);
        }
    }
}
