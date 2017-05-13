/**************************************************
 * by 丁浩
 * 2014-05-29 
 **************************************************/

using System.Linq.Expressions;
using System.Web.Helpers;
namespace System.Linq
{
    /// <summary>
    /// IQueryable&lt;T&gt;的扩展方法 排序相关
    /// </summary>
    public static partial class IQueryableExtensionMethods
    {
        /// <summary>
        /// （自定义）根据属性名称对序列进行升序排序。
        /// </summary>
        /// <typeparam name="T">序列中的元素类型。</typeparam>
        /// <param name="source">待排序序列。</param>
        /// <param name="propertyName">排序依据的属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name。</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return applyOrder<T>(source, propertyName, "OrderBy");
        }

        /// <summary>
        /// （自定义）根据属性名称对序列进行降序排序。
        /// </summary>
        /// <typeparam name="T">序列中的元素类型。</typeparam>
        /// <param name="source">待排序序列。</param>
        /// <param name="propertyName">排序依据的属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name。</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return applyOrder<T>(source, propertyName, "OrderByDescending");
        }

        /// <summary>
        /// （自定义）根据属性名称以及排序方向对序列进行排序。
        /// </summary>
        /// <typeparam name="T">序列中的元素类型。</typeparam>
        /// <param name="source">待排序序列。</param>
        /// <param name="propertyName">排序依据的属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name。</param>
        /// <param name="sortDirection">排序方向。OrderDirection枚举。</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> source, string propertyName, SortDirection sortDirection)
        {
            if (sortDirection == SortDirection.Ascending)
                return applyOrder<T>(source, propertyName, "OrderBy");
            else
                return applyOrder<T>(source, propertyName, "OrderByDescending");
        }

        /// <summary>
        /// （自定义）根据属性名称对序列进行后续升序排序。
        /// </summary>
        /// <typeparam name="T">序列中的元素类型。</typeparam>
        /// <param name="source">待排序序列。</param>
        /// <param name="propertyName">排序依据的属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name。</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName)
        {
            return applyOrder<T>(source, propertyName, "ThenBy");
        }

        /// <summary>
        /// （自定义）根据属性名称对序列进行后续降序排序。
        /// </summary>
        /// <typeparam name="T">序列中的元素类型。</typeparam>
        /// <param name="source">待排序序列。</param>
        /// <param name="propertyName">排序依据的属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name。</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return applyOrder<T>(source, propertyName, "ThenByDescending");
        }

        /// <summary>
        /// （自定义）根据属性名称以及排序方向对序列进行后续排序。
        /// </summary>
        /// <typeparam name="T">序列中的元素类型。</typeparam>
        /// <param name="source">待排序序列。</param>
        /// <param name="propertyName">排序依据的属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name。</param>
        /// <param name="sortDirection">排序方向。OrderDirection枚举。</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenSort<T>(this IQueryable<T> source, string propertyName, SortDirection sortDirection)
        {
            if (sortDirection == SortDirection.Ascending)
                return applyOrder<T>(source, propertyName, "ThenOrderBy");
            else
                return applyOrder<T>(source, propertyName, "ThenOrderByDescending");
        }

        private static IOrderedQueryable<T> applyOrder<T>(IQueryable<T> source, string propertyName, string orderType)
        {
            try
            {
                Type type = typeof(T);
                ParameterExpression param = Expression.Parameter(type, "o");
                Type propertyType;
                Expression expr = ExpressionHelper.ParsePropertyName(type, propertyName, param, out propertyType);
                Type delegateType = typeof(Func<,>).MakeGenericType(type, propertyType);
                LambdaExpression lambda = Expression.Lambda(delegateType, expr, param);
                object result = typeof(Queryable).GetMethods().Single(
                a => a.Name == orderType
                && a.IsGenericMethodDefinition
                && a.GetGenericArguments().Length == 2
                && a.GetParameters().Length == 2)
                .MakeGenericMethod(type, propertyType).Invoke(null, new object[] { source, lambda });
                return (IOrderedQueryable<T>)result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
