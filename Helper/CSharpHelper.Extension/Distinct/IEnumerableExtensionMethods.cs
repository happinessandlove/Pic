/**************************************************
 * by 丁浩
 * 2015-02-04
 **************************************************/

using System.Collections.Generic;
namespace System.Linq
{
    /// <summary>
    /// IEnumerable&lt;T&gt;的扩展方法 去除重复项
    /// </summary>
    public static partial class IEnumerableExtensionMethods
    {
        /// <summary>
        /// Distinct比较器
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
