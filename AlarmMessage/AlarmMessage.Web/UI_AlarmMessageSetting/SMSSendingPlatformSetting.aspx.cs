using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using AlarmMessage.Service.AlarmMessageSetting;

namespace AlarmMessage.Web.UI_AlarmMessageSetting
{
    public partial class SMSSendingPlatformSetting : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {               
                
            }
        }
        [WebMethod]
        public static string GetSmsConfigInfo()
        {
            DataTable table = SMSSendingPlatformSettingService.GetSmsConfigInfoTable();
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static int SaveSmsConfigInfo(string mSmsItemId, string mSmsName, string mInterfaceAddress, string mInterfacePort, string mUserCode, string mUserId, string mSmsTemplate, string mMaxSmsPerNumberOnDay, string mMaxSendTimesPerSms, string mMaxSmsWordLength, string mInvalidTime, string mRemark, string mEnabled)
        {
            int result = SMSSendingPlatformSettingService.SaveSmsConfigInfoResult(mSmsItemId, mSmsName, mInterfaceAddress, mInterfacePort, mUserCode, mUserId, mSmsTemplate, mMaxSmsPerNumberOnDay, mMaxSendTimesPerSms, mMaxSmsWordLength, mInvalidTime, mRemark, mEnabled);
            return result;
        }
        [WebMethod]
        public static string AmendPasswordInfo(string mSmsItemId, string mOldPwd, string mNewPwd)
        {
            if (mOldPwd == "")
            {
                return "请填写原密码!";
            }
            else if (mNewPwd == "")
            {
                return "请填写新密码!";
            }                      
            else
            {
                string m_AmendPasswordResult = SMSSendingPlatformSettingService.AmendPassword(mSmsItemId, mOldPwd, mNewPwd);
                return m_AmendPasswordResult;
            }
        }
    }
}