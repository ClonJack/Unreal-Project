using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnrealTeam.SB.Additional.Constants;
using UnrealTeam.SB.Configs.Common;
using LogType = Fusion.LogType;

namespace UnrealTeam.SB.Configs.App
{
    [CreateAssetMenu(menuName = "Configs/App", fileName = "AppConfig")]
    public class AppConfig : SoSingleConfig
    {
        [Header("Level")] [SerializeField] private TargetScene _targetScene = TargetScene.Level;
        [field: SerializeField] public bool SkipMenu { get; private set; }
        [field: SerializeField] public LogType LogLevel { get; private set; }

        
        [field: Header("Network")] 
        [field: SerializeField] public GameMode GameMode { get; private set; }
        [field: SerializeField] public string SessionName { get; private set; }
        [field: SerializeField, Range(0, 255)] public int MaxPlayerCount { get; private set; }


        private readonly Dictionary<TargetScene, string> _scenesMap = new()
        {
            { TargetScene.CharacterPlayground, SceneNames.Prototype },
            { TargetScene.Level, SceneNames.Level },
        };

        public string GetTargetScene() => _scenesMap[_targetScene];
    }
}