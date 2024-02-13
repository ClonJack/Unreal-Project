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
    public class SettingsMenuOpenButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private PageContainer _pageContainer;
        private IObjectResolver _objectResolver;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider, IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
            _pageContainer = objectsProvider.MenuCanvasRefs.MainPageContainer;
            _button.onClick.AddListener(OpenSettings);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OpenSettings);
        }

        private void OpenSettings()
        {
            _pageContainer.Push(ScreenNavNames.SettingsMenuPage, true, onLoad: InjectInPage).ToUniTask().Forget();
        }

        private void InjectInPage((string pageId, Page page) obj) 
            => _objectResolver.InjectGameObject(obj.page.gameObject);
    }
}