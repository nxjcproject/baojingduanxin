using AlarmMessage.Service.AlarmMessageSetting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlarmMessage.Web.UI_AlarmMessageSetting
{
    public partial class SystemAlarmSetting : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权

                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf", "zc_nxjc_qtx", "zc_nxjc_tsc_tsf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");                         //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "SystemAlarmSetting.aspx";   //向web用户控件传递当前调用的页面名称
                this.OrganisationTree_ProductionLine.LeveDepth = 5;
            }
        }
        [WebMethod]
        public static string SystemAlarmTypeList() 
        {
            DataTable table = SystemAlarmSettingService.GetSystemAlarmTypeListTable();
            string json=EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetSystemAlarmContrast(string organizationID,string AlarmType)
        {
            DataTable table = SystemAlarmSettingService.GetSystemAlarmContrastTable(organizationID,AlarmType);
            string json=EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;      
        }
        [WebMethod]
        public static string GetStaffTableData(string organizationID) 
        {
            DataTable table = SystemAlarmSettingService.GetStaffTable(organizationID);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;          
        }
        [WebMethod]
        public static int AddSystemAlarmStaffInfo(string isStaffInfoInsert,string contrastItemId,string organizationID, string alarmType, string alarmTypeName,string phoneNumber, string staffInfoItemId, string beginTime, string endTime, string delay, string enabled)
            //'{organizationID: "' + organizationId + '",staffId:"' + mStaffInfoID + '",phoneNumber:"' + mPhoneNumber + '",staffInfoItemId:"' + mStaffInfoItemId + '",beginTime:"' + mBeginTime + '",endTime:"' + mEndTime + '",delay:"' + mdelay + '",enabled:"' + mEnabled + '"}',
        {
            int reback = SystemAlarmSettingService.AddSystemAlarmStaffInfoToTable(isStaffInfoInsert, contrastItemId,organizationID, alarmType, alarmTypeName, phoneNumber, staffInfoItemId, beginTime, endTime, delay, enabled);
            return reback;
        }
        [WebMethod]
        public static int CancelSystemAlarmStaffInfo(string contrastItemId) 
        {
            int reback = SystemAlarmSettingService.CancelSystemAlarmStaffInfoToTable(contrastItemId);
            return reback;       
        }
    }
}