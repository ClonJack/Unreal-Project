using UnityEngine;

namespace Common.Extensions
{
    public static class JsonExtensions
    {
        public static T FromJson<T>(this string json) 
            => JsonUtility.FromJson<T>(json);

        public static string ToJson<T>(this T obj)
            => JsonUtility.ToJson(obj);
    }
}