using UnityEngine;

namespace UnrealTeam.SB.Services.Assets
{
    public interface IAssetProvider
    {
        public T Load<T>(string path) where T : Object;

        public T[] LoadMany<T>(string path) where T : Object;
    }
}