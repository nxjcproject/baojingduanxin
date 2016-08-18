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
        public static DataTable GetSmsSendInfo(string organizationId,string organizationName ,string startTime, string endTime, string state1)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string mySql = @"(SELECT '' as LevelCode,'node' as NodeType,[SenderKeyId] ,[SenderType] ,[GroupKey1],[GroupKey2] ,[OrderSendTime],[AlarmText]
                                    ,'' as [PhoneNumber],'' as [SendCount],'' as [State],'' as [SendResult]
                                FROM [NXJC].[dbo].[terminal_SmsSendInfo] 
                                where( [GroupKey1] like @organizationId+'%' or [GroupKey1]=@organizationName)
                                        and OrderSendTime>=@startTime
                                        and OrderSendTime<=@endTime
                                group by  [SenderKeyId],[SenderType],[GroupKey1],[GroupKey2],[OrderSendTime] ,[AlarmText])
	                        union all
                             (SELECT '' as LevelCode,'leafnode' as NodeType,A.[SenderKeyId],A.[SenderType],A.[GroupKey1],A.[GroupKey2],A.[OrderSendTime]
                                    ,A.[AlarmText] ,[PhoneNumber],A.[SendCount],B.[TYPE_NAME] as State,A.[SendResult]
                                FROM [NXJC].[dbo].[terminal_SmsSendInfo] A,[dbo].[system_TypeDictionary] B  
                                where  B.[GROUP_ID]='SMSendState' and A.state=B.[TYPE_ID]
                                     and   (A.[GroupKey1] like @organizationId+'%' or A.[GroupKey1]=@organizationName)
                                     and A.OrderSendTime>=@startTime
                                     and A.OrderSendTime<=@endTime
                                     and A.State=@state1)
                              order by OrderSendTime desc,NodeType desc ,GroupKey1";
            SqlParameter[] parameters ={
                            new SqlParameter("organizationId", organizationId),
                            new SqlParameter("organizationName", organizationName),
                            new SqlParameter("startTime", startTime),
                            new SqlParameter("endTime", endTime),
                            new SqlParameter("state1", state1)
                         };
            DataTable table = dataFactory.Query(mySql, parameters);
            int mcode = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string id = table.Rows[i]["NodeType"].ToString();
                if (id == "node")
                {
                    string nodeCode = "M01" + (++mcode).ToString("00");
                    table.Rows[i]["LevelCode"] = nodeCode;
                    int mleafcode = 0;
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        if (table.Rows[j]["SenderKeyId"].ToString().Trim() == table.Rows[i]["SenderKeyId"].ToString().Trim() && table.Rows[j]["NodeType"].ToString().Equals("leafnode"))
                        {
                            table.Rows[j]["LevelCode"] = nodeCode + (++mleafcode).ToString("00");
                        }
                    }
                }         
            }
            DataColumn stateColumn = new DataColumn("state", typeof(string));
            table.Columns.Add(stateColumn);
            foreach (DataRow dr in table.Rows)
            {
                if (dr["NodeType"].ToString() == "node")
                {
                    dr["state"] = "closed";
                }
                else
                {
                    dr["state"] = "open";
                }
            }
            return table;
        }
//        public static DataTable ComboboxValue()
//        {
//            string connectionString = ConnectionStringFactory.NXJCConnectionString;
//            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
//            string mySql = @"SELECT [TYPE_ID]
//                                ,[TYPE_NAME]
//                                ,[GROUP_ID]
//                                ,[DISPLAY_INDEX]
//                                ,[REMARK]
//                                ,[ENABLED]
//                            FROM [NXJC].[dbo].[system_TypeDictionary]
//                            where [GROUP_ID]='SMSendState' order by DISPLAY_INDEX";
//            DataTable table = dataFactory.Query(mySql);
//            return table;
//        }
    }
}