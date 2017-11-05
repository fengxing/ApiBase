using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ApiBase.Core
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var ex = actionExecutedContext.Exception;
            if (ex != null)
            {
                if (ex is HttpResponseException)
                {
                    var he = ex as HttpResponseException;
                    actionExecutedContext.Response = new HttpResponseMessage()
                    {
                        StatusCode = he.Response.StatusCode,
                        Content = new StringContent(he.Response.ReasonPhrase)
                    };
                    return;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(actionExecutedContext.Exception.Message) &&
                        actionExecutedContext.Exception.Message.Contains("|"))
                    {
                        var arr = actionExecutedContext.Exception.Message.Split('|');
                        var code = 999;
                        int.TryParse(arr[arr.Length - 2], out code);
                        var error = "";
                        if (arr.Length >= 2)
                        {
                            error = arr[arr.Length - 1];
                        }
                        actionExecutedContext.Response = new HttpResponseMessage()
                        {
                            StatusCode = (HttpStatusCode)code,
                            Content = new StringContent(error)
                        };
                    }
                    else
                    {
                        actionExecutedContext.Response = new HttpResponseMessage()
                        {
                            StatusCode = (HttpStatusCode)999,
                            Content = new StringContent(ex.Message)
                        };
                    }
                }
            }
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    StatusCode = (HttpStatusCode)998,
                    Content = new StringContent("UnCatchException")
                };
            }
        }
    }
}