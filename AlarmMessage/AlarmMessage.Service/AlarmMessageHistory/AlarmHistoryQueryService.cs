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
                        and A.StartTime>=@startTime 
                        and A.EndTime<=@endTime 
                        and A.OrganizationID like  @organizationId + '%'
                        and  A.AlarmTypeId like @type";
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