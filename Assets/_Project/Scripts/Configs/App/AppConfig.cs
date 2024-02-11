using System.Collections.Generic;
using Configs.App;
using UnityEngine;
using UnrealTeam.SB.Constants;

namespace  UnrealTeam.SB.Configs.App
{
    [CreateAssetMenu(menuName = "Configs/App", fileName = "AppConfig")]
    public class AppConfig : ScriptableObject, ISingleConfig
    {
        [SerializeField] private TargetScene _targetScene = TargetScene.Level;
        private Dictionary<TargetScene, string> _scenesMap = new()
        {
            { TargetScene.CharacterPlayground, SceneNames.Prototype },
        };

        public string GetTargetScene()
            => _scenesMap[_targetScene];
    }
}