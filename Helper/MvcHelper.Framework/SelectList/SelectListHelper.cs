/**************************************************
 * by 丁浩
 * 2016-01-30
 * **************************************************/

using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）SelectList辅助类。
    /// </summary>
    public class SelectListHelper
    {
        /// <summary>
        /// 获取下拉列表框的值属性信息。
        /// </summary>
        /// <param name="classType">实体类型。</param>
        /// <param name="valuePropertyName">值属性的名称，不区分大小写。若为null，则第一缺省值为带有[SelectListValueProperty]标识的属性，第二缺省值为带有[Key]标识的属性，第三缺省值为“id”，第四缺省值为“类型名称Id”。</param>
        /// <returns></returns>
        public static PropertyInfo GetSelectListValuePropertyInfo(Type classType, string valuePropertyName = null)
        {
            PropertyInfo property = null;
            if (valuePropertyName != null)
            {
                property = ReflectionHelper.GetProperty(classType, valuePropertyName);
                if (property == null) throw new Exception("类" + classType.Name + "没有名为" + valuePropertyName + "的属性");
            }
            else
            {
                property = classType.GetProperties().FirstOrDefault(t => t.GetCustomAttribute(typeof(SelectListValuePropertyAttribute), false) != null);
                if (property == null)
                {
                    property = classType.GetProperties().FirstOrDefault(t => t.GetCustomAttribute(typeof(KeyAttribute), false) != null);
                    if (property == null)
                    {
                        property = ReflectionHelper.GetProperty(classType, "id");
                        if (property == null)
                        {
                            property = ReflectionHelper.GetProperty(classType, classType.Name + "id");
                            if (property == null) throw new Exception("类" + classType.Name + "没有可以作为下拉列表框选项值的属性");
                        }
                    }
                }
            }
            return property;
        }

        /// <summary>
        /// 获取下拉列表框的显示属性信息。
        /// </summary>
        /// <param name="classType">实体类型。</param>
        /// <param name="textPropertyName">显示属性的名称，不区分大小写。若为null，则第一缺省值为带有[SelectListTextProperty]标识的属性，第二缺省值为“name”，第三缺省值为“类型名称name”。</param>
        /// <returns></returns>
        public static PropertyInfo GetSelectListTextPropertyInfo(Type classType, string textPropertyName = null)
        {
            PropertyInfo property = null;
            if (textPropertyName != null)
            {
                property = ReflectionHelper.GetProperty(classType, textPropertyName);
                if (property == null) throw new Exception("类" + classType.Name + "没有名为" + textPropertyName + "的属性");
            }
            else
            {
                property = classType.GetProperties().FirstOrDefault(t => t.GetCustomAttribute(typeof(SelectListTextPropertyAttribute), false) != null);
                if (property == null)
                {
                    property = ReflectionHelper.GetProperty(classType, "name");
                    if (property == null)
                    {
                        property = ReflectionHelper.GetProperty(classType, classType.Name + "name");
                        if (property == null) throw new Exception("类" + classType.Name + "没有可以作为下拉列表框选项显示文字的属性");
                    }
                }
            }
            return property;
        }

        /// <summary>
        /// 获取下拉列表框的选项集合。
        /// <para>  返回System.Web.Mvc.SelectListItem集合。</para>
        /// </summary>
        /// <typeparam name="TEntity">下拉列表框数据源实体类型。</typeparam>
        /// <param name="valueProperty">绑定至下拉列表框的值属性信息。</param>
        /// <param name="textProperty">绑定至下拉列表框的显示属性信息。</param>
        /// <param name="selectedValue">当前选中项的值。缺省值为字符串"0"。</param>
        /// <param name="filter">下拉列表框数据源的谓词筛选值，如：t=>t.Property==value。缺省值为null</param>
        /// <param name="orderBy">下拉列表框数据源的排序依据和方式，如：p=>p.OrderBy(t=>t.Property1).ThenBy(t=>t.Property2)。缺省值为null</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectListItems<TEntity>
            (
            PropertyInfo valueProperty,
            PropertyInfo textProperty,
            string selectedValue = "0",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            ) where TEntity : class
        {
            DbContext db = (DbContext)UnityConfig.Container.Resolve<IDbContext>();
            DbSet<TEntity> dbSet = ((DbContext)db).Set<TEntity>();
            IQueryable<TEntity> datas = dbSet;
            if (filter != null) datas = datas.Where(filter);
            if (orderBy != null) datas = orderBy(datas);
            List<SelectListItem> listItems = datas.ToList().Select(t => new SelectListItem { Text = textProperty.GetValue(t, null).ToString(), Value = valueProperty.GetValue(t, null).ToString(), Selected = valueProperty.GetValue(t, null).ToString() == selectedValue ? true : false }).ToList();
            return listItems;
        }

        /// <summary>
        /// 获取枚举类型的下拉列表框选项集合。
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="selectedValue">当前选中项的显示值。缺省值为空字符串：""</param>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumSelectListItems(Type enumType,string selectedValue="")
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            string[] keys = Enum.GetNames(enumType);
            Array values = Enum.GetValues(enumType);
            for (int i = 0; i < keys.Length; i++)
            {
                listItems.Add(new SelectListItem { Text = keys[i], Value = ((int)values.GetValue(i)).ToString(), Selected = (selectedValue != null && selectedValue == keys[i]) });
            }
            return listItems;
        }
    }
}
