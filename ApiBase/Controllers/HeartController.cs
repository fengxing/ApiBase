using System;
using System.Web.Http;

namespace ApiBase.Controllers
{
    public class HeartController : BaseController
    {
        /// <summary>
        /// 心跳服务
        /// </summary>
        /// <returns>当前时间</returns>  
        [AllowAnonymous]
        [Route("Heart/Heart")]
        [HttpPost]
        public string Heart()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
