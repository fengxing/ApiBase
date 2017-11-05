using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiBase.Core
{
    public class AuthFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!DebugHelper.IsDebug())
            {
                if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                {
                    var uid = GetTag(actionContext, "uid");
                    if (string.IsNullOrWhiteSpace(uid))
                    {
                        actionContext.Response = new HttpResponseMessage() { Content = new StringContent("用户信息不存在,请先登陆!"), StatusCode = (HttpStatusCode)500 };
                    }
                }
            }
        }


        public string GetTag(HttpActionContext actionContext, string tagName)
        {
            try
            {
                var tag = "";
                if (actionContext.Request.Headers.Contains(tagName))
                {
                    try
                    {
                        tag = actionContext.Request.Headers.GetValues(tagName).FirstOrDefault();
                        return tag;
                    }
                    catch { }
                }
                return "";
            }
            catch 
            {
                return "";
            }
        }
    }
}