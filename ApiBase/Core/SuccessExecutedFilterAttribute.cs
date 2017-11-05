using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace ApiBase.Core
{
    public class SuccessExecutedFilterAttribute : ActionFilterAttribute
    {
        private static int cacheSeconds = ConfigurationManager.AppSettings["CacheSecond"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["CacheSecond"]) : 60;

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null)
            {
                var hashCodeKey = actionExecutedContext.Request.GetKey("hashcode");
                var objContent = actionExecutedContext.Response.Content;
                if (objContent != null)
                {
                    if (objContent.GetType().Name == typeof(StringContent).Name)
                    {
                        actionExecutedContext.Response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = objContent
                        };
                        MemoryCacher.Set(hashCodeKey, objContent.ToString(), cacheSeconds);
                    }
                    else
                    {
                        var value = ((ObjectContent)objContent).Value;
                        if (value is string || value is ValueType)
                        {
                            actionExecutedContext.Response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent(value.ToString())
                            };
                            MemoryCacher.Set(hashCodeKey, value.ToString(), cacheSeconds);
                        }
                        else
                        {
                            MemoryCacher.Set(hashCodeKey, value, cacheSeconds);
                        }
                    }
                }
                else
                {
                    MemoryCacher.Set(hashCodeKey, "", cacheSeconds);
                    actionExecutedContext.Response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(""),
                    };
                }
            }
        }
    }
}
