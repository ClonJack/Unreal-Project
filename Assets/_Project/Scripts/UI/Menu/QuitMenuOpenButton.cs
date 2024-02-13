using Common.Constants;
using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using VContainer;
using VContainer.Unity;

namespace UI.Menu
{
    public class QuitMenuOpenButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IObjectResolver _objectResolver;
        private ModalContainer _modalContainer;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider, IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
            _modalContainer = objectsProvider.MenuCanvasRefs.MainModalContainer;
            _button.onClick.AddListener(OpenAuthors);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OpenAuthors);
        }

        private void OpenAuthors() 
            => _modalContainer.Push(ScreenNavNames.QuitMenuModal, true, onLoad: InjectInModal).ToUniTask().Forget();
        
        private void InjectInModal((string modalId, Modal modal) obj) 
            => _objectResolver.InjectGameObject(obj.modal.gameObject);
    }
}