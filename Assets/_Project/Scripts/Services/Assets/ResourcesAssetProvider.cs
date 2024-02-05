using UnityEngine;

namespace Services.Assets
{
    public class ResourcesAssetProvider : IAssetProvider
    {
        public T Load<T>(string path)
            where T : Object
            => Resources.Load<T>(path);

        public T[] LoadMany<T>(string path)
            where T : Object 
            => Resources.LoadAll<T>(path);
    }
}