using System.Collections.Generic;
using UnityEngine;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.Services.Configs
{
    public interface IConfigAccess
    {
        public TConfig GetSingle<TConfig>()
            where TConfig : Object, ISingleConfig;

        public TConfig GetMultiple<TConfig>(string id)
            where TConfig : Object, IMultipleConfig;
        
        public IEnumerable<TConfig> GetAllMultiple<TConfig>() 
            where TConfig : Object, IMultipleConfig;
    }
}