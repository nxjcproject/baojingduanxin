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
        public static string GetPageIdByNodeId(string myNodeId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string Sql = @"SELECT [PAGE_ID]                          
                        FROM [IndustryEnergy_SH].[dbo].[content]
                        where [NODE_ID]=@NodeId";
            SqlParameter mPara = new SqlParameter("NodeId", myNodeId);
            DataTable Table = dataFactory.Query(Sql, mPara);
            string mPageId = "";
            if (Table.Rows.Count != 0)
            {
                mPageId = Table.Rows[0]["PAGE_ID"].ToString();
            }
            return mPageId;
        }
        public static DataTable GetSystemAlarmTypeListTable(string alarmGroup)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           
            DataTable table = new DataTable();
            if (alarmGroup == "All" || alarmGroup == "")
            {
                string mSql = @" SELECT A.[AlarmTypeId]
                              ,A.[AlarmTypeName]                       
                            FROM [dbo].[system_SystemAlarmType] A
                            order by AlarmGroup,DisplayIndex";
                table = dataFactory.Query(mSql);
            }
            else
            {
                string mySql = @" SELECT A.[AlarmTypeId]
                              ,A.[AlarmTypeName]                       
                            FROM [NXJC].[dbo].[system_SystemAlarmType] A
                            where A.[AlarmGroup]= @alarmGroup
                            order by AlarmGroup,DisplayIndex";
                SqlParameter para = new SqlParameter("alarmGroup", alarmGroup);
                table = dataFactory.Query(mySql, para);
            }          
            return table;
        }
        public static DataTable GetMainMachineListTable(string organizationId, string startTime, string endTime, string type, string alarmGroup)
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
                            from  [dbo].[system_AlarmLog] A,[dbo].[system_Organization] B,[dbo].[system_SystemAlarmType] C,[dbo].[system_Organization] D
                        where A.OrganizationID=B.OrganizationID
                        and C.AlarmTypeId=A.AlarmTypeId
                        and ((A.StartTime>='{1}' and A.StartTime<='{2}') 
                              or (A.EndTime is null and C.AlarmMethod = 'continuous')
                              or (A.EndTime>='{1}' and A.EndTime<='{2}' and C.AlarmMethod = 'trigger'))
                        and D.OrganizationID = '{0}'
                        and B.LevelCode like D.LevelCode + '%'
                        {3}
                        {4}
                        order by A.[StartTime] desc";
            string m_Type = "";
            string m_AlarmGroup = "";
            if (type != "All")
            {
                m_Type = string.Format(" and A.AlarmTypeId = '{0}' ", type);
            }
            if (alarmGroup != "All" && alarmGroup != "")
            {
                m_AlarmGroup = string.Format(" and A.AlarmGroup = '{0}' ", alarmGroup);
            }

            mysql = string.Format(mysql, organizationId, startTime, endTime, m_Type, m_AlarmGroup);

            DataTable table = dataFactory.Query(mysql);
            return table;
        }
        public static DataTable GetRealTimeAlarm(string myOrganizationId, string myAlarmGroup)
        {
            string connectionstring = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionstring);
            string m_Sql = @"SELECT A.AlarmItemId
                                  ,A.AlarmGroup
                                  ,A.AlarmKeyId
                                  ,C.Name
                                  ,A.OrganizationID
                                  ,B.AlarmTypeName
                                  ,A.AlarmTypeId
                                  ,A.StartTime
                                  ,A.EndTime
                                  ,A.AlarmText
                              FROM system_AlarmLog A, system_SystemAlarmType B, system_Organization C, system_Organization D
                              where A.AlarmTypeId = B.AlarmTypeId
                              and (A.StartTime >= DATEADD(mi,-10,GETDATE()) 
                                      or (A.EndTime is null and B.AlarmMethod = 'continuous') 
                                      or (A.EndTime >= DATEADD(mi,-10,GETDATE()) and B.AlarmMethod = 'trigger'))
                              and D.OrganizationID = '{0}'
                              and C.LevelCode like D.LevelCode + '%'
                              and A.OrganizationID = C.OrganizationID
                              {1}
                              order by A.StartTime desc";
            string m_AlarmGroup = "";
            if (myAlarmGroup != "All" && myAlarmGroup != "")
            {
                m_AlarmGroup = string.Format(" and A.AlarmGroup = '{0}' ", myAlarmGroup);
            }
            m_Sql = string.Format(m_Sql, myOrganizationId, m_AlarmGroup);

            DataTable table = dataFactory.Query(m_Sql);
            return table;
        }
    }
}