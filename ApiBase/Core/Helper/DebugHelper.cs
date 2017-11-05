using System;
using System.Configuration;
using System.Linq;

namespace ApiBase
{
    public class DebugHelper
    {
        public static bool IsDebug()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IsDEBUG"))
            {
                if (AppSetting("IsDEBUG", "false") == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

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