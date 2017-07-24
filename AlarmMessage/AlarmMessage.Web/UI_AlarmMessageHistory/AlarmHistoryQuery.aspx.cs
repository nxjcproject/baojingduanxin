using baojinglishijiluchaxun.Service.Baojinglishijiluchaxun;
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
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf","zc_nxjc_lpsc_lpsf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
                string m_NodeId = Request.QueryString["PageId"] != null ? Request.QueryString["PageId"] : "";
                //string m_NodeId = "84AC953D-4EFE-4C2A-A287-B83D57B21020";
                Hiddenfield_PageId.Value = baojinglishijiluchaxun.Service.Baojinglishijiluchaxun.AlarmHistorySelect1.GetPageIdByNodeId(m_NodeId);
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");  //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "AlarmHistoryQuery.aspx";   //向web用户控件传递当前调用的页面名称
                this.OrganisationTree_ProductionLine.LeveDepth = 5;
            }
        }
        [WebMethod]
        public static string SystemAlarmTypeList(string alarmGroup)
        {
            DataTable table = baojinglishijiluchaxun.Service.Baojinglishijiluchaxun.AlarmHistorySelect1.GetSystemAlarmTypeListTable(alarmGroup);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetReportData(string organizationId, string startTime, string endTime, string type, string alarmGroup)
        {
            DataTable table = baojinglishijiluchaxun.Service.Baojinglishijiluchaxun.AlarmHistorySelect1.GetMainMachineListTable(organizationId, startTime, endTime, type, alarmGroup);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetRealTimeAlarm(string organizationId, string alarmGroup)
        {
            DataTable table = baojinglishijiluchaxun.Service.Baojinglishijiluchaxun.AlarmHistorySelect1.GetRealTimeAlarm(organizationId, alarmGroup);
            if (table != null)
            {
                string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
                return json;
            }
            else
            {
                return "{\"rows\":[],\"total\":0}";
            }
        }
    }
}