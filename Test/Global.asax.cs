using System.Web.Http;
using ApiBase;
using ApiBase.Core;

namespace Test
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HttpGetDelegatingHandler());
#endif
            GlobalConfiguration.Configure(WebApiConfig.RegisterAll);
            WebApiConfig.RemoveFilter(GlobalConfiguration.Configuration, WebApiConfig.Filter.AuthFilterAttribute);
        }
    }
}
