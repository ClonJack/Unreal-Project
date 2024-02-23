using System.Collections.Generic;
using UnityEngine;
using UnrealTeam.SB.Common.Constants;
using UnrealTeam.SB.Configs.Common;

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