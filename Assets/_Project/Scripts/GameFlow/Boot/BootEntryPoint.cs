using Common.Constants;
using Configs;
using Configs.App;
using Services.Configs;
using Services.Save;
using UnityEngine.SceneManagement;

namespace GameFlow
{
    public class BootEntryPoint
    {
        private readonly IConfigLoader _configLoader;
        private readonly IConfigAccess _configAccess;
        private readonly SaveService _saveService;


        public BootEntryPoint(
            IConfigLoader configLoader,
            IConfigAccess configAccess,
            SaveService saveService)
        {
            _configLoader = configLoader;
            _configAccess = configAccess;
            _saveService = saveService;
        }

        public void Execute()
        {
            LoadSaveData();
            LoadStaticData();
            LoadTargetScene();
        }

        private void LoadSaveData()
            => _saveService.Load();

        private void LoadStaticData()
        {
            _configLoader.LoadSingle<PlayerConfig>(ConfigsPaths.PlayerConfig);
            _configLoader.LoadSingle<AppConfig>(ConfigsPaths.AppConfig);
        }

        private void LoadTargetScene()
        {
            string targetScene = _configAccess.GetSingle<AppConfig>().GetTargetScene();
            SceneManager.LoadScene(targetScene);
        }
    }
}