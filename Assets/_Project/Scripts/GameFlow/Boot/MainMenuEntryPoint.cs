using Common.Enums;
using Cysharp.Threading.Tasks;
using Services.Loading;
using Services.Other;

namespace GameFlow
{
    public class MainMenuEntryPoint
    {
        private readonly LoadingCurtain _loadingCurtain;
        private readonly ScreenNavService _screenNavService;


        public MainMenuEntryPoint(
            LoadingCurtain loadingCurtain, 
            ScreenNavService screenNavService)
        {
            _screenNavService = screenNavService;
            _loadingCurtain = loadingCurtain;
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
            => await _screenNavService.OpenPageAsync(ContainerKey.MainMenu_Main_PageContainer, ScreenKey.MainMenu_MainPage, playAnim: false);
    }
}