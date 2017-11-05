using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class GetOrderAdjustsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<OrderAdjustFullItem> OrderAdjustFullItems { get; set; }
    }
}