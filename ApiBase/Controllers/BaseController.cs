using System;
using System.Configuration;
using System.Web.Http;

namespace ApiBase.Controllers
{
    public class BaseController : ApiController
    {
        public static string AppSetting(string name, string def = "")
        {
            var appsetting = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrWhiteSpace(appsetting))
            {
                if (!string.IsNullOrWhiteSpace(def))
                {
                    return def.Trim();
                }
                throw new AppSettingException(name);
            }
            else
            {
                return appsetting.Trim();
            }
        }
    }
}
