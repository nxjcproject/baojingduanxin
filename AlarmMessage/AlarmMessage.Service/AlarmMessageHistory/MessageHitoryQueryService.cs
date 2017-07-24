using AlarmMessage.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AlarmMessage.Service.AlarmMessageSetting
{
    public class MessageHitoryQueryService
    {
        public static DataTable GetSmsSendInfo(string organizationId, string organizationName, string startTime, string endTime, string state, string phoneNumber, string myStaticsMethod)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string m_Sql = @"SELECT A.SendItemId
                                  ,A.SenderKeyId
                                  ,A.GroupKey1
                                  ,A.GroupKey2
                                  ,A.GroupKey3
                                  ,A.PhoneNumber
                                  ,A.SendCount
                                  ,A.CreateTime
                                  ,A.OrderSendTime
                                  ,A.AlarmText
                                  ,(case when A.State = '0' then '正在发送'
	                                    when A.State = '1' then '重试满'
			                            when A.State = '2' then '超期'
			                            when A.State = '3' then '超条数'
			                            when A.State = '4' then '电话号码不合法'
			                            when A.State = '80' then '发送前报警解除'
			                            when A.State = '99' then '已发送' end) as SmsState
                                  ,A.SendResult
                              FROM terminal_SmsSendInfo A, system_AlarmLog B, system_Organization C, system_Organization D
                              where A.OrderSendTime >= '{0}'
                              and A.OrderSendTime < '{1}'
                              and A.SenderKeyId = B.AlarmItemId
                              and B.OrganizationID = C.OrganizationID
                              and D.OrganizationID = '{2}'
                              and C.LevelCode like D.LevelCode + '%'
                              {3}
                              {4}";
            string m_StateCondition = "";
            string m_PhoneNumberCondition = "";
            if (state != "All")
            {
                m_StateCondition = string.Format(" and A.State = {0} ", state);
            }
            if (phoneNumber != "" && phoneNumber != "undefined")
            {
                m_PhoneNumberCondition = string.Format(" and A.PhoneNumber = '{0}' ", phoneNumber);
            }
            m_Sql = string.Format(m_Sql, startTime, endTime, organizationId, m_StateCondition, m_PhoneNumberCondition);

            DataTable m_SmsSendInfoTable = dataFactory.Query(m_Sql);
            DataTable m_SmsSendInfoTreeTable = SetSmsSendInfoTree(m_SmsSendInfoTable, myStaticsMethod);
            return m_SmsSendInfoTreeTable;
        }
        private static DataTable SetSmsSendInfoTree(DataTable mySmsSendInfoTable, string myStaticsMethod)
        {
            if (mySmsSendInfoTable != null)
            {
                DataView m_SmsSendInfoView = mySmsSendInfoTable.DefaultView;
                m_SmsSendInfoView.Sort = "CreateTime desc,AlarmText, SendResult,PhoneNumber";
                if (myStaticsMethod == "SmsMessage")     //以短信内容组织树
                {

                    DataTable m_RootTable = m_SmsSendInfoView.ToTable(true, "SenderKeyId", "AlarmText", "GroupKey1", "GroupKey2", "GroupKey3");
                    DataTable m_SmsSendInfoTableSorted = m_SmsSendInfoView.ToTable();
                    m_SmsSendInfoTableSorted.Columns.Add("text", typeof(string));
                    for (int i = 0; i < m_RootTable.Rows.Count; i++)
                    {
                        DataRow m_NewRowTemp = m_SmsSendInfoTableSorted.NewRow();
                        m_NewRowTemp["text"] = m_RootTable.Rows[i]["AlarmText"];
                        m_NewRowTemp["SendItemId"] = m_RootTable.Rows[i]["SenderKeyId"];
                        m_NewRowTemp["SenderKeyId"] = "0";
                        m_NewRowTemp["GroupKey1"] = m_RootTable.Rows[i]["GroupKey1"];
                        m_NewRowTemp["GroupKey2"] = m_RootTable.Rows[i]["GroupKey2"];
                        m_NewRowTemp["GroupKey3"] = m_RootTable.Rows[i]["GroupKey3"];
                        m_NewRowTemp["PhoneNumber"] = DBNull.Value;
                        m_NewRowTemp["SendCount"] = DBNull.Value;
                        m_NewRowTemp["CreateTime"] = DBNull.Value;
                        m_NewRowTemp["OrderSendTime"] = DBNull.Value;
                        m_NewRowTemp["AlarmText"] = DBNull.Value;
                        m_NewRowTemp["SmsState"] = DBNull.Value;
                        m_NewRowTemp["SendResult"] = DBNull.Value;
                        m_SmsSendInfoTableSorted.Rows.Add(m_NewRowTemp);
                    }
                    m_SmsSendInfoTableSorted.Columns["SendItemId"].ColumnName = "id";
                    m_SmsSendInfoTableSorted.Columns["SenderKeyId"].ColumnName = "ParentId";
                    return m_SmsSendInfoTableSorted;
                }
                else                                    //以电话号码组织树
                {
                    DataTable m_RootTable = m_SmsSendInfoView.ToTable(true, "PhoneNumber");
                    DataTable m_SmsSendInfoTableSorted = m_SmsSendInfoView.ToTable();
                    m_SmsSendInfoTableSorted.Columns.Add("text", typeof(string));
                    for (int i = 0; i < m_RootTable.Rows.Count; i++)
                    {
                        DataRow m_NewRowTemp = m_SmsSendInfoTableSorted.NewRow();
                        m_NewRowTemp["text"] = m_RootTable.Rows[i]["PhoneNumber"];
                        m_NewRowTemp["SendItemId"] = m_RootTable.Rows[i]["PhoneNumber"];
                        m_NewRowTemp["SenderKeyId"] = DBNull.Value; 
                        m_NewRowTemp["GroupKey1"] = DBNull.Value;
                        m_NewRowTemp["GroupKey2"] = DBNull.Value;
                        m_NewRowTemp["GroupKey3"] = DBNull.Value;
                        m_NewRowTemp["PhoneNumber"] = "0";
                        m_NewRowTemp["SendCount"] = DBNull.Value;
                        m_NewRowTemp["CreateTime"] = DBNull.Value;
                        m_NewRowTemp["OrderSendTime"] = DBNull.Value;
                        m_NewRowTemp["AlarmText"] = DBNull.Value;
                        m_NewRowTemp["SmsState"] = DBNull.Value;
                        m_NewRowTemp["SendResult"] = DBNull.Value;
                        m_SmsSendInfoTableSorted.Rows.Add(m_NewRowTemp);
                    }
                    m_SmsSendInfoTableSorted.Columns["SendItemId"].ColumnName = "id";
                    m_SmsSendInfoTableSorted.Columns["PhoneNumber"].ColumnName = "ParentId";
                    m_SmsSendInfoTableSorted.Columns.Add("PhoneNumber", typeof(string));
                    return m_SmsSendInfoTableSorted;
                }
            }
            else
            {
                return null;
            }
        }
    }
}