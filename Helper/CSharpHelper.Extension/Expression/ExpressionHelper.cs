/**************************************************
 * by 丁浩
 * 2013-08-27
 **************************************************/
 
 using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// （自定义）表达式树相关的辅助类
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// 根据属性名称构建一个访问属性的表达式树，形如：p=>p.Name
        /// </summary>
        /// <param name="objectType">属性所在类的类型</param>
        /// <param name="propertyName">属性名称，可为多级导航属性，忽略大小写，必须为public。如：Clas.College.Name</param>
        /// <param name="param">表达式树的形参</param>
        /// <param name="propertyType">out 属性类型</param>
        /// <returns></returns>
        public static Expression ParsePropertyName(Type objectType, string propertyName, ParameterExpression param, out Type propertyType)
        {
            try
            {
                PropertyInfo property = null;
                string[] propertys = propertyName.Split('.');
                if (propertys.Length == 1)
                {
                    property = ReflectionHelper.GetProperty(objectType, propertyName);
                    if (property == null) throw new Exception(objectType.Name + "中没有名为" + propertyName + "的属性");
                    propertyType = property.PropertyType;
                    return Expression.Property(param, propertyName);
                }
                else
                {
                    Expression propertyAccess = param;
                    propertyType = objectType;
                    for (int i = 0; i < propertys.Length; i++)
                    {
                        property = ReflectionHelper.GetProperty(propertyType, propertys[i]);
                        if (property == null) throw new Exception(objectType.Name + "中没有名为" + propertyName + "的属性");
                        propertyType = property.PropertyType;
                        propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    }
                    return propertyAccess;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
