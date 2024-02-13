using Common.Constants;
using Cysharp.Threading.Tasks;
using Services.Loading;
using Services.Other;
using UnityScreenNavigator.Runtime.Core.Page;
using VContainer;
using VContainer.Unity;

namespace GameFlow
{
    public class MainMenuEntryPoint
    {
        private readonly LoadingCurtain _loadingCurtain;
        private readonly PageContainer _pageContainer;
        private readonly IObjectResolver _objectResolver;


        public MainMenuEntryPoint(
            ObjectsProvider objectsProvider, 
            LoadingCurtain loadingCurtain, 
            IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
            _loadingCurtain = loadingCurtain;
            _pageContainer = objectsProvider.MenuCanvasRefs.MainPageContainer;
        }
        
        public async UniTask ExecuteAsync()
        {
#if UNITY_EDITOR
            // Требуется для корректной работы BootFromAnyScene, так как эдитор отрабатывает позже билда контейнера
            await UniTask.Yield();
#endif
            await LoadMenuPage();
            await _loadingCurtain.HideAsync();
        }

        private async UniTask LoadMenuPage() 
            => await _pageContainer.Push(ScreenNavNames.MainMenuPage, false, onLoad: InjectInPage).ToUniTask();

        private void InjectInPage((string pageId, Page page) obj) 
            => _objectResolver.InjectGameObject(obj.page.gameObject);
    }
}