#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnrealTeam.SB.Constants;

namespace GameFlow
{
    [InitializeOnLoad]
    public class BootFromAnyScene
    {
        static BootFromAnyScene()
        {
            EditorApplication.playModeStateChanged += Run;
        }

        private static void Run(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredPlayMode)
                return;

            EditorApplication.playModeStateChanged -= Run;
            
            var currentScene = SceneManager.GetActiveScene();
            
            if (IsBootScene(currentScene))
                return;
            
            if (IsSceneInBuild(currentScene))
                LoadBootScene();
        }

        private static bool IsBootScene(Scene currentScene) 
            => currentScene.name == SceneNames.Boot;

        private static bool IsSceneInBuild(Scene currentScene) 
            => EditorBuildSettings.scenes.Any(s => s.path == currentScene.path);

        private static void LoadBootScene() 
            => SceneManager.LoadScene(SceneNames.Boot);
    }
}

#endif