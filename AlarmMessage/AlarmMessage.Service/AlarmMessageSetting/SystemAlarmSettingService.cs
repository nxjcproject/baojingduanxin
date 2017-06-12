using AlarmMessage.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmMessage.Service.AlarmMessageSetting
{
    public class  SystemAlarmSettingService
    {
        public static DataTable GetSystemAlarmTypeListTable(string alarmGroup) 
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string Sql = @"SELECT [PAGE_ID]                          
                        FROM [IndustryEnergy_SH].[dbo].[content]
                        where [NODE_ID]=@alarmGroup";
            SqlParameter mPara = new SqlParameter("alarmGroup", alarmGroup);
            DataTable Table = dataFactory.Query(Sql, mPara);
            string mPageId = "";
            if (Table.Rows.Count != 0)
            {
                mPageId = Table.Rows[0]["PAGE_ID"].ToString();
            }
            DataTable table = new DataTable();
            if (mPageId == "All" || mPageId == "")
            {
                string mSql = @" SELECT A.[AlarmTypeId]
                              ,A.[AlarmTypeName]                       
                            FROM [dbo].[system_SystemAlarmType] A
                            order by AlarmGroup,DisplayIndex";
                table = dataFactory.Query(mSql);
            }
            else
            {
                string mySql = @" SELECT [AlarmTypeId]
                              ,[AlarmTypeName]                       
                            FROM [NXJC].[dbo].[system_SystemAlarmType] A,[IndustryEnergy_SH].[dbo].[content] B
                            where B.[NODE_ID]=@alarmGroup
                            and A.[AlarmGroup]=B.[PAGE_ID]
                            order by AlarmGroup,DisplayIndex";
                SqlParameter para = new SqlParameter("alarmGroup", alarmGroup);
                table = dataFactory.Query(mySql, para);
            }
            return table;     
        }
//        private static DataTable SystemAlarmType(string alarmGroup) 
//        {
//            string connectionString = ConnectionStringFactory.NXJCConnectionString;
//            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
//            string mySql = @" SELECT [AlarmTypeId]
//                              ,[AlarmTypeName]                       
//                            FROM [dbo].[system_SystemAlarmType]
//                            order by AlarmGroup,DisplayIndex";
//            DataTable Table = dataFactory.Query(mySql);
//            return Table;
        
//        }
        public static DataTable GetSystemAlarmContrastTable(string organizationID,string AlarmType)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            DataTable Table = new DataTable();
            string mySql = @"SELECT A.[ContrastItemId]
                            ,A.[StaffInfoItemId]
                            ,B.[StaffInfoID]
	                        ,B.Name
	                        ,B.[PhoneNumber]
                            ,A.[OrganizationID]
	                     	,C.Name as Organization
                            ,A.[MessageType]
                            ,A.[AlarmTypeId]
                            ,A.[SendDelay]
                            ,A.[StartTime]
                            ,A.[EndTime]
                            ,A.[AlarmText]
                            ,A.[Enabled]
                          FROM [dbo].[terminal_SystemAlarmContrast] A,[dbo].[system_StaffInfo] B,[dbo].[system_Organization] C
                          where A.[StaffInfoItemId]=B.[StaffInfoItemId]
                          and A.[OrganizationID]=C.[OrganizationID]
                          and A.[OrganizationID] like @organizationId+'%' ";
            if (AlarmType == "AllValue")
            {
                mySql = mySql + @" order by B.[StaffInfoID],A.[AlarmTypeId]";
                Table = dataFactory.Query(mySql, new SqlParameter("@organizationId", organizationID));
            }
            else
            {
                mySql = mySql + @" and A.[AlarmTypeId]=@alarmType 
                                   order by B.[StaffInfoID],A.[AlarmTypeId]";
                SqlParameter[] myPara = { new SqlParameter("@organizationId", organizationID), 
                                      new SqlParameter("@alarmType", AlarmType) };
                Table = dataFactory.Query(mySql, myPara);

            } 
//            string mysql = @"SELECT B.[StaffInfoID]
//	                        ,B.Name
//	                        ,B.[PhoneNumber]
//                            ,A.[OrganizationID]
//	                      	,C.Name as Organization
//                            ,A.[MessageType]
//                            ,A.[AlarmTypeId]
//                            ,A.[SendDelay]
//                            ,A.[StartTime]
//                            ,A.[EndTime]
//                            ,A.[AlarmText]
//                            ,A.[Enabled]
//                          FROM BaiYin.[NXJC].[dbo].[terminal_SystemAlarmContrast] A,BaiYin.[NXJC].[dbo].[system_StaffInfo] B,BaiYin.[NXJC].[dbo].[system_Organization] C
//                          where A.[StaffInfoItemId]=B.[StaffInfoItemId]
//                          and A.[OrganizationID]=C.[OrganizationID]
//                          order by B.[StaffInfoID],A.[AlarmTypeId]";
//            Table = dataFactory.Query(mysql);
            return Table;    
        
        }
        public static DataTable GetStaffTable(string organizationID) 
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            DataTable Table = new DataTable();
            string mysql = @"SELECT  A.[StaffInfoID]
	                          ,A.[Name]
	                          ,A.[StaffInfoItemId]
	                          ,A.[PhoneNumber]
                              ,A.[OrganizationID]
	                          ,B.Name as Organization
                              ,A.[WorkingTeamName]
                              ,A.[WorkingSectionID]
                              ,A.[Sex]
                              ,A.[Enabled]
                          FROM [dbo].[system_StaffInfo] A,[dbo].[system_Organization] B
                          where A.[Enabled]=1
                          and A.[OrganizationID]=B.[OrganizationID]
                          and A.[OrganizationID] like @mOrganizationID+'%'
                          order by A.[StaffInfoID] asc";
            Table = dataFactory.Query(mysql, new SqlParameter("@mOrganizationID", organizationID));
            return Table;    
        }
        public static int AddSystemAlarmStaffInfoToTable(string alarmGroup, string isStaffInfoInsert, string contrastItemId, string organizationID, string alarmType, string alarmTypeName, string phoneNumber, string staffInfoItemId, string beginTime, string endTime, string delay, string enabled)       
        {
            int executeResult = 0;
            if (Convert.ToBoolean(isStaffInfoInsert))
            {
                if (alarmType != "AllValue")
                {
                    executeResult = InsertOperation(organizationID, alarmType, alarmTypeName, phoneNumber, staffInfoItemId, beginTime, endTime, delay, enabled);
                }
                else
                {
                    DataTable Table = GetSystemAlarmTypeListTable(alarmGroup);
                    string malarmType = "";
                    string malarmTypeName = "";
                    foreach (DataRow dR in Table.Rows)
                    {
                        malarmType = dR["AlarmTypeId"].ToString();
                        malarmTypeName = dR["AlarmTypeName"].ToString();
                        executeResult = executeResult + InsertOperation(organizationID, malarmType, malarmTypeName, phoneNumber, staffInfoItemId, beginTime, endTime, delay, enabled);
                    }
                }
            }
            else 
            {
                executeResult = UpdateOperation(contrastItemId, staffInfoItemId, alarmType, alarmTypeName, beginTime, endTime, delay, enabled);
            }
          
            return executeResult;
        }
        private static int InsertOperation(string organizationID, string alarmType, string alarmTypeName, string phoneNumber, string staffInfoItemId, string beginTime, string endTime, string delay, string enabled)
        {
            int executeResult = 0;
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string mGUID = System.Guid.NewGuid().ToString();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[terminal_SystemAlarmContrast]
                                    ([ContrastItemId]
                                    ,[StaffInfoItemId]
                                    ,[OrganizationID]
                                    ,[MessageType]
                                    ,[AlarmTypeId]
                                    ,[SendDelay]
                                    ,[StartTime]
                                    ,[EndTime]
                                    ,[AlarmText]
                                    ,[Enabled])
                                VALUES
                                    (@mContrastItemId
                                    ,@mStaffInfoItemId
                                    ,@mOrganizationID
                                    ,@mMessageType
                                    ,@mAlarmTypeId
                                    ,@mSendDelay
                                    ,@mStartTime
                                    ,@mEndTime
                                    ,@mAlarmText
                                    ,@mEnabled)";            
                command.Parameters.Add(new SqlParameter("@mContrastItemId", mGUID));
                command.Parameters.Add(new SqlParameter("@mStaffInfoItemId", staffInfoItemId));
                command.Parameters.Add(new SqlParameter("@mOrganizationID", organizationID));
                command.Parameters.Add(new SqlParameter("@mMessageType", "SMS"));
                command.Parameters.Add(new SqlParameter("@mAlarmTypeId", alarmType));
                command.Parameters.Add(new SqlParameter("@mSendDelay", delay));
                command.Parameters.Add(new SqlParameter("@mStartTime", beginTime));
                command.Parameters.Add(new SqlParameter("@mEndTime", endTime));
                command.Parameters.Add(new SqlParameter("@mAlarmText", alarmTypeName));
                command.Parameters.Add(new SqlParameter("@mEnabled", enabled));                             
                connection.Open();
                executeResult = command.ExecuteNonQuery();      
            }
               return executeResult;
        }
        private static int UpdateOperation(string contrastItemId, string staffInfoItemId, string malarmType, string malarmTypeName, string beginTime, string endTime, string delay, string enabled)
        {
            int executeResult = 0;
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            { 
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"UPDATE [dbo].[terminal_SystemAlarmContrast]
                                       SET 
                                           [StaffInfoItemId] = @mStaffInfoItemId
                                          ,[SendDelay] = @mSendDelay
                                            ,[AlarmTypeId]=@mAlarmTypeId
                                            ,[AlarmText]=@mAlarmText
                                          ,[StartTime] = @mStartTime
                                          ,[EndTime] = @mEndTime
                                          ,[Enabled] = @mEnabled
                                     WHERE  [ContrastItemId] =@mContrastItemId";
                command.Parameters.Add(new SqlParameter("@mContrastItemId", contrastItemId));
                command.Parameters.Add(new SqlParameter("@mStaffInfoItemId", staffInfoItemId));
                command.Parameters.Add(new SqlParameter("@mAlarmTypeId", malarmType));
                command.Parameters.Add(new SqlParameter("@mAlarmText", malarmTypeName));
                command.Parameters.Add(new SqlParameter("@mSendDelay", delay));
                command.Parameters.Add(new SqlParameter("@mStartTime", beginTime));
                command.Parameters.Add(new SqlParameter("@mEndTime", endTime));
                command.Parameters.Add(new SqlParameter("@mEnabled", enabled));
                connection.Open();
                executeResult = command.ExecuteNonQuery();
            }
            return executeResult;
        }
        public static int CancelSystemAlarmStaffInfoToTable(string contrastItemId) 
        {
            int executeResult = 0;
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string mGUID = System.Guid.NewGuid().ToString();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM [dbo].[terminal_SystemAlarmContrast]
                                                WHERE [ContrastItemId]=@mContrastItemId";
                command.Parameters.Add(new SqlParameter("@mContrastItemId", contrastItemId));           
                connection.Open();
                executeResult = command.ExecuteNonQuery();
            }
            return executeResult;       
        }
    }
}
