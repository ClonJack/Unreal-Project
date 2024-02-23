using UnityEngine;
using UnrealTeam.SB.Extensions;

namespace UnrealTeam.SB.Save
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
            string serializedData = data.ToJson();
            PlayerPrefs.SetString(_dataKey, serializedData);
            PlayerPrefs.Save();
        }

        public T Load()
        {
            string serializedData = PlayerPrefs.GetString(_dataKey);
            return serializedData.FromJson<T>();
        }
    }
}