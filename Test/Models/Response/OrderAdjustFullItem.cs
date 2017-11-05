using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderAdjustFullItem
    {
        /// <summary>
        /// 精调编号
        /// </summary>
        public Guid AdjustID { get; set; }

        /// <summary>
        /// 计划编号
        /// </summary>
        public Guid PlanID { get; set; }

        /// <summary>
        /// 检查资料编号
        /// </summary>
        public Guid CheckUpID { get; set; }

        /// <summary>
        /// 检查资料名称
        /// </summary>
        public string CheckUpName { get; set; }

        /// <summary>
        /// 检查资料描述
        /// </summary>
        public string CheckUpDescription { get; set; }

        /// <summary>
        /// 精调序列码
        /// </summary>
        public string AdjustSN { get; set; }

        /// <summary>
        /// 精调状态
        /// <value>1、开始精调</value>
        /// </summary>
        public int AdjustStatus { get; set; }

        /// <summary>
        /// 精调需求
        /// </summary>
        public string AdjustRequire { get; set; }

        /// <summary>
        /// 精调动画状态
        /// <value>1.动画待制作</value>
        /// <value>2.动画制作中</value>
        /// <value>3.动画已确认</value>
        /// </summary>
        public int AnimStatus { get; set; }

        /// <summary>
        /// 精调金额
        /// </summary>
        public decimal AdjustMoney { get; set; }

        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool FinanceStatus { get; set; }

        /// <summary>
        /// 是否已开票
        /// </summary>
        public bool InvoiceStatus { get; set; }

        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime? AdjustEndDate { get; set; }

        /// <summary>
        /// 终止备注
        /// </summary>
        public string AdjustEndRemark { get; set; }
    }
}