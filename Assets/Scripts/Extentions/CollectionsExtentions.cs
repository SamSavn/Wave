using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionsExtentions
{
    public static T GetRandom<T>(this IEnumerable<T> collection) where T : class
    {
        if (collection.IsNullOrEmpty())
            return null;

        int random = 0;

        if (collection is List<T> list)
        {
            random = Random.Range(0, list.Count);
            return list[random];
        }
        else if (collection is T[] array)
        {
            random = Random.Range(0, array.Length);
            return array[random];
        }

        random = Random.Range(0, collection.Count());
        return collection.ElementAt(random);
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection == null)
            return false;

        if (collection is List<T> list)
        {
            return list.Count == 0;
        }
        else if (collection is T[] array)
        {
            return array.Length == 0;
        }

        return collection.Count() == 0;
    }
}