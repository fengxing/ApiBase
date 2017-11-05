using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ApiBase.Core
{
    public static class HttpRequestMessageHelper
    {
        public static string GetKey(this HttpRequestMessage request,string key)
        {
            try
            {
                object hashCode;
                if (request.Properties.TryGetValue(key, out hashCode))
                {
                    if (hashCode != null)
                    {
                        return hashCode.ToString();
                    }
                }
            }
            catch { }
            return "";
        }

        public static bool GetBoolHeader(this HttpRequestMessage request, string tag)
        {
            bool ret = false;
            IEnumerable<string> retrys = null;
            if (request.Headers.TryGetValues(tag, out retrys))
            {
                var retry = retrys.First();
                if (bool.TryParse(retry, out ret))
                {
                    return ret;
                }
            }
            return false;
        }


        public static string GetHeader(this HttpRequestMessage request, string tag)
        {
            IEnumerable<string> values = null;
            if (request.Headers.TryGetValues(tag, out values))
            {
                return values.First();
            }
            return "";
        }
    }
}
