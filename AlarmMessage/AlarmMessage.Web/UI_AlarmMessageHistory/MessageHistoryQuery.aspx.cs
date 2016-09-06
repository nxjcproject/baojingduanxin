using AlarmMessage.Service.AlarmMessageSetting;
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
    public partial class MessageHistoryQuery : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
                ////////////////////调试用,自定义的数据授权
#if DEBUG
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "MessageHistoryQuery.aspx";                                       //向web用户控件传递当前调用的页面名称
                this.OrganisationTree_ProductionLine.LeveDepth = 5;
            }
        }
        [WebMethod]
        public static string GetQueryData(string organizationId, string organizationName, string startTime, string endTime, string state1)
        {
            DataTable table = MessageHitoryQueryService.GetSmsSendInfo(organizationId, organizationName, startTime, endTime, state1);
            string[] paraColumns = { "NodeType", "SenderType", "GroupKey1", "GroupKey2" };
            string json = EasyUIJsonParser.TreeGridJsonParser.DataTableToJsonByLevelCode(table, "LevelCode");
            return json;
        }
        //[WebMethod]
        //public static string GetComboboxValue()
        //{
        //    DataTable table = MessageHitoryQueryService.ComboboxValue();
        //    string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
        //    return json;
        //}
    }
}