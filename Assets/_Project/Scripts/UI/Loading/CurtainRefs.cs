using TransitionsPlus;
using UnityEngine;

namespace UI.Loading
{
    public class CurtainRefs : MonoBehaviour
    {
        [field: SerializeField] public TransitionAnimator TransitionAnimator { get; private set; }
    }
}