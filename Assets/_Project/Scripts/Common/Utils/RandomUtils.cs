using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnrealTeam.SB.Common.Utils
{
    public static class RandomUtils
    {
        /// <summary>
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public static int Generate(int min, int max)
            => Random.Range(min, max + 1);

        /// <summary>
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public static float Generate(float min, float max)
            => Random.Range(min, max);
        
        /// <summary>
        /// </summary>
        /// <param name="chance">Chance coefficient from 0 to 1</param>
        /// <returns></returns>
        public static bool TryChance(float chance)
        {
            if (chance < 0 || chance > 1)
                throw new InvalidOperationException("InvalidChance");
            
            if (chance == 0)
                return false;
            
            return chance >= Generate(0.0f, 1.0f);
        }

        public static T PickOne<T>(IEnumerable<T> collection)
            => PickMany(collection, 1).Single();

        public static IEnumerable<T> PickMany<T>(IEnumerable<T> collection, int count) 
            => collection.OrderBy(_ => Guid.NewGuid()).Take(count);

        public static Vector3 GenerateFromRange(Vector3 range)
            => new(GenerateFromRange(range.x), GenerateFromRange(range.y), GenerateFromRange(range.z));

        public static float GenerateFromRange(float range)
        {
            if (range == 0)
                return 0;
            
            float absValue = Mathf.Abs(range);
            return Generate(-absValue, absValue);
        }
    }
}