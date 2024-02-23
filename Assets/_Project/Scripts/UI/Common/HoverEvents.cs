using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Common
{
    public class HoverEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> Entered;
        public event Action<PointerEventData> Exited;


        public void OnPointerEnter(PointerEventData eventData)
            => Entered?.Invoke(eventData);

        public void OnPointerExit(PointerEventData eventData)
            => Exited?.Invoke(eventData);
    }
}