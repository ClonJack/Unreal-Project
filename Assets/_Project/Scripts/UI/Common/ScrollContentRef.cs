using UnityEngine;

namespace UI.Common
{
    public class ScrollContentRef : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Content { get; private set; }
    }
}