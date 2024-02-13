using Common.Constants;
using Configs;
using Configs.App;
using Cysharp.Threading.Tasks;
using Services.Configs;
using Services.Loading;
using Services.Save;

namespace GameFlow
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
            _sceneLoader = sceneLoader;
            _configLoader = configLoader;
            _configAccess = configAccess;
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

        private async UniTask LoadTargetScene()
        {
            string targetScene = _configAccess.GetSingle<AppConfig>().GetTargetScene();
            await _sceneLoader.LoadAsync(targetScene);
        }
    }
}