using Cysharp.Threading.Tasks;
using UnrealTeam.SB.Additional.Enums.ScreenNav;
using UnrealTeam.SB.Services.Loading;
using UnrealTeam.SB.Services.Other;

namespace UnrealTeam.SB.GameFlow.MainMenu
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
        {
            var pageContainer = ContainerKey.MainMenu_Main_PageContainer;
            var mainMenuPage = ScreenKey.MainMenu_MainPage;
            await _screenNavService.OpenPageAsync(pageContainer, mainMenuPage,false);
        }
    }
}