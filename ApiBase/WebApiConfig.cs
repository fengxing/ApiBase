using ApiBase.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;

namespace ApiBase
{
    public static class WebApiConfig
    {
        public static void RegisterAttrHandler(HttpConfiguration config)
        {
            config.Filters.Add(new ApiExceptionFilterAttribute());
            config.Filters.Add(new AuthFilterAttribute());
            config.Filters.Add(new SuccessExecutedFilterAttribute());
            config.Services.Replace(typeof(IHttpControllerSelector), new ControllerNotFoundSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector), new ActionNotFoundSelector());
            config.MessageHandlers.Add(new SameRequestCacheHandler());
        }

        public static void RegisterBase(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               name: "Error404",
               routeTemplate: "{*url}",
               defaults: new { controller = "Error", action = "Handle404" }
           );
            ServicePointManager.DefaultConnectionLimit = 1024;
            ServicePointManager.Expect100Continue = false;
        }


        public static void RegisterAll(HttpConfiguration config)
        {
            RegisterBase(config);
            RegisterAttrHandler(config);
        }

        public static void RemoveFilter(HttpConfiguration config, Filter filter)
        {
            IFilter f = null;
            var e = config.Filters.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.Instance.GetType().Name.ToString() == filter.ToString())
                {
                    f = e.Current.Instance;
                }
            }
            if (f != null)
            {
                config.Filters.Remove(f);
            }
        }

        public enum Filter
        {
            ApiExceptionFilterAttribute,
            AuthFilterAttribute,
            SuccessExecutedFilterAttribute
        }
    }
}
