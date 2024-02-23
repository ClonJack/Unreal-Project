using System;
using System.Collections.Generic;
using System.Linq;
using UnrealTeam.SB.Configs.Common;
using UnrealTeam.SB.Services.Assets;
using Object = UnityEngine.Object;

namespace UnrealTeam.SB.Services.Configs
{
    public class ConfigProvider : IConfigLoader, IConfigAccess
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<Type, Object> _singleConfigs = new();
        private readonly Dictionary<Type, Dictionary<string, IMultipleConfig>> _multipleConfigs = new();


        public ConfigProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void LoadSingle<TConfig>(string path)
            where TConfig : Object, ISingleConfig
            => _singleConfigs[typeof(TConfig)] = _assetProvider.Load<TConfig>(path);

        public void LoadMultiple<TConfig>(string path)
            where TConfig : Object, IMultipleConfig
            => _multipleConfigs[typeof(TConfig)] = _assetProvider
                .LoadMany<TConfig>(path)
                .ToDictionary(c => c.Id, c => (IMultipleConfig)c);

        public TConfig GetSingle<TConfig>()
            where TConfig : Object, ISingleConfig
            => _singleConfigs[typeof(TConfig)] as TConfig;

        public TConfig GetMultiple<TConfig>(string id)
            where TConfig : Object, IMultipleConfig
            => (TConfig)_multipleConfigs[typeof(TConfig)][id];

        public IEnumerable<TConfig> GetAllMultiple<TConfig>()
            where TConfig : Object, IMultipleConfig
            => _multipleConfigs[typeof(TConfig)].Values.Cast<TConfig>();
    }
}