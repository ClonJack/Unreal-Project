using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;

namespace UI.Refs
{
    public class MainMenuCanvasRefs : MonoBehaviour
    {
        [field: SerializeField] public PageContainer MainPageContainer { get; private set; }
        [field: SerializeField] public ModalContainer MainModalContainer { get; private set; }
    }
}