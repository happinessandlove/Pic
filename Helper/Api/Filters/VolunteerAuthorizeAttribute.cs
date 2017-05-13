using ImageClassification.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Api.Filters
{
    public class VolunteerAuthorizeAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> tokenItem=null;
            actionContext.Request.Headers.TryGetValues("token", out tokenItem);
            //没有token
            if(tokenItem==null)
                throwUnanthorizedException();
            string token = tokenItem.ToList()[0];
            VolunteerToken volunteerToken = VolunteerService.GetToken(token);
            //数据库没有此token
            if (volunteerToken == null)
                throwUnanthorizedException();
            //token过期
            if (volunteerToken.ExpiredTime < DateTime.UtcNow)
                throwUnanthorizedException();

            Volunteer volunteer = VolunteerService.GetVolunteer(volunteerToken.VolunteerId);
            //volunteer是否存在或有效
            if (volunteer == null || volunteer.Status == EnumUserStatus.注销 || volunteer.Status == EnumUserStatus.密码输入错误临时锁定 || volunteer.Status == EnumUserStatus.注册未验证手机)
                throwUnanthorizedException();

            SetPrincipal(new VolunteerPrincipal(volunteer));

            if (!VolunteerService.UpdateTokenActiveTime(volunteer.Id))
                throwUnanthorizedException();
            base.OnActionExecuting(actionContext);
        }

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if(HttpContext.Current!=null)
            {
                HttpContext.Current.User = principal;
            }
        }
        private void throwUnanthorizedException()
        {
            var reponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized
            };
            throw new HttpResponseException(reponse);
        }
    }
}