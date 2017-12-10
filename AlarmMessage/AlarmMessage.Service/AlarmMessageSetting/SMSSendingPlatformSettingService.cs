using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlarmMessage.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;

namespace AlarmMessage.Service.AlarmMessageSetting
{
    public class SMSSendingPlatformSettingService
    {
        public static DataTable GetSmsConfigInfoTable()
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string Sql = @"SELECT TOP 1 *
                                  FROM [NXJC].[dbo].[terminal_SmsConfig]";
            try
            {
                DataTable table = dataFactory.Query(Sql);
                return table;
            }
            catch
            {
                return null;
            }
        }

        public static int SaveSmsConfigInfoResult(string mSmsItemId, string mSmsName, string mInterfaceAddress, string mInterfacePort, string mUserCode, string mUserId, string mSmsTemplate, string mMaxSmsPerNumberOnDay, string mMaxSendTimesPerSms, string mMaxSmsWordLength, string mInvalidTime, string mRemark, string mEnabled)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            string mySql = @"UPDATE [dbo].[terminal_SmsConfig]
                                SET  [SmsName]=@mSmsName
                                    ,[InterfaceAddress]=@mInterfaceAddress
                                    ,[InterfacePort]=@mInterfacePort
                                    ,[UserCode]=@mUserCode
                                    ,[UserId]=@mUserId
                                    ,[SmsTemplate]=@mSmsTemplate
                                    ,[MaxSmsPerNumberOnDay]=@mMaxSmsPerNumberOnDay
                                    ,[MaxSendTimesPerSms]=@mMaxSendTimesPerSms                                   
                                    ,[MaxSmsWordLength]=@mMaxSmsWordLength
                                    ,[InvalidTime]=@mInvalidTime
                                    ,[Remark]=@mRemark
                                    ,[Enabled]=@mEnabled                                   
                              WHERE  [SmsItemId]=@mSmsItemId";
            SqlParameter[] para = { new SqlParameter("@mSmsItemId",mSmsItemId),
                                    new SqlParameter("@mSmsName",mSmsName),
                                    new SqlParameter("@mInterfaceAddress",mInterfaceAddress),
                                    new SqlParameter("@mInterfacePort",mInterfacePort),
                                    new SqlParameter("@mUserCode",mUserCode),
                                    new SqlParameter("@mUserId",mUserId),
                                    new SqlParameter("@mSmsTemplate", mSmsTemplate),
                                    new SqlParameter("@mMaxSmsPerNumberOnDay", mMaxSmsPerNumberOnDay),
                                    new SqlParameter("@mMaxSendTimesPerSms",  mMaxSendTimesPerSms),
                                    new SqlParameter("@mMaxSmsWordLength", mMaxSmsWordLength),                                   
                                    new SqlParameter("@mInvalidTime",mInvalidTime),
                                    new SqlParameter("@mRemark", mRemark),
                                    new SqlParameter("@mEnabled", mEnabled)};
            try
            {
                int result = factory.ExecuteSQL(mySql, para);
                return result;
            }
            catch
            {
                return 0;
            }        
        }

        public static string GetOldPassword()
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string Sql = @"SELECT TOP 1 *
                                  FROM [NXJC].[dbo].[terminal_SmsConfig]";
            DataTable table = dataFactory.Query(Sql);
            return table.Rows[0]["Password"].ToString();
        }

        public static string AmendPassword(string mSmsItemId, string mOldPwd, string mNewPwd)
        {
            string DecryptGetOldpwd = DesDecrypt(GetOldPassword());
            string result = "";
            if (!mOldPwd.Equals(DecryptGetOldpwd))
            {
                result = "原密码不正确！";
            }
            if (mOldPwd.Equals(DecryptGetOldpwd))
            {
                string mEncryptNewPwd = DesEncrypt(mNewPwd);
                string connectionString = ConnectionStringFactory.NXJCConnectionString;
                ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
                string Sql = @"UPDATE [NXJC].[dbo].[terminal_SmsConfig] 
                                  SET [Password]=@mEncryptNewPwd
                                  WHERE [SmsItemId]=@mSmsItemId";
                SqlParameter[] m_para = { new SqlParameter("@mEncryptNewPwd", mEncryptNewPwd),
                                          new SqlParameter("@mSmsItemId",mSmsItemId)};
                int dt = dataFactory.ExecuteSQL(Sql, m_para);
                result = dt.ToString();
            }       
            return result;            
        }

        private const string Key = "Sdht112332125432Co3221353~";
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="val"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(Key.Substring(0, 24));
                DES.Mode = CipherMode.ECB;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(encryptString);
                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch {
                return "";
            }
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(Key.Substring(0, 24));
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(decryptString);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
            }
            return result;
        }
    }
}
