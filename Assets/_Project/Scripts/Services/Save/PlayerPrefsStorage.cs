using UnityEngine;
using UnrealTeam.SB.Common.Extensions;

namespace UnrealTeam.SB.Services.Save
{
    public class PlayerPrefsStorage<T> : ISaveStorage<T>
    {
        private readonly string _dataKey;


        public PlayerPrefsStorage(string dataKey)
        {
            _dataKey = dataKey;
        }

        public void Save(T data)
        {
            var serializedData = data.ToJson();
            PlayerPrefs.SetString(_dataKey, serializedData);
            PlayerPrefs.Save();
        }

        public T Load()
        {
            var serializedData = PlayerPrefs.GetString(_dataKey);
            return serializedData.FromJson<T>();
        }
    }
}