using System.Collections.Generic;
using Common.Constants;
using UnityEngine;

namespace Configs.App
{
    [CreateAssetMenu(menuName = "Configs/App", fileName = "AppConfig")]
    public class AppConfig : ScriptableObject, ISingleConfig
    {
        [SerializeField] private TargetScene _targetScene = TargetScene.Level;

        private Dictionary<TargetScene, string> _scenesMap = new()
        {
            { TargetScene.Level, SceneNames.Level },
            { TargetScene.CharacterPlayground, SceneNames.CharacterPlayground },
        };


        public string GetTargetScene()
            => _scenesMap[_targetScene];
    }
}