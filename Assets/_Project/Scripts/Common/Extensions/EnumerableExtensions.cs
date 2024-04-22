using System.Collections.Generic;

namespace UnrealTeam.SB.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Same<T>(this IEnumerable<T> thisCollection, IEnumerable<T> otherCollection)
        {
            using var thisEnumerator = thisCollection.GetEnumerator();
            using var otherEnumerator = otherCollection.GetEnumerator();
            
            while (thisEnumerator.MoveNext())
            {
                if (!otherEnumerator.MoveNext())
                    return false;

                if (!EqualityComparer<T>.Default.Equals(thisEnumerator.Current, otherEnumerator.Current))
                    return false;
            }

            if (otherEnumerator.MoveNext())
                return false;

            return true;
        }
    }
}