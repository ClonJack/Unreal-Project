#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnrealTeam.SB.Constants;

namespace UnrealTeam.SB.GameFlow
{
    [InitializeOnLoad]
    public class BootFromAnyScene
    {
       /* static BootFromAnyScene()
        {
            EditorApplication.playModeStateChanged += Run;
        }

        private static void Run(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredPlayMode)
                return;

            EditorApplication.playModeStateChanged -= Run;
            Scene currentScene = SceneManager.GetActiveScene();
            
            if (currentScene.name == SceneNames.Boot)
                return;
            
            if (EditorBuildSettings.scenes.Any(s => s.path == currentScene.path))
                SceneManager.LoadScene(SceneNames.Boot);
        }*/
    }
}

#endif