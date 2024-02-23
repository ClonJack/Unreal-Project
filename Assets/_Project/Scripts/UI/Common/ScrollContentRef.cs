using UnityEngine;

namespace UnrealTeam.SB.UI.Common
{
    public class ScrollContentRef : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Content { get; private set; }
    }
}