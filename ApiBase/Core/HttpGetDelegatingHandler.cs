using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiBase.Core
{
    public class HttpGetDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var api = request.GetConfiguration().Services.GetApiExplorer().ApiDescriptions.FirstOrDefault(r => r.HttpMethod.Method == "GET" && r.RelativePath!= "Error/Handle404");
            if (api != null)
            {
                var context = request.GetRequestContext();
                var url = request.RequestUri.ToString();
                if (!url.Contains(api.RelativePath))
                {
                    var redirect = new HttpResponseMessage(HttpStatusCode.Redirect);
                    redirect.Headers.Location = new Uri(url + api.RelativePath);
                    return Task.FromResult(redirect);
                }
                request.SetRequestContext(context);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
