namespace System.Web.Mvc
{
    public class ResultMessage
    {
        public static string Invalid = "数据填写有误，请检查！";
        public static string NotLogin = "您没有登录，请登录后再操作！";
        public static string Success = "操作成功！";
        public static string Failure = "操作失败！";
        public static string Error = "系统错误！";
        public static string NoPermission = "没有操作权限！";
        public static string CreateSuccess = "数据添加成功！";
        public static string CreateFailure = "数据未能正确添加，请检查数据，重新尝试！";
        public static string DeleteSuccess = "数据删除成功！";
        public static string DeleteFailure = "未能删除数据，请刷新页面，重新操作！";
        public static string DeletesSuccess = "指定数据删除成功！";
        public static string DeletesFailure = "未能删除指定数据，请刷新页面，重新操作！";
        public static string EditSuccess = "数据更新成功！";
        public static string EditFailure = "数据未能正确更新，请检查数据，重新尝试！";
        public static string RankUpSuccess = "数据调整排序成功！";
        public static string RankUpFailure = "数据未能正确调整排序，请刷新页面，重新尝试！";
    }
}