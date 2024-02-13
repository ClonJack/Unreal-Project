using Common.Constants;
using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using VContainer;
using VContainer.Unity;

namespace UI.Menu
{
    public class AuthorsMenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private PageContainer _pageContainer;
        private IObjectResolver _objectResolver;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider, IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
            _pageContainer = objectsProvider.MenuCanvasRefs.MainPageContainer;
            _button.onClick.AddListener(OpenAuthors);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OpenAuthors);
        }

        private void OpenAuthors() 
            => _pageContainer.Push(ScreenNavNames.AuthorsMenuPage, true, onLoad: InjectInPage).ToUniTask().Forget();
        
        private void InjectInPage((string pageId, Page page) obj) 
            => _objectResolver.InjectGameObject(obj.page.gameObject);
    }
}