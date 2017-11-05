using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace ApiBase.Core
{
    public class SameRequestCacheHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                return new Task<HttpResponseMessage>(() => new HttpResponseMessage() { StatusCode = HttpStatusCode.OK });
            }
            var uid = request.GetHeader("uid");
            if (string.IsNullOrWhiteSpace(uid) == false && request.GetBoolHeader("retry") == false)
            {
                var method = request.GetHeader("method");
                var hashCode = (uid + method + request.Content.ReadAsStringAsync().Result).GetHashCode();
                var cache = MemoryCacher.Get(hashCode.ToString());
                if (cache == null)
                {
                    request.Properties.Add("uid", uid);
                    request.Properties.Add("hashcode", hashCode);
                    return base.SendAsync(request, cancellationToken);
                }
                else
                {
                    if (cache.Value == null)
                    {
                        var httpCacheOK = new HttpResponseMessage() { StatusCode = (HttpStatusCode)888 };
                        httpCacheOK.Headers.Add("ApiCache", "SameRequest");
                        return Task.FromResult(httpCacheOK);
                    }
                    else
                    {
                        var httpCachOK = new HttpResponseMessage()
                        {
                            StatusCode = (HttpStatusCode)888,
                        };
                        if (cache.Value is string || cache.Value.GetType().IsValueType)
                        {
                            httpCachOK.Content = new StringContent(cache.Value.ToString().Trim(new char[] { '"' }));
                            httpCachOK.Headers.Add("ApiCache", "SameRequest");
                            return Task.FromResult(httpCachOK);
                        }
                        else
                        {
                            httpCachOK.Content = new ObjectContent(cache.Value.GetType(), cache.Value, new JsonMediaTypeFormatter());
                            httpCachOK.Headers.Add("ApiCache", "SameRequest");
                            return Task.FromResult(httpCachOK);
                        }
                    }
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
