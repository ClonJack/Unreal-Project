using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Loading
{
    public class SceneLoader
    {
        private readonly LoadingCurtain _loadingCurtain;

        
        public SceneLoader(LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
        }
        
        public async UniTask LoadAsync(string sceneName, bool showCurtain = false)
        {
            if (IsCurrentScene(sceneName))
                return;

            if (showCurtain)
                await _loadingCurtain.ShowAsync();
            
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            await loadingOperation;

            await _loadingCurtain.HideAsync();
        }

        private static bool IsCurrentScene(string sceneName) 
            => SceneManager.GetActiveScene().name == sceneName;
    }
}