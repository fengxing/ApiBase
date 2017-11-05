using System;
using System.Web.Http;
using Test.Models;
using Test.Models.Response;

namespace Test.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// 获取订单的精调信息
        /// HttpGet特性用来调试(无需输入路由)
        /// 配合HttpGetDelegatingHandler特性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Adjust/GetOrderAdjusts")]
        [HttpPost]
        [HttpGet]
        [DataQuery]
        public GetOrderAdjustsResponse GetOrderAdjustsRequest(GetOrderAdjustsRequest request)
        {
            return new Models.Response.GetOrderAdjustsResponse();
        }
    }
}
