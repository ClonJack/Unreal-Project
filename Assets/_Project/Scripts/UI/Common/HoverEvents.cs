using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnrealTeam.SB.UI.Common
{
    public class HoverEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action Entered;
        public event Action Exited;
        public event Action Selected;
        public event Action Deselected;
        

        public void OnPointerEnter(PointerEventData eventData)
            => Entered?.Invoke();

        public void OnPointerExit(PointerEventData eventData)
            => Exited?.Invoke();

        public void OnSelect(BaseEventData eventData)
            => Selected?.Invoke();

        public void OnDeselect(BaseEventData eventData)
            => Deselected?.Invoke();
    }
}