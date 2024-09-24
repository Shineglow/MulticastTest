using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Network.Extensions
{
    public static class EnumerableExtension
    {
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            return collection.ElementAt(Random.Range(0, collection.Count()));
        }
    }
}