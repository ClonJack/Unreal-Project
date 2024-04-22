using Cysharp.Threading.Tasks;
using UnrealTeam.SB.Common.Constants;
using UnrealTeam.SB.Configs.App;
using UnrealTeam.SB.Configs.Player;
using UnrealTeam.SB.Services.Configs;
using UnrealTeam.SB.Services.Loading;
using UnrealTeam.SB.Services.Save;

namespace UnrealTeam.SB.GameFlow
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
            var sceneName = GetTargetScene();
            await _sceneLoader.LoadAsync(sceneName);
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