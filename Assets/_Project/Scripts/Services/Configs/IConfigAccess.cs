using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace UnrealTeam.SB.Configs
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