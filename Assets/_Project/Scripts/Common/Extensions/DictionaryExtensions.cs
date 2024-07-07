using System.Collections.Generic;

namespace UnrealTeam.SB.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            dictionary.Add(key, newValue);
            return newValue;
        }
    }
}