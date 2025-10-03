using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class ClassHelpers
    {
        /// <summary>
        /// Merges a dictionary left (right (other) keys override left (me) keys if duplicates exist)
        /// </summary>
        /// <param name="me">caller dictionary</param>
        /// <param name="others">callee dictionary</param>
        /// <typeparam name="T">dictionary class</typeparam>
        /// <typeparam name="K">key var type</typeparam>
        /// <typeparam name="V">value var type</typeparam>
        /// <returns></returns>
        public static T MergeDictionary<T, K, V>(this T me, params IDictionary<K, V>[] others)
            where T : IDictionary<K, V>, new()
        {
            T newMap = new();
            foreach (IDictionary<K, V> src in
                     (new List<IDictionary<K, V>> { me }).Concat(others))
            {
                foreach (KeyValuePair<K, V> p in src)
                {
                    newMap[p.Key] = p.Value;
                }
            }

            return newMap;
        }
    }
}