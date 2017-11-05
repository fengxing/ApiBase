using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// 患者精调列表请求
    /// </summary>
    public class GetOrderAdjustsRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public Guid OrderID { get; set; }
    }
}