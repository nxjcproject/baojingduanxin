using AlarmMessage.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace baojinglishijiluchaxun.Service.Baojinglishijiluchaxun
{
    public class AlarmHistorySelect1
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
            if (Table.Rows.Count!=0)
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
        public static DataTable GetMainMachineListTable(string organizationId, string startTime, string endTime, string type)
        {
            string connectionstring = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionstring);
            string mysql = @"select 
                             A.[AlarmItemId]
                            ,A.[AlarmGroup]
                            ,A.[AlarmKeyId]
	                        ,B.Name
                            ,A.[OrganizationID]
	                        ,C.AlarmTypeName
                            ,A.[AlarmTypeId]
                            ,A.[StartTime]
                            ,A.[EndTime]
                            ,A.[AlarmText]
                            from  [dbo].[system_AlarmLog] A,[dbo].[system_Organization] B,[dbo].[system_SystemAlarmType] C
                        where A.OrganizationID=B.OrganizationID
                        and C.AlarmTypeId=A.AlarmTypeId
                        and ((A.StartTime>=@startTime and A.EndTime<=@endTime) or (A.StartTime>=@startTime and A.EndTime is NULL))
                        and A.OrganizationID like  @organizationId + '%'
                        and  A.AlarmTypeId like @type
                        order by A.[StartTime],A.[EndTime]";
            SqlParameter[] parameters = { new SqlParameter("@organizationId", organizationId), new SqlParameter("@startTime", startTime), new SqlParameter("@endTime", endTime), new SqlParameter("@type", type) };
            DataTable table = dataFactory.Query(mysql, parameters);
            return table;
        }
        public static DataTable GetMainMachineListTable(string organizationId, string startTime, string endTime)
        {
            string connectionstring = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionstring);
            string mysql = @"select 
                             A.[AlarmItemId]
                            ,A.[AlarmGroup]
                            ,A.[AlarmKeyId]
	                        ,B.Name
                            ,A.[OrganizationID]
	                        ,C.AlarmTypeName
                            ,A.[AlarmTypeId]
                            ,A.[StartTime]
                            ,A.[EndTime]
                            ,A.[AlarmText]
                            from  [dbo].[system_AlarmLog] A,[dbo].[system_Organization] B,[dbo].[system_SystemAlarmType] C
                        where A.OrganizationID=B.OrganizationID
                        and C.AlarmTypeId=A.AlarmTypeId
                        and A.StartTime>=@startTime 
                        and A.EndTime<=@endTime 
                        and A.OrganizationID like  @organizationId + '%'
                        order by [StartTime] desc";
            SqlParameter[] parameters = { new SqlParameter("@organizationId", organizationId), new SqlParameter("@startTime", startTime), new SqlParameter("@endTime", endTime) };
            DataTable table = dataFactory.Query(mysql, parameters);
            return table;
        }
    }
}