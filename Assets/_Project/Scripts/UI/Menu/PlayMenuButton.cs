using Configs.App;
using Cysharp.Threading.Tasks;
using Services.Configs;
using Services.Loading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Menu
{
    public class PlayMenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IConfigAccess _configAccess;
        private SceneLoader _sceneLoader;


        [Inject]
        public void Construct(IConfigAccess configAccess, SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _configAccess = configAccess;
        }

        private void Awake()
        {
            _button.onClick.AddListener(LoadTargetScene);
        }
        

        private void LoadTargetScene()
        {
            string targetScene = _configAccess.GetSingle<AppConfig>().GetTargetScene();
            _sceneLoader.LoadAsync(targetScene, showCurtain: true).Forget();
        }
    }
}