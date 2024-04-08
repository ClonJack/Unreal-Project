using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnrealTeam.SB.Common.Constants;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.Configs.App
{
    [CreateAssetMenu(menuName = "Configs/App", fileName = "AppConfig")]
    public class AppConfig : ScriptableObject, ISingleConfig
    {
        [Header("Level")] [SerializeField] private TargetScene _targetScene = TargetScene.Level;
        [field: SerializeField] public bool SkipMenu { get; private set; }

        [Header("Network")] 
        [field: SerializeField] public GameMode GameMode;
        [field: SerializeField] public string SessionName;
        [field: SerializeField, Range(0, 255)] public int MaxPlayerCount;


        private readonly Dictionary<TargetScene, string> _scenesMap = new()
        {
            { TargetScene.CharacterPlayground, SceneNames.Prototype },
            { TargetScene.Level, SceneNames.Level },
        };

        public string GetTargetScene()
            => _scenesMap[_targetScene];
    }
}