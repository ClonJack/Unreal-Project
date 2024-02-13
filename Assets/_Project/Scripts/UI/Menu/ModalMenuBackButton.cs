using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using VContainer;

namespace UI.Menu
{
    public class ModalMenuBackButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private ModalContainer _modalContainer;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider)
        {
            _modalContainer = objectsProvider.MenuCanvasRefs.MainModalContainer;
        }

        private void Awake() 
            => _button.onClick.AddListener(BackToMenu);

        private void OnDestroy() 
            => _button.onClick.RemoveListener(BackToMenu);

        private void BackToMenu() 
            => _modalContainer.Pop(true).ToUniTask().Forget();
    }
}