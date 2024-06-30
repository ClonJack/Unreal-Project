using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnrealTeam.SB.Additional.Constants;
using UnrealTeam.SB.Configs.App;
using UnrealTeam.SB.Configs.Player;
using UnrealTeam.SB.Services.Configs;
using UnrealTeam.SB.Services.Loading;
using UnrealTeam.SB.Services.Save;

namespace UnrealTeam.SB.GameFlow.Boot
{
    public class BootEntryPoint
    {
        private readonly IConfigLoader _configLoader;
        private readonly IConfigAccess _configAccess;
        private readonly SaveService _saveService;
        private readonly SceneLoader _sceneLoader;

        public BootEntryPoint(
            IConfigLoader configLoader,
            IConfigAccess configAccess,
            SaveService saveService,
            SceneLoader sceneLoader)
        {
            _configAccess = configAccess;
            _sceneLoader = sceneLoader;
            _configLoader = configLoader;
            _saveService = saveService;
        }

        public async UniTask ExecuteAsync()
        {
#if UNITY_EDITOR
            // Требуется для корректной работы BootFromAnyScene, так как эдитор отрабатывает позже билда контейнера
            await UniTask.Yield();
#endif
            LoadSaveData();
            LoadStaticData();
            ConfigureLogs();
            await LoadTargetScene();
        }

        private async UniTask LoadTargetScene()
        {
            var appConfig = _configAccess.GetSingle<AppConfig>();
            var sceneName = appConfig.SkipMenu 
                ? appConfig.GetTargetScene() 
                : SceneNames.MainMenu;
            await _sceneLoader.LoadAsync(sceneName);
        }

        private void ConfigureLogs()
        {
            var appConfig = _configAccess.GetSingle<AppConfig>();
            Fusion.Log.LogLevel = appConfig.LogLevel;
        }

        private string GetTargetScene()
        {
            var appConfig = _configAccess.GetSingle<AppConfig>();
            return appConfig.SkipMenu 
                ? appConfig.GetTargetScene() 
                : SceneNames.MainMenu;
        }

        private void LoadSaveData()
            => _saveService.Load();

        private void LoadStaticData()
        {
            _configLoader.LoadSingle<PlayerConfig>(ConfigsPaths.PlayerConfig);
            _configLoader.LoadSingle<AppConfig>(ConfigsPaths.AppConfig);
        }
    }
}