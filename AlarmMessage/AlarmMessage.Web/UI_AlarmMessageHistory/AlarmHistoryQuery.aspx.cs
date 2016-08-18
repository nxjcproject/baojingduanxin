﻿using baojinglishijiluchaxun.Service.Baojinglishijiluchaxun;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlarmMessage.Web.UI_AlarmMessageHistory
{
    public partial class AlarmHistoryQuery : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");  //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "AlarmHistoryQuery.aspx";   //向web用户控件传递当前调用的页面名称
                this.OrganisationTree_ProductionLine.LeveDepth = 5;
            }
        }

        [WebMethod]
        public static string GetReportData(string organizationId, string startTime, string endTime,string type)
        {
            DataTable table = baojinglishijiluchaxun.Service.Baojinglishijiluchaxun.AlarmHistorySelect1.GetMainMachineListTable(organizationId, startTime, endTime, type);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetReportDataWithoutType(string organizationId, string startTime, string endTime) 
        {
            DataTable table = baojinglishijiluchaxun.Service.Baojinglishijiluchaxun.AlarmHistorySelect1.GetMainMachineListTable(organizationId, startTime, endTime);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        
    }
}