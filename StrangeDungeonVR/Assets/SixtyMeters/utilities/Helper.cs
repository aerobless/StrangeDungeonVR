using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.utilities
{
    public static class Helper
    {
        /**
         * Returns a random entry from the list.
         */
        public static T GETRandomFromList<T>(IReadOnlyList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}