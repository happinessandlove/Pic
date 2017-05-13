
/**************************************************
 * by 丁浩
 * 2016-01-30 
 * **************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 下拉列表框的Html辅助方法类。
    /// </summary>
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// （自定义）Html扩展方法，生成下拉列表框。
        /// <para>  下拉列表框的值属性：自动匹配。第一缺省值为带有[SelectListValueProperty]标识的属性，第二缺省值为带有[Key]标识的属性，第三缺省值为“id”，第四缺省值为“类型名称Id”。</para>
        /// <para>  下拉列表框的显示属性：自动匹配。第一缺省值为带有[SelectListTextProperty]标识的属性，第二缺省值为“name”，第三缺省值为“类型名称name”。</para> 
        /// <para>  数据源：数据表全部数据。</para>
        /// <para>  排序方式：按主键升序排序。</para>
        /// </summary>
        /// <typeparam name="TPageModel">实体类型，通常为页面强类型。</typeparam>
        /// <typeparam name="TProperty">属性类型。</typeparam>
        /// <typeparam name="TSourceDataModel">下拉列表框数据来源的实体类型。</typeparam>
        /// <param name="html"></param>
        /// <param name="propertyExpression">下拉列表框的属性。依此确定下拉列表框的名称以及当前选中项的值。</param>
        /// <param name="sourceEntityExpression">指定数据来源实体类型，lambda表达式。为外键导航属性，形如：model=>model.NavigationProperty。</param>
        /// <param name="insertDefaultOption">是否在选项之前插入一条默认选项，如：“-请选择-”。缺省值为true。</param>
        /// <param name="defaultOptionText">插入默认选项的显示文字，缺省值为空。insertDefaultOption为true时，有效。</param>
        /// <param name="defaultOptionValue">插入默认选项的值，缺省值为字符串"0"。insertDefaultOption为true时，有效。</param>
        /// <returns></returns>   
        public static MvcHtmlString SelectListFor<TPageModel, TProperty, TSourceDataModel>
        (
            this HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TProperty>> propertyExpression,
            Expression<Func<TPageModel, TSourceDataModel>> sourceEntityExpression,
            bool insertDefaultOption = true,
            string defaultOptionText = "",
            string defaultOptionValue = "0"
            ) where TSourceDataModel : class
        {
            return selectListFor(html, propertyExpression, sourceEntityExpression, null, null, null, null, insertDefaultOption, defaultOptionText, defaultOptionValue);
        }

        /// <summary>
        /// （自定义）Html扩展方法，生成下拉列表框。
        /// <para>  下拉列表框的值属性：指定。</para>
        /// <para>  下拉列表框的显示属性：指定。</para> 
        /// <para>  数据源：数据表全部数据。</para>
        /// <para>  排序方式：按主键升序排序。</para>
        /// </summary>
        /// <typeparam name="TPageModel">实体类型，通常为页面强类型。</typeparam>
        /// <typeparam name="TProperty">属性类型。</typeparam>
        /// <typeparam name="TSourceDataModel">下拉列表框数据来源的实体类型。</typeparam>
        /// <typeparam name="TBindValueProperty">绑定至下拉列表框的值属性类型。</typeparam>
        /// <typeparam name="TBindTextProperty">绑定至下拉列表框的显示属性类型。</typeparam>
        /// <param name="html"></param>
        /// <param name="propertyExpression">下拉列表框的属性。依此确定下拉列表框的名称以及当前选中项的值。</param>
        /// <param name="sourceEntityExpression">指定数据来源实体类型，lambda表达式。为外键导航属性，形如：model=>model.NavigationProperty。</param>
        /// <param name="bindValuePropertyExpression">绑定至下拉列表框的值属性表达式。</param>
        /// <param name="bindTextPropertyExpression">绑定至下拉列表框的显示属性的表达式。</param>
        /// <param name="insertDefaultOption">是否在选项之前插入一条默认选项，如：“-请选择-”。缺省值为true。</param>
        /// <param name="defaultOptionText">插入默认选项的显示文字，缺省值为空。insertDefaultOption为true时，有效。</param>
        /// <param name="defaultOptionValue">插入默认选项的值，缺省值为字符串"0"。insertDefaultOption为true时，有效。</param>
        /// <returns></returns>
        public static MvcHtmlString SelectListFor<TPageModel, TProperty, TSourceDataModel, TBindValueProperty, TBindTextProperty>
        (
            this HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TProperty>> propertyExpression,
            Expression<Func<TPageModel, TSourceDataModel>> sourceEntityExpression,
            Expression<Func<TSourceDataModel, TBindValueProperty>> bindValuePropertyExpression,
            Expression<Func<TSourceDataModel, TBindTextProperty>> bindTextPropertyExpression,
            bool insertDefaultOption = true,
            string defaultOptionText = "",
            string defaultOptionValue = "0"
            ) where TSourceDataModel : class
        {
            return selectListFor(html, propertyExpression, sourceEntityExpression, bindValuePropertyExpression, bindTextPropertyExpression, null, null, insertDefaultOption, defaultOptionText, defaultOptionValue);
        }

        /// <summary>
        /// （自定义）Html扩展方法，生成下拉列表框。
        /// <para>  下拉列表框的值属性：自动匹配。第一缺省值为带有[SelectListValueProperty]标识的属性，第二缺省值为带有[Key]标识的属性，第三缺省值为“id”，第四缺省值为“类型名称Id”。</para>
        /// <para>  下拉列表框的显示属性：自动匹配。第一缺省值为带有[SelectListTextProperty]标识的属性，第二缺省值为“name”，第三缺省值为“类型名称name”。</para> 
        /// <para>  数据源：可指定查询条件。</para>
        /// <para>  排序方式：可指定。</para>
        /// </summary>
        /// <typeparam name="TPageModel">实体类型，通常为页面强类型。</typeparam>
        /// <typeparam name="TProperty">属性类型。</typeparam>
        /// <typeparam name="TSourceDataModel">下拉列表框数据来源的实体类型。</typeparam>
        /// <param name="html"></param>
        /// <param name="propertyExpression">下拉列表框的属性。依此确定下拉列表框的名称以及当前选中项的值。</param>
        /// <param name="sourceEntityExpression">指定数据来源实体类型，lambda表达式。为外键导航属性，形如：model=>model.NavigationProperty。</param>
        /// <param name="filter">下拉列表框数据源的谓词筛选值，如：t=>t.Property==value。</param>
        /// <param name="orderBy">下拉列表框数据源的排序依据和方式，如：p=>p.OrderBy(t=>t.Property1).ThenBy(t=>t.Property2)。</param>
        /// <param name="insertDefaultOption">是否在选项之前插入一条默认选项，如：“-请选择-”。缺省值为true。</param>
        /// <param name="defaultOptionText">插入默认选项的显示文字，缺省值为空。insertDefaultOption为true时，有效。</param>
        /// <param name="defaultOptionValue">插入默认选项的值，缺省值为字符串"0"。insertDefaultOption为true时，有效。</param>
        /// <returns></returns>
        public static MvcHtmlString SelectListFor<TPageModel, TProperty, TSourceDataModel>
        (
            this HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TProperty>> propertyExpression,
            Expression<Func<TPageModel, TSourceDataModel>> sourceEntityExpression,
            Expression<Func<TSourceDataModel, bool>> filter,
            Func<IQueryable<TSourceDataModel>, IOrderedQueryable<TSourceDataModel>> orderBy,
            bool insertDefaultOption = true,
            string defaultOptionText = "",
            string defaultOptionValue = "0"
            ) where TSourceDataModel : class
        {
            return selectListFor(html, propertyExpression, sourceEntityExpression, null, null, filter, orderBy, insertDefaultOption, defaultOptionText, defaultOptionValue);
        }

        /// <summary>
        /// （自定义）Html扩展方法，生成下拉列表框。
        /// <para>  下拉列表框的值属性：指定。</para>
        /// <para>  下拉列表框的显示属性：指定。</para> 
        /// <para>  数据源：可指定查询条件。</para>
        /// <para>  排序方式：可指定。</para>
        /// </summary>
        /// <typeparam name="TPageModel">实体类型，通常为页面强类型。</typeparam>
        /// <typeparam name="TProperty">属性类型。</typeparam>
        /// <typeparam name="TSourceDataModel">下拉列表框数据来源的实体类型。</typeparam>
        /// <typeparam name="TBindValueProperty">绑定至下拉列表框的值属性类型。</typeparam>
        /// <typeparam name="TBindTextProperty">绑定至下拉列表框的显示属性类型。</typeparam>
        /// <param name="html"></param>
        /// <param name="propertyExpression">下拉列表框的属性。依此确定下拉列表框的名称以及当前选中项的值。</param>
        /// <param name="sourceEntityExpression">指定数据来源实体类型，lambda表达式。为外键导航属性，形如：model=>model.NavigationProperty。</param>
        /// <param name="bindValuePropertyExpression">绑定至下拉列表框的值属性表达式。</param>
        /// <param name="bindTextPropertyExpression">绑定至下拉列表框的显示属性的表达式。</param>
        /// <param name="filter">下拉列表框数据源的谓词筛选值，如：t=>t.Property==value。</param>
        /// <param name="orderBy">下拉列表框数据源的排序依据和方式，如：p=>p.OrderBy(t=>t.Property1).ThenBy(t=>t.Property2)。</param>
        /// <param name="insertDefaultOption">是否在选项之前插入一条默认选项，如：“-请选择-”。缺省值为true。</param>
        /// <param name="defaultOptionText">插入默认选项的显示文字，缺省值为空。insertDefaultOption为true时，有效。</param>
        /// <param name="defaultOptionValue">插入默认选项的值，缺省值为字符串"0"。insertDefaultOption为true时，有效。</param>
        /// <returns></returns>
        public static MvcHtmlString SelectListFor<TPageModel, TProperty, TSourceDataModel, TBindValueProperty, TBindTextProperty>
        (
            this HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TProperty>> propertyExpression,
            Expression<Func<TPageModel, TSourceDataModel>> sourceEntityExpression,
            Expression<Func<TSourceDataModel, TBindValueProperty>> bindValuePropertyExpression,
            Expression<Func<TSourceDataModel, TBindTextProperty>> bindTextPropertyExpression,
            Expression<Func<TSourceDataModel, bool>> filter,
            Func<IQueryable<TSourceDataModel>, IOrderedQueryable<TSourceDataModel>> orderBy,
            bool insertDefaultOption = true,
            string defaultOptionText = "",
            string defaultOptionValue = "0"
            ) where TSourceDataModel : class
        {
            return selectListFor(html, propertyExpression, sourceEntityExpression, bindValuePropertyExpression, bindTextPropertyExpression, filter, orderBy, insertDefaultOption, defaultOptionText, defaultOptionValue);
        }


        /// <summary>
        /// （自定义）Html扩展方法，枚举类型生成下拉列表框。
        /// <para>  下拉列表框的值属性：枚举类型的值</para>
        /// <para>  下拉列表框的显示属性：枚举类型的名称</para> 
        /// </summary>
        /// <typeparam name="TPageModel">实体类型，通常为页面强类型。</typeparam>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <param name="html"></param>
        /// <param name="propertyExpression">下拉列表框的属性。依此确定下拉列表框的名称以及当前选中项的值。</param>
        /// <param name="insertDefaultOption">是否在选项之前插入一条默认选项，如：“-请选择-”。缺省值为true。</param>
        /// <param name="defaultOptionText">插入默认选项的显示文字，缺省值为空。insertDefaultOption为true时，有效。</param>
        /// <param name="defaultOptionValue">插入默认选项的值，缺省值为字符串"0"。insertDefaultOption为true时，有效。</param>
        /// <returns></returns>   
        public static MvcHtmlString SelectListFor<TPageModel, TEnum>
        (
            this HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TEnum>> propertyExpression,
            bool insertDefaultOption = true,
            string defaultOptionText = "",
            string defaultOptionValue = "0"
            )
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TPageModel, TEnum>(propertyExpression, html.ViewData);
            string value = (metadata.Model == null ? null : metadata.Model.ToString());
            List<SelectListItem> listItems = SelectListHelper.GetEnumSelectListItems(typeof(TEnum), value); 
            if (insertDefaultOption) listItems.Insert(0, new SelectListItem { Text = defaultOptionText, Value = defaultOptionValue, Selected = value == null });
            return html.DropDownListFor(propertyExpression, listItems);
        }


        #region 私有方法
        private static MvcHtmlString selectListFor<TPageModel, TProperty, TSourceDataModel, TBindValueProperty, TBindTextProperty>
            (
            HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TProperty>> propertyExpression,
            Expression<Func<TPageModel, TSourceDataModel>> sourceEntityExpression,
            Expression<Func<TSourceDataModel, TBindValueProperty>> bindValuePropertyExpression,
            Expression<Func<TSourceDataModel, TBindTextProperty>> bindTextPropertyExpression,
            Expression<Func<TSourceDataModel, bool>> filter,
            Func<IQueryable<TSourceDataModel>, IOrderedQueryable<TSourceDataModel>> orderBy,
            bool insertDefaultOption,
            string defaultOptionText,
            string defaultOptionValue
            ) where TSourceDataModel : class
        {
            PropertyInfo valueProperty = typeof(TSourceDataModel).GetProperty(((MemberExpression)bindValuePropertyExpression.Body).Member.Name);
            PropertyInfo textProperty = typeof(TSourceDataModel).GetProperty(((MemberExpression)bindTextPropertyExpression.Body).Member.Name);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TPageModel, TProperty>(propertyExpression, html.ViewData);
            string value = (metadata.Model == null ? "0" : metadata.Model.ToString());
            List<SelectListItem> list = SelectListHelper.GetSelectListItems<TSourceDataModel>(valueProperty, textProperty, value, filter, orderBy);
            if (insertDefaultOption) list.Insert(0, new SelectListItem { Text = defaultOptionText, Value = defaultOptionValue });
            return html.DropDownListFor(propertyExpression, list);
        }

        private static MvcHtmlString selectListFor<TPageModel, TProperty, TSourceDataModel>
            (
            HtmlHelper<TPageModel> html,
            Expression<Func<TPageModel, TProperty>> propertyExpression,
            Expression<Func<TPageModel, TSourceDataModel>> sourceEntityExpression,
            string bindValuePropertyName,
            string bindTextPropertyName,
            Expression<Func<TSourceDataModel, bool>> filter,
            Func<IQueryable<TSourceDataModel>, IOrderedQueryable<TSourceDataModel>> orderBy,
            bool insertDefaultOption,
            string defaultOptionText,
            string defaultOptionValue
            ) where TSourceDataModel : class
        {
            PropertyInfo valueProperty = SelectListHelper.GetSelectListValuePropertyInfo(typeof(TSourceDataModel), bindValuePropertyName);
            PropertyInfo textProperty = SelectListHelper.GetSelectListTextPropertyInfo(typeof(TSourceDataModel), bindTextPropertyName);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TPageModel, TProperty>(propertyExpression, html.ViewData);
            string value = (metadata.Model == null ? "0" : metadata.Model.ToString());
            List<SelectListItem> list = SelectListHelper.GetSelectListItems<TSourceDataModel>(valueProperty, textProperty, value, filter, orderBy);
            if (insertDefaultOption) list.Insert(0, new SelectListItem { Text = defaultOptionText, Value = defaultOptionValue });
            return html.DropDownListFor(propertyExpression, list);
        }
        #endregion
    }
}