/**************************************************
 * by 丁浩
 * 2015-02-11
 **************************************************/

namespace System.Reflection
{
    /// <summary>
    /// （自定义）反射获取程序集、接口、类、类的属性
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取属性信息
        /// <para>  忽略大小写，必须为public的属性</para>
        /// </summary>
        /// <param name="classType">实体类型</param>
        /// <param name="propertyName">属性名称。</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(Type classType, string propertyName)
        {
            return classType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
        }

        /// <summary>
        /// 获取属性信息，可以为多级导航属性
        /// <para>  忽略大小写，必须为public的属性</para>
        /// </summary>
        /// <param name="classType">起始实体的类型。</param>
        /// <para>  如：typeof(Student)，对应的propertyNameString:"Clas.College.University.Name"。</para>
        /// <para>  方法执行后得到最终属性所在实体的类型。如："Clas.College.University.Name"得到"University"的类型。</para>
        /// <param name="propertyNameString">属性名称，可以为多级名称。如："Clas.College.University.Name"。</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(ref Type classType, string propertyNameString)
        {
            string propertyName = propertyNameString.IndexOf('.') < 0 ? propertyNameString : propertyNameString.Substring(0, propertyNameString.IndexOf('.'));
            PropertyInfo propertyInfo = GetProperty(classType, propertyName);
            if (propertyInfo == null) throw new Exception("多级导航属性异常：类" + classType.Name + "中不含名为" + propertyName + "的属性。");
            else
            {
                if (propertyNameString.IndexOf('.') > 0)
                {
                    classType = propertyInfo.PropertyType;
                    return GetProperty(ref classType, propertyNameString.Substring(propertyNameString.IndexOf('.') + 1));
                }
                else
                    return propertyInfo;
            }
        }
    }
}
