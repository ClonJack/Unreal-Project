using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Loading
{
    public class SceneLoader
    {
        public async UniTask LoadAsync(string sceneName)
        {
            if (IsCurrentScene(sceneName))
                return;
            
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            await loadingOperation;
        }

        private static bool IsCurrentScene(string sceneName) 
            => SceneManager.GetActiveScene().name == sceneName;
    }
}