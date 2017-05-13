/**************************************************
 * by 丁浩
 * 2014-05-29 
 * **************************************************/

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// （自定义）不参与查询的标识。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NonQueryAttribute : Attribute
    {
    }

    /// <summary>
    /// （自定义）外键关联查询的标记。
    /// <para>  Config文件中设置该标识是加在外键属性上还是导航属性上。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class AssociatedQueryAttribute : Attribute
    {
        internal QueryPropertyType QueryPropertyType { get; set; }
        internal string AssociatedPropertyName { get; set; }
        internal string DisplayName { get; set; }
        internal string ValuePropertyName { get; set; }
        internal string TextPropertyName { get; set; }

        /// <summary>
        /// 构造函数。
        /// <para>  展现形式：下拉列表框。</para>
        /// <para>  关联深度：一级。</para>
        /// <para>  查询项的显示名称：自动匹配。</para>
        /// <para>  下拉列表框的value值对应的属性：自动匹配。</para>
        /// <para>  下拉列表框的显示值对应的属性：自动匹配。</para>
        /// </summary>
        public AssociatedQueryAttribute()
        {
            this.QueryPropertyType = QueryPropertyType.Select;
        }

        /// <summary>
        /// 构造函数。
        /// <para>  展现形式：下拉列表框。</para>
        /// <para>  关联深度：多级。</para>
        /// <para>  查询项的显示名称：显式指定。</para>
        /// <para>  下拉列表框的value值对应的属性：显式指定。</para>
        /// <para>  下拉列表框的显示值对应的属性：显式指定。</para>
        /// </summary>
        /// <param name="DisplayName">查询项的显示名称。</param>
        /// <param name="ValueFieldName">下拉列表框的value值对应的属性，大小写不区分。多级关联查询时，必须写明完整的属性名称，如：Clas.College.Id。</param>
        /// <param name="TextFieldName">下拉列表框的显示值对应的属性，大小写不区分。多级关联查询时，必须写明完整的属性名称，如：Clas.College.Name。</param>
        public AssociatedQueryAttribute(string DisplayName, string ValueFieldName, string TextFieldName)
        {
            if (string.IsNullOrEmpty(DisplayName.Trim()) || string.IsNullOrEmpty(ValueFieldName.Trim()) || string.IsNullOrEmpty(TextFieldName.Trim()))
                throw new Exception("AssociatedQueryAttribute异常：请显式指定AssociatedModelType、DisplayName、ValueFieldName、TextFieldName的值");
            
            this.QueryPropertyType = QueryPropertyType.Select;
            this.DisplayName = DisplayName.Trim();
            this.ValuePropertyName = ValueFieldName.Trim();
            this.TextPropertyName = TextFieldName.Trim();
        }

        /// <summary>
        /// 构造函数。
        /// <para>  展现形式：显式指定，非下拉列表框。</para>
        /// <para>  关联深度：多级。</para>
        /// <para>  查询项的显示名称：显式指定。</para>
        /// </summary>
        /// <param name="QueryPropertyType">QueryPropertyType枚举值，关联查询体现形式，不能为QueryPropertyType.Select。</param>
        /// <param name="AssociatedPropertyName">关联的属性名称，不区分大小写。多级关联查询时，必须写明完整的属性名称，如：Clas.College.Name。</param>
        /// <param name="DisplayName">查询项的显示名称。</param>
        public AssociatedQueryAttribute(QueryPropertyType QueryPropertyType, string AssociatedPropertyName, string DisplayName)
        {
            if (QueryPropertyType == QueryPropertyType.Select)
                throw new Exception("关联查询生成为下拉列表框时，请使用AssociatedQueryAttribute另一构造函数");
            
            this.QueryPropertyType = QueryPropertyType;
            this.AssociatedPropertyName = AssociatedPropertyName.Trim();
            this.DisplayName = DisplayName.Trim();
        }
    }
}
