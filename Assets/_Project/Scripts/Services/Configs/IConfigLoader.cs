using UnityEngine;

namespace UnrealTeam.SB.Configs
{
    public interface IConfigLoader
    {
        public void LoadSingle<TConfig>(string path) 
            where TConfig : Object, ISingleConfig;        
        
        public void LoadMultiple<TConfig>(string path) 
            where TConfig : Object, IMultipleConfig;
    }
}