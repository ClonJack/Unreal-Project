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
        private readonly SaveService _saveService;
        private readonly SceneLoader _sceneLoader;
        
        public BootEntryPoint(
            IConfigLoader configLoader,
            SaveService saveService,
            SceneLoader sceneLoader)
        {
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
            // await LoadTargetScene();
            await _sceneLoader.LoadAsync(SceneNames.MainMenu);
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