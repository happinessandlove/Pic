/**************************************************
 * by 丁浩
 * 2016-01-30 
 * **************************************************/

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）查询对话框的辅助方法
    /// </summary>
    public static class QueryHelper
    {
        #region 辅助封装类型
        /// <summary>
        /// 查询条件
        /// </summary>
        private enum QueryCondition
        {
            Equal, Contains, StartsWith, EndsWith, LessThan, LessThanOrEqual, GreaterThan, GreaterThanOrEqual, Between
        }
        /// <summary>
        /// 多条件查询间逻辑关系
        /// </summary>
        private enum QueryLogic
        {
            And, Or
        }
        /// <summary>
        /// 查询项封装类
        /// </summary>
        private class QueryItem
        {
            public QueryPropertyType QueryPropertyType { get; set; }
            public string PropertyName { get; set; }//多级名称
            public string DisplayName { get; set; }
            public Type EntityType { get; set; } //最终一级实体类型
            public PropertyInfo ValueProperty { get; set; }
            public PropertyInfo TextProperty { get; set; }

            //构造函数 关联查询属性
            public QueryItem(Type modelType, PropertyInfo propertyInfo, AssociatedQueryAttribute aqa)
            {
                bool hasSpecifiedValue = !(aqa.AssociatedPropertyName == null && aqa.DisplayName == null && aqa.TextPropertyName == null && aqa.ValuePropertyName == null);

                //QueryPropertyType
                this.QueryPropertyType = aqa.QueryPropertyType;

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  aqa:         QueryPropertyType    AssociatedPropertyName     DisplayName         ValuePropertyName          TextPropertyName
                // 无标签              Select                   null                 null                   null                      null
                // 有标签              Select                   null                 指定         Clas.College.University.Id  Clas.College.University.Name
                // 有标签          指定（非Select）  Clas.College.University.Name     指定                   null                      null
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // QueryItem:    QueryPropertyType        EntityType           PropertyName           DisplayName    ValueProperty      TextProperty
                // 无标签             Select                  Clas                 Clas.Id                 获取            Id               Name
                // 有标签             Select               University     Clas.College.University.Id       指定            Id               Name
                // 有标签         指定（非Select）         University     Clas.College.University.Name      指定           null              null
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //有[AssociatedQuery]标识，或者有[AssociatedQuery]标识且不带参数
                if (!hasSpecifiedValue)
                {
                    if (Config.AssociatedQueryAttributeAddedOnForeignKey)
                    {
                        PropertyInfo nvProperty = getNavigationPropertyByForeignKeyName(modelType, propertyInfo.Name);
                        if (nvProperty == null) throw new Exception("查询对话框异常：" + modelType.Name + "类中未找到与" + propertyInfo.Name + "属性相匹配的导航属性");
                        this.EntityType = nvProperty.PropertyType;
                        this.PropertyName = nvProperty.Name;
                    }
                    else
                    {
                        this.EntityType = propertyInfo.PropertyType;
                        PropertyInfo fkProperty = getForeignKeyPropertyByNavigationPropertyTypeName(modelType, this.EntityType.Name);
                        if (fkProperty == null) throw new Exception("查询对话框异常：" + modelType.Name + "类中未找到与" + propertyInfo.Name + "属性相匹配的外键属性");
                        this.PropertyName = propertyInfo.Name;
                    }
                    this.DisplayName = getPropertyDisplayName(propertyInfo);
                    this.ValueProperty = SelectListHelper.GetSelectListValuePropertyInfo(this.EntityType);
                    this.TextProperty = SelectListHelper.GetSelectListTextPropertyInfo(this.EntityType);
                    this.PropertyName += "." + this.ValueProperty.Name;
                }
                //有[AssociatedQuery]标识且带参数 Select
                else if (this.QueryPropertyType == QueryPropertyType.Select)
                {
                    Type type = modelType;
                    this.ValueProperty = ReflectionHelper.GetProperty(ref type, aqa.ValuePropertyName);
                    type = modelType;
                    this.TextProperty = ReflectionHelper.GetProperty(ref type, aqa.TextPropertyName);
                    this.EntityType = type;
                    this.PropertyName = aqa.ValuePropertyName;
                    this.DisplayName = aqa.DisplayName;
                }
                //有标签 非Select
                else
                {
                    Type type = modelType;
                    ReflectionHelper.GetProperty(ref type, aqa.AssociatedPropertyName);
                    this.EntityType = type;
                    this.PropertyName = aqa.AssociatedPropertyName;
                    this.DisplayName = aqa.DisplayName;
                }
            }
            //构造函数 枚举属性、普通属性
            public QueryItem(Type modelType, PropertyInfo propertyInfo)
            {
                this.EntityType = propertyInfo.PropertyType;
                this.PropertyName = propertyInfo.Name;
                if (this.EntityType.IsEnum)
                    this.QueryPropertyType = QueryPropertyType.Enum;
                else if (this.PropertyName.ToLower() == "gender")
                    this.QueryPropertyType = QueryPropertyType.Gender;
                else this.QueryPropertyType = ConvertToQueryPropertyType(this.EntityType);
                this.DisplayName = getPropertyDisplayName(propertyInfo);
            }
        }
        /// <summary>
        /// 封装客户端提交的查询条件
        /// </summary>
        private class QueryItemClient
        {
            public QueryPropertyType QueryPropertyType { get; set; }
            public QueryLogic? QueryLogic { get; set; }
            public string QueryProperty { get; set; }
            public QueryCondition QueryCondition { get; set; }
            public string QueryValue { get; set; }
            public string QueryValue1 { get; set; }
            public string QueryValue2 { get; set; }
        }
        #endregion

        #region 生成查询对话框
        /// <summary>
        /// 生成查询对话框的html代码
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="queryString">json格式的查询字符串</param>
        /// <param name="excludeProperties">排除的属性，多个属性以逗号隔开</param>
        /// <returns></returns>
        public static MvcHtmlString RenderQuery(Type type, string queryString, string excludeProperties)
        {
            try
            {
                List<QueryItem> queryItems = getQueryItems(type, excludeProperties);
                if (queryItems.Count == 0) return new MvcHtmlString("");
                StringBuilder sb = new StringBuilder();
                string dialog = "<div class=\"dialog\" id=\"query-dialog\" style=\"display:none\"><div class=\"dialog-header\"><div class=\"dialog-title\">查询</div></div><div class=\"dialog-body\"><table><thead><tr><th class=\"col-query-logic\">逻 辑</th><th class=\"col-query-property\">查 询 项</th><th class=\"col-query-condition\">关 系</th><th class=\"col-query-value\">查 询 词</th><th class=\"col-query-operation\"></th></tr></thead><tbody id=\"query-items\">{0}</tbody><tfoot><tr><td colspan=\"5\"><a href=\"/\" class=\"add-query-item\">添加查询条件</a></td></tr></tfoot></table></div><div class=\"dialog-button\"><a id=\"submit-query\" class=\"button\" href=\"/\"><span>查 询</span></a><a id=\"submit-cancel\" class=\"button layer-close\" href=\"/\"><span>取 消</span></a></div><div class=\"dialog-footer\"></div></div>";
                StringBuilder dialogBody = new StringBuilder();

                #region 模板
                StringBuilder sbTemplate = new StringBuilder();
                string templateTable = "<table id=\"query-template\" style=\"display: none\">{0}</table>";
                string templateRow = "<tr class=\"query-item\"><td class=\"col-query-logic\">{0}</td><td class=\"col-query-property\">{1}</td><td class=\"col-query-condition\">{2}</td><td class=\"col-query-value\">{3}</td><td class=\"col-query-operation\"><a href=\"/\" class=\"remove-query-item\" title=\"移除查询条件\"></a></td></tr>";
                string templateLogic = "<select name=\"queryLogic\" class=\"query-logic\"><option value=\"And\">并且</option><option value=\"Or\">或者</option></select>";
                string templateProperty = "<select name=\"queryProperty\" class=\"query-property\">{0}</select>";
                string templateSelectList = "<div id=\"query-template-select\" style=\"display:none\">{0}</div>";
                StringBuilder sbTemplateSelectList = new StringBuilder();
                StringBuilder propertyString = new StringBuilder();
                foreach (var p in queryItems)
                {
                    propertyString.AppendFormat("<option value=\"{0}\" type=\"{1}\">{2}</option>", p.PropertyName, p.QueryPropertyType, p.DisplayName);
                }
                templateProperty = string.Format(templateProperty, propertyString.ToString());
                string templateCondition = "<select propertytype=\"{0}\" name=\"queryCondition\" class=\"query-condition\">{1}</select>";
                string templateStringCondition = "<option value=\"Contains\">包含</option><option value=\"Equal\">等于</option><option value=\"StartsWith\">以此开头</option><option value=\"EndsWith\">以此结尾</option>";
                string templateNumericalCondition = "<option value=\"Equal\">等于</option><option value=\"LessThan\">小于</option><option value=\"LessThanOrEqual\">小于等于</option><option value=\"GreaterThan\">大于</option><option value=\"GreaterThanOrEqual\">大于等于</option><option value=\"Between\">介于</option>";
                string templateDateTimeCondition = "<option value=\"GreaterThanOrEqual\">起始于</option><option value=\"LessThanOrEqual\">终止于</option><option value=\"Between\">介于</option>";
                string templateBooleanCondition = "<option value=\"Equal\">等于</option>";
                string templateGenderCondition = "<option value=\"Equal\">等于</option>";
                string templateSelectCondition = "<option value=\"Equal\">等于</option>";
                string conditionString = "";
                switch (queryItems[0].QueryPropertyType)
                {
                    case QueryPropertyType.String: conditionString = string.Format(templateCondition, queryItems[0].QueryPropertyType, templateStringCondition); break;
                    case QueryPropertyType.Numerical: conditionString = string.Format(templateCondition, queryItems[0].QueryPropertyType, templateNumericalCondition); break;
                    case QueryPropertyType.DateTime: conditionString = string.Format(templateCondition, queryItems[0].QueryPropertyType, templateDateTimeCondition); break;
                    case QueryPropertyType.Boolean: conditionString = string.Format(templateCondition, queryItems[0].QueryPropertyType, templateBooleanCondition); break;
                    case QueryPropertyType.Gender: conditionString = string.Format(templateCondition, queryItems[0].QueryPropertyType, templateGenderCondition); break;
                    case QueryPropertyType.Select: conditionString = string.Format(templateCondition, queryItems[0].QueryPropertyType, templateSelectCondition); break;
                }
                string templateOneValue = "<input type=\"text\" name=\"queryValue\" class=\"query-value\" value=\"\" />";
                string templateTwoValue = "<input type=\"text\" name=\"queryValue1\" class=\"query-value1\" value=\"\" /> - <input type=\"text\" name=\"queryValue2\" class=\"query-value2\" value=\"\" />";
                string templateBooleanValue = "<select name=\"queryValue\" class=\"query-value\"><option value=\"True\">是</option><option value=\"False\">否</option></select>";
                string templateGenderValue = "<select name=\"queryValue\" class=\"query-value\"><option value=\"男\">男</option><option value=\"女\">女</option></select>";
                string templateSelectValue = "<select name=\"queryValue\" propertyname=\"{0}\" class=\"query-value\">{1}</select>";
                string valueString = "";
                //生成所有select形式的下拉列表框模板
                List<QueryItem> selectItems = queryItems.Where(t => t.QueryPropertyType == QueryPropertyType.Select|| t.QueryPropertyType == QueryPropertyType.Enum).ToList();
                for (int i = 0; i < selectItems.Count(); i++)
                {
                    string options = "";
                    QueryItem item = selectItems[i];
                    options = getSelectOptions(item);
                    sbTemplateSelectList.AppendFormat(templateSelectValue, item.PropertyName, options);
                    if (i == 0) valueString = string.Format(templateSelectValue, item.PropertyName, options);
                }
                if (queryItems[0].QueryPropertyType != QueryPropertyType.Select) valueString = templateOneValue;
                sbTemplate.AppendFormat(templateTable, string.Format(templateRow, templateLogic, templateProperty, conditionString, valueString)).Append(string.Format(templateSelectList, sbTemplateSelectList));
                #endregion

                #region 生成查询项
                if (string.IsNullOrWhiteSpace(queryString))
                {
                    sb.AppendFormat(dialog, string.Format(templateRow, "", templateProperty, conditionString, valueString));
                }
                else
                {
                    List<QueryItemClient> queryItemClient = JsonConvert.DeserializeObject<List<QueryItemClient>>(queryString);
                    string logicStr, propertyStr, conditionStr = "", valueStr = "";
                    for (int i = 0; i < queryItemClient.Count; i++)
                    {
                        var item = queryItemClient[i];
                        logicStr = (i == 0 ? "" : templateLogic.Replace(string.Format("value=\"{0}\"", item.QueryLogic.ToString()), string.Format("value=\"{0}\"", item.QueryLogic.ToString()) + " selected=\"selected\""));
                        propertyStr = templateProperty.Replace(string.Format("value=\"{0}\"", item.QueryProperty), string.Format("value=\"{0}\"", item.QueryProperty) + " selected=\"selected\"");
                        switch (item.QueryPropertyType)
                        {
                            case QueryPropertyType.String:
                                conditionStr = string.Format(templateCondition, item.QueryPropertyType.ToString(), templateStringCondition);
                                valueStr = templateOneValue.Replace("value=\"\"", string.Format("value=\"{0}\"", item.QueryValue));
                                break;
                            case QueryPropertyType.Numerical:
                                conditionStr = string.Format(templateCondition, item.QueryPropertyType.ToString(), templateNumericalCondition);
                                if (item.QueryCondition != QueryCondition.Between)
                                    valueStr = templateOneValue.Replace("value=\"\"", string.Format("value=\"{0}\"", item.QueryValue));
                                else
                                    valueStr = templateTwoValue.Replace("class=\"query-value1\" value=\"\"", string.Format("class=\"query-value1\" value=\"{0}\"", item.QueryValue1)).Replace("class=\"query-value2\" value=\"\"", string.Format("class=\"query-value2\" value=\"{0}\"", item.QueryValue2));
                                break;
                            case QueryPropertyType.DateTime:
                                conditionStr = string.Format(templateCondition, item.QueryPropertyType.ToString(), templateDateTimeCondition);
                                if (item.QueryCondition != QueryCondition.Between)
                                    valueStr = templateOneValue.Replace("value=\"\"", string.Format("value=\"{0}\"", item.QueryValue));
                                else
                                    valueStr = templateTwoValue.Replace("class=\"query-value1\" value=\"\"", string.Format("class=\"query-value1\" value=\"{0}\"", item.QueryValue1)).Replace("class=\"query-value2\" value=\"\"", string.Format("class=\"query-value2\" value=\"{0}\"", item.QueryValue2));
                                break;
                            case QueryPropertyType.Boolean:
                                conditionStr = string.Format(templateCondition, item.QueryPropertyType.ToString(), templateBooleanCondition);
                                valueStr = templateBooleanValue.Replace(string.Format("value=\"{0}\"", item.QueryValue), string.Format("value=\"{0}\"", item.QueryValue) + " selected=\"selected\"");
                                break;
                            case QueryPropertyType.Gender:
                                conditionStr = string.Format(templateCondition, item.QueryPropertyType.ToString(), templateGenderCondition);
                                valueStr = templateGenderValue.Replace(string.Format("value=\"{0}\"", item.QueryValue), string.Format("value=\"{0}\"", item.QueryValue) + " selected=\"selected\"");
                                break;
                            case QueryPropertyType.Select:
                            case QueryPropertyType.Enum:
                                conditionStr = string.Format(templateCondition, item.QueryPropertyType.ToString(), templateSelectCondition);
                                valueStr = string.Format(templateSelectValue, item.QueryProperty, getSelectOptions(queryItems.First(t => t.PropertyName == item.QueryProperty), item.QueryValue));
                                break;
                        }
                        conditionStr = conditionStr.Replace(string.Format("value=\"{0}\"", item.QueryCondition.ToString()), string.Format("value=\"{0}\"", item.QueryCondition.ToString()) + " selected=\"selected\"");
                        dialogBody.AppendFormat(templateRow, logicStr, propertyStr, conditionStr, valueStr);
                    }
                    sb.AppendFormat(dialog, dialogBody);
                }
                #endregion
                return new MvcHtmlString(sb.Append(sbTemplate).ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="datas">IQueryable集合的数据源</param>
        /// <param name="queryString">json格式的查询字符串</param>
        /// <returns></returns>
        public static IQueryable<TEntity> ExecuteQuery<TEntity>(IQueryable<TEntity> datas, string queryString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(queryString)) return datas;
                List<QueryItemClient> queryItemClientList = JsonConvert.DeserializeObject<List<QueryItemClient>>(queryString);

                Type type = typeof(TEntity);
                ParameterExpression param = Expression.Parameter(type, "q");
                Expression<Func<TEntity, bool>> filter;
                Expression left = null, right = null, right1 = null, right2 = null, thisExpression = null, queryExpression = null;
                foreach (var qi in queryItemClientList)
                {
                    double d = 0, d1 = 0, d2 = 0;
                    if (qi.QueryCondition != QueryCondition.Between && string.IsNullOrWhiteSpace(qi.QueryValue)) continue;
                    if (qi.QueryCondition == QueryCondition.Between && (string.IsNullOrWhiteSpace(qi.QueryValue1) || string.IsNullOrWhiteSpace(qi.QueryValue2))) continue;
                    if (qi.QueryPropertyType == QueryPropertyType.Numerical && qi.QueryCondition != QueryCondition.Between && !double.TryParse(qi.QueryValue, out d)) continue;
                    if (qi.QueryPropertyType == QueryPropertyType.Numerical && qi.QueryCondition == QueryCondition.Between && (!double.TryParse(qi.QueryValue1, out d1) || !double.TryParse(qi.QueryValue2, out d2))) continue;
                    if (qi.QueryPropertyType == QueryPropertyType.DateTime && qi.QueryCondition == QueryCondition.Between && (string.IsNullOrWhiteSpace(qi.QueryValue1) || string.IsNullOrWhiteSpace(qi.QueryValue2))) continue;
                    if (qi.QueryPropertyType == QueryPropertyType.DateTime && qi.QueryCondition == QueryCondition.Equal && (string.IsNullOrWhiteSpace(qi.QueryValue))) continue;

                    Type propertyType;
                    left = parsePropertyName(type, qi.QueryProperty, param, out propertyType);
                    switch (qi.QueryPropertyType)
                    {
                        case QueryPropertyType.Gender:
                            right = Expression.Constant(qi.QueryValue);
                            thisExpression = Expression.Equal(left, right);
                            break;
                        case QueryPropertyType.String:
                            right = Expression.Constant(qi.QueryValue);
                            switch (qi.QueryCondition)
                            {
                                case QueryCondition.Equal: thisExpression = Expression.Equal(left, right); break;
                                case QueryCondition.Contains: thisExpression = Expression.Call(left, typeof(string).GetMethod("Contains"), right); break;
                                case QueryCondition.StartsWith: thisExpression = Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), right); break;
                                case QueryCondition.EndsWith: thisExpression = Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), right); break;
                            }
                            break;
                        case QueryPropertyType.Numerical:
                            right = Expression.Constant(d);
                            right1 = Expression.Constant(d1);
                            right2 = Expression.Constant(d2);
                            switch (qi.QueryCondition)
                            {
                                case QueryCondition.Equal: thisExpression = Expression.Equal(left, changeValue(right, propertyType)); break;
                                case QueryCondition.LessThan: thisExpression = Expression.LessThan(left, changeValue(right, propertyType)); break;
                                case QueryCondition.LessThanOrEqual: thisExpression = Expression.LessThanOrEqual(left, changeValue(right, propertyType)); break;
                                case QueryCondition.GreaterThan: thisExpression = Expression.GreaterThan(left, changeValue(right, propertyType)); break;
                                case QueryCondition.GreaterThanOrEqual: thisExpression = Expression.GreaterThanOrEqual(left, changeValue(right, propertyType)); break;
                                case QueryCondition.Between: thisExpression = Expression.And(Expression.GreaterThanOrEqual(left, changeValue(right1, propertyType)), Expression.LessThanOrEqual(left, changeValue(right2, propertyType))); break;
                            }
                            break;
                        case QueryPropertyType.DateTime:
                            switch (qi.QueryCondition)
                            {
                                case QueryCondition.LessThanOrEqual:
                                    thisExpression = Expression.LessThanOrEqual(left, changeValue(Expression.Constant(DateTime.Parse(qi.QueryValue + " 23:59:59")), propertyType));
                                    break;
                                case QueryCondition.GreaterThanOrEqual:
                                    thisExpression = Expression.GreaterThanOrEqual(left, changeValue(Expression.Constant(DateTime.Parse(qi.QueryValue)), propertyType));
                                    break;
                                case QueryCondition.Between:
                                    thisExpression = Expression.And(Expression.GreaterThanOrEqual(left, changeValue(Expression.Constant(DateTime.Parse(qi.QueryValue1)), propertyType)), Expression.LessThanOrEqual(left, changeValue(Expression.Constant(DateTime.Parse(qi.QueryValue2 + " 23:59:59")), propertyType)));
                                    break;
                                case QueryCondition.Equal:
                                    right = Expression.Constant(DateTime.Parse(qi.QueryValue));
                                    thisExpression = Expression.Equal(left, changeValue(right, propertyType));
                                    break;
                            }
                            break;
                        case QueryPropertyType.Boolean:
                            thisExpression = Expression.Equal(left, changeValue(Expression.Constant(bool.Parse(qi.QueryValue)), propertyType));
                            break;
                        case QueryPropertyType.Select:
                            dynamic keyValue;
                            if (propertyType.Name == "String") keyValue = qi.QueryValue;
                            else keyValue = propertyType.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { qi.QueryValue });
                            thisExpression = Expression.Equal(left, changeValue(Expression.Constant(keyValue), propertyType));
                            break;
                    }
                    if (queryExpression != null)
                    {
                        switch (qi.QueryLogic.Value)
                        {
                            case QueryLogic.And: queryExpression = Expression.And(queryExpression, thisExpression); break;
                            case QueryLogic.Or: queryExpression = Expression.Or(queryExpression, thisExpression); break;
                        }
                    }
                    else
                        queryExpression = thisExpression;
                    thisExpression = null;
                }
                if (queryExpression != null)
                {
                    filter = (Expression<Func<TEntity, bool>>)Expression.Lambda<Func<TEntity, bool>>(queryExpression, param);
                    datas = datas.Where(filter);
                }
                return datas;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region 辅助方法
        private static List<QueryItem> getQueryItems(Type type, string excludeProperties)
        {
            List<QueryItem> queryItems = new List<QueryItem>();
            string[] exProperties = string.IsNullOrEmpty(excludeProperties) ? null : excludeProperties.Split(',');
            foreach (var p in type.GetProperties())
            {
                //排除含有不参与查询标记的属性
                if (p.GetCustomAttribute(typeof(NonQueryAttribute), false) != null) continue;
                //排除可识别的主键属性
                if (p.Name.ToLower() == (type.Name + "id").ToLower() || p.Name.ToLower() == "id") continue;
                //排除集合属性
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) continue;
                //排除指定的属性
                if (exProperties != null && exProperties.FirstOrDefault(s => s.ToLower() == p.Name.ToLower()) != null) continue;

                bool isForeignKeyProperty = getNavigationPropertyByForeignKeyName(type, p.Name) != null;
                bool isNavigationProperty = getForeignKeyPropertyByNavigationPropertyTypeName(type, p.PropertyType.Name) != null;
                //排除可识别的导航属性 关联查询标记加在外键属性上
                if (Config.AssociatedQueryAttributeAddedOnForeignKey && isNavigationProperty) continue;
                //排除可识别的外键属性 关联查询标记加在导航属性上
                if (!Config.AssociatedQueryAttributeAddedOnForeignKey && isForeignKeyProperty) continue;
                //关联查询属性 带AssociatedQueryAttribute的外键属性或导航属性 如：ClasId、Clas
                object[] associatedQueryAttributes = p.GetCustomAttributes(typeof(AssociatedQueryAttribute), false);
                if (associatedQueryAttributes.Length > 0)
                {
                    foreach (AssociatedQueryAttribute q in associatedQueryAttributes)
                    {
                        queryItems.Add(new QueryItem(type, p, q));
                    }
                }
                //关联查询属性 不带AssociatedQueryAttribute的外键属性或导航属性 如：ClasId、Clas
                else if (isForeignKeyProperty || isNavigationProperty)
                {
                    //不带标记转换为带无参标记
                    queryItems.Add(new QueryItem(type, p, new AssociatedQueryAttribute()));
                }
                //普通属性
                else
                {
                    queryItems.Add(new QueryItem(type, p));
                }
            }
            return queryItems;
        }

        private static PropertyInfo getForeignKeyPropertyByNavigationPropertyTypeName(Type modelType, string navigationPropertyTypeName)
        {
            return ReflectionHelper.GetProperty(modelType, navigationPropertyTypeName + "id");
        }

        private static PropertyInfo getNavigationPropertyByForeignKeyName(Type modelType, string foreignKeyName)
        {
            if (!foreignKeyName.ToLower().EndsWith("id")) return null;
            return ReflectionHelper.GetProperty(modelType, foreignKeyName.ToLower().Replace("id", ""));
        }

        private static Type getNavigationPropertyTypeByForeignKeyName(Type modelType, string foreignKeyName)
        {
            PropertyInfo property = getNavigationPropertyByForeignKeyName(modelType, foreignKeyName);
            if (property != null) return property.PropertyType;
            else return null;
        }

        private static string getPropertyDisplayName(PropertyInfo property)
        {
            Attribute attribute = property.GetCustomAttribute(typeof(DisplayAttribute), false);
            if (attribute != null) return ((DisplayAttribute)attribute).Name;
            attribute = property.GetCustomAttribute(typeof(DisplayNameAttribute), false);
            if (attribute != null) return ((DisplayNameAttribute)attribute).DisplayName;
            return property.Name;
        }

        private static QueryPropertyType ConvertToQueryPropertyType(Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                NullableConverter nullableConverter = new NullableConverter(propertyType);
                propertyType = nullableConverter.UnderlyingType;
            }
            string typeName = propertyType.Name;
            QueryPropertyType type;
            switch (typeName)
            {
                case "String": type = QueryPropertyType.String; break;
                case "DateTime": type = QueryPropertyType.DateTime; break;
                case "Int32":
                case "Int16":
                case "Int64":
                case "Single":
                case "Decimal":
                case "Double": type = QueryPropertyType.Numerical; break;
                case "Boolean": type = QueryPropertyType.Boolean; break;
                default: type = QueryPropertyType.String; break;
            }
            return type;
        }

        private static string getSelectOptions(QueryItem queryItem, string selectedValue = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (queryItem.QueryPropertyType == QueryPropertyType.Enum)
            {
                listItems = SelectListHelper.GetEnumSelectListItems(queryItem.EntityType, selectedValue);
            }
            else
            {
                MethodInfo method = typeof(SelectListHelper).GetMethod("GetSelectListItems");
                method = method.MakeGenericMethod(queryItem.EntityType);
                listItems = method.Invoke(null, new object[] { queryItem.ValueProperty, queryItem.TextProperty, selectedValue, null, null }) as List<SelectListItem>;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in listItems)
            {
                string s = string.Format("<option value=\"{0}\"{1}>{2}</option>", item.Value, item.Selected ? "selected=\"selected\"" : "", item.Text);
                sb.Append(s);
            }
            return sb.ToString();
        }

        private static Expression changeValue(Expression expression, Type convertType)
        {
            if (expression.Type != convertType)
                return Expression.Convert(expression, convertType);
            else return expression;
        }

        private static Expression parsePropertyName(Type objectType, string propertyName, ParameterExpression param, out Type propertyType)
        {
            string[] propertys = propertyName.Split('.');
            if (propertys.Length == 1)
            {
                propertyType = ReflectionHelper.GetProperty(objectType, propertyName).PropertyType;
                return Expression.Property(param, propertyName);
            }
            else
            {
                Expression propertyAccess = param;
                Type typeOfProp = objectType;
                for (int i = 0; i < propertys.Length; i++)
                {
                    PropertyInfo property = ReflectionHelper.GetProperty(typeOfProp, propertys[i]);
                    if (property == null)
                    {
                        propertyType = null;
                        return null;
                    }
                    typeOfProp = property.PropertyType;
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
                propertyType = typeOfProp;
                return propertyAccess;
            }
        }
        #endregion
    }
}
