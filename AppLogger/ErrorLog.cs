using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AppLogger
{
    public static class ErrorLog
    {
        public static string strAppPath = string.Empty;
        public static StreamWriter oWrite;

        #region Common Logs
        static object FileLock = new object();
        public static void CommonTrace(string msg)
        {
            lock (FileLock)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Common\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog_" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("{0} : {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff"), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockTrans = new object();
        public static void CommonTrans(string msg)
        {
            lock (FileLockTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Common\\TransactionLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransactionLog_" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("{0} : {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff"), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocCommonError = new object();
        public static void CommonError(Exception Ex)
        {
            lock (FileLocCommonError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Common\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog_" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()) + Environment.NewLine);
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region DataBase Logs
        static object FileLocDBTrace = new object();
        public static void DBTrace(string msg)
        {
            lock (FileLocDBTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\DB Logs\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocDBError = new object();
        public static void DBError(Exception Ex)
        {
            lock (FileLocDBError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\DB Logs\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Email Logs
        static object FileLockEmail = new object();
        public static void writeLogEmail(string msg)
        {
            lock (FileLockEmail)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Emails\\Trans";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransactionLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        static object FileLockError = new object();
        public static void writeLogEmailError(string msg)
        {
            lock (FileLockError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Emails\\Error";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region SMS Logs
        static object FileLocSMSTrace = new object();
        public static void SMSTrace(string msg)
        {
            lock (FileLocSMSTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\SMS Interface\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocSMSError = new object();
        public static void SMSError(Exception Ex)
        {
            lock (FileLocSMSError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\SMS Interface\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region BBPS Comaplaint Logs
        static object FileLockBBPSComlaintsTrans = new object();
        public static void writeLogBBPSComlaintsTrans(string msg)
        {
            lock (FileLockBBPSComlaintsTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Complaints\\BBPS\\Trans";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransactionLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockBBPSComlaintsError = new object();
        public static void writeLogBBPSComlaintsError(string msg)
        {
            lock (FileLockBBPSComlaintsError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Complaints\\BBPS\\Error";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Other Comaplaint Logs
        static object FileLockOhterComlaintsTrans = new object();
        public static void writeLogOtherComlaintsTrans(string msg)
        {
            lock (FileLockOhterComlaintsTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Complaints\\Others\\Trans";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransactionLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockOthersComlaintsError = new object();
        public static void writeLogOthersComlaintsError(string msg)
        {
            lock (FileLockOthersComlaintsError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Complaints\\Others\\Error";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Page Levels Logs
        public static void CreateFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ErrorLog";

                if (!(System.IO.File.Exists(strAppPath + "\\Error" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\Error" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DMT logs
        public static void CreateDMT_XMLFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "DMTLogs";

                if (!(System.IO.File.Exists(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateOTP_XMLFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "OTPLogs";

                if (!(System.IO.File.Exists(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeDMTLogs(string methodname, string request, string response)
        {
            try
            {
                CreateDMT_XMLFile();
                File.AppendAllText(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DMT_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public static void writeOTPLogs(string methodname, string request, string response)
        {
            try
            {
                CreateOTP_XMLFile();
                File.AppendAllText(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\OTPLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region BBPS logs
        public static void CreateBBPS_XMLFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "BBPSLogs";

                if (!(System.IO.File.Exists(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeBBPSLogs(string methodname, string request, string response)
        {
            try
            {
                CreateBBPS_XMLFile();
                File.AppendAllText(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\BBPS_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region RECHARGE Logs.
        public static void CreateRecharge_XMLFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "RechargeLogs";

                if (!(System.IO.File.Exists(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeRechargeLogs(string methodname, string request, string response)
        {
            try
            {
                CreateRecharge_XMLFile();
                File.AppendAllText(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\Recharge_XML_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region AEPS logs
        public static void CreateAEPS_JSONFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "AEPSLogs";

                if (!(System.IO.File.Exists(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeAEPSLogs(string methodname, string request, string response)
        {
            try
            {
                CreateAEPS_JSONFile();
                File.AppendAllText(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AEPS_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region VA logs
        public static void CreateVirtualAccount_JSONFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "AEPSLogs";

                if (!(System.IO.File.Exists(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeAgentVA_Logs(string methodname, string request, string response)
        {
            try
            {
                CreateVirtualAccount_JSONFile();
                File.AppendAllText(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentVirtualAccount_JSON_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion
   
        #region Commission Logs.
        public static void CreateCommissionFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "CommissionLog";

                if (!(System.IO.File.Exists(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeCommission_Logs(string methodname, string data, string msg)
        {
            try
            {
                CreateCommissionFile();
                File.AppendAllText(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request  : " + data + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + msg + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\CommissionLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region Settlement Transactions
        static object FileLockSettledTrans = new object();
        public static void TransSettlmentTrans(string msg)
        {
            lock (FileLockSettledTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Transaction\\Settlement\\TransLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockSettledTransError = new object();
        public static void TransSettlmentError(Exception Ex)
        {
            lock (FileLockSettledTransError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Transaction\\Settlement\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Wallet REFIL Logs
        static object FileLockRefilTrans = new object();
        public static void writeLogRefilTrans(string msg)
        {
            lock (FileLockRefilTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Wallet Transfer\\TraceLog";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockRefilTransError = new object();
        public static void writeLogRefilTransError(Exception Ex)
        {
            lock (FileLockRefilTransError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Wallet Transfer\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region EKYC Logs
        public static void CreateEKYC_XMLFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Error Logs\\EKYC";

                if (!(System.IO.File.Exists(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeAgentEKYCLogs(string methodname, string request, string response)
        {
            try
            {
                CreateEKYC_XMLFile();
                File.AppendAllText(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\AgentEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        static object FileLocEKYCErrorError = new object();
        public static void EKYCError(Exception Ex)
        {
            lock (FileLocEKYCErrorError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\EKYC\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Device EKYC Logs
        static object FileLockDeviceEKYCTrans = new object();
        public static void writeLogDeviceEKYCTrans(string msg)
        {
            lock (FileLockDeviceEKYCTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\EKYC\\Device\\Trans";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransactionLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockDeviceEKYCError = new object();
        public static void DeviceEKYCError(Exception Ex)
        {
            lock (FileLockDeviceEKYCError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Device EKYC";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        public static void CreateDeviceEKYC_XMLFile()
        {
            try
            {
                strAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Error Logs\\Device EKYC";

                if (!(System.IO.File.Exists(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt") == true))
                {
                    Directory.CreateDirectory(strAppPath);
                    oWrite = File.CreateText(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt");
                    oWrite.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void writeDeviceEKYCLogs(string methodname, string request, string response)
        {
            try
            {
                CreateDeviceEKYC_XMLFile();
                File.AppendAllText(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Method Name : " + methodname + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Request : " + request + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "Response : " + response + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "At : " + String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()) + Environment.NewLine);
                File.AppendAllText(strAppPath + "\\DeviceEKYCLogs" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", "-------------****-----------------" + Environment.NewLine);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region Alert Management Logs
        static object FileLockAlertTrans = new object();
        public static void writeLogAlertTrans(string msg)
        {
            lock (FileLockAlertTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Alert Services\\Trans";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransactionLogs-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockAlertError = new object();
        public static void writeLogAlertError(string msg)
        {
            lock (FileLockAlertError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Alert Services\\Error";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region XML Serialisation

        public static string XmlSerialser<T>(T Tobj)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(Tobj.GetType());
                serializer.Serialize(stringwriter, Tobj);
                return stringwriter.ToString();
            }
        }

        public static string ConvertXmlToString<T>(T Tobj)
        {
            string XmlResponse = string.Empty;
            using (var stringwriter = new System.IO.StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var serializer = new XmlSerializer(Tobj.GetType());
                serializer.Serialize(stringwriter, Tobj, ns);
                XmlResponse = stringwriter.ToString().Trim();
            }
            XmlResponse = RemoveXmlns(XmlResponse);
            return XmlResponse;
        }

        public static string RemoveXmlns(string xml)
        {
            XDocument d = XDocument.Parse(xml);
            d.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in d.Descendants())
                elem.Name = elem.Name.LocalName;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(d.CreateReader());

            return xmlDocument.InnerXml;
        }
        #endregion

        #region Fetch BBPS Billers Logs
        static object FileLockBillers = new object();
        public static void BillerTrace(string msg)
        {
            lock (FileLockBillers)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Fetch Billers\\TraceLog";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockBillerError = new object();
        public static void BillerError(Exception Ex)
        {
            lock (FileLockBillerError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Fetch Billers\\ErrorLog";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }

                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Message : {0}", Ex.StackTrace));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Bulk Wallet Transfer
        static object FileLockWalletTransfer = new object();
        public static void WalletTransferTrace(string msg)
        {
            lock (FileLockWalletTransfer)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Bulk Wallet Transfer\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLockWalletTransferError = new object();
        public static void WalletTransferError(Exception Ex)
        {
            lock (FileLockWalletTransferError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Bulk Wallet Transfer\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Agent Management Logs
        static object FileLocAgentManagementTrace = new object();
        public static void AgentManagementTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocAgentManagementError = new object();
        public static void AgentManagementError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Report Logs
        static object FileLocReportTrace = new object();
        public static void ReportTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocReportError = new object();
        public static void ReportError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Limit Logs
        static object FileLocLimitTrace = new object();
        public static void LimitTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }



        static object FileLocLimitError = new object();
        public static void LimitError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Transaction Report Logs
        static object FileLocTransactionReportTrace = new object();
        public static void TransactionReportTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Report\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocTransactionReportError = new object();
        public static void TransactionReportError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Report\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region BC Management Logs
        static object FileLocBCManagementTrace = new object();
        public static void BCManagementTrace(string msg)
        {
            lock (FileLocBCManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\BC Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocBCManagementError = new object();
        public static void BCManagementError(Exception Ex)
        {
            lock (FileLocBCManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\BC Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Terminal Configuration Logs
        static object FileLockTerminalConfigurationError = new object();
        public static void TerminalLogsError(Exception Ex)
        {
            lock (FileLockTerminalConfigurationError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Pull Terminal Logs\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }



        static object FileLockTerminalConError = new object();
        public static void TerminalLogsTrace(string msg)
        {
            lock (FileLockTerminalConError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Pull Terminal Logs\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Agent Commission Logs
        static object FileLocAgentCommissionTrace = new object();
        public static void CommissionConfigTrace(string msg)
        {
            lock (FileLocAgentCommissionTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Commission Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocAgentCommissionError = new object();
        public static void CommissionConfigError(Exception Ex)
        {
            lock (FileLocAgentCommissionError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Commission Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region ZOM Logs
        static object FileLocZOMTrans = new object();
        public static void ZOMTrans(string msg)
        {
            lock (FileLocZOMTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\ZOM Management\\TransLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        static object FileLocZOMError = new object();
        public static void ZOMError(Exception Ex)
        {
            lock (FileLocZOMError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\ZOM Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion  

        #region Commission Logs
        static object FileLocCommissionTrans = new object();
        public static void CommissionTrans(string msg)
        {
            lock (FileLocCommissionTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Commission Management\\TransLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        static object FileLocCommissionError = new object();
        public static void CommissionError(Exception Ex)
        {
            lock (FileLocCommissionError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Commission Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion  

        #region User Profile Logs
        static object FileLocProfileTrans = new object();
        public static void ProfileTrans(string msg)
        {
            lock (FileLocProfileTrans)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\User Profile\\TransLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TransLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        static object FileLocProfileError = new object();
        public static void ProfileError(Exception Ex)
        {
            lock (FileLocProfileError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\User Profile\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

        #region Admin Management Logs
        static object FileLocAdminManagementTrace = new object();
        public static void AdminManagementTrace(string msg)
        {
            lock (FileLocAdminManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Admin Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocAdminManagementError = new object();
        public static void AdminManagementError(Exception Ex)
        {
            lock (FileLocAdminManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Admin Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                                               "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                                               "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion


        #region

      
        public static void UploadTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocUploadError = new object();
        public static void UploadError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                    "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                    "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

      
        #region Dashboard Logs
        static object FileLocDashboardTrace = new object();
        public static void DashboardTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocDashboardError = new object();
        public static void DashboardError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Agent Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                    "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                    "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion


        //SOR
        #region Rule Logs
        static object FileLocRuleTrace = new object();
        public static void RuleTrace(string msg)
        {
            lock (FileLocAgentManagementTrace)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Rule Management\\TraceLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\TraceLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine(String.Format("Time : {0} {1} Message : {2}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString(), msg));
                    sw.Close();
                    tw.Close();
                }
            }
        }

        static object FileLocRuleError = new object();
        public static void RuleError(Exception Ex)
        {
            lock (FileLocAgentManagementError)
            {
                string m_Path = HttpContext.Current.Server.MapPath("~/") + "Error Logs\\Rule Management\\ErrorLogs";
                {
                    if (!Directory.Exists(m_Path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(m_Path);
                        dir.Create();
                    }
                    FileStream tw = new FileStream(String.Format(@"{0}.txt", m_Path + "\\ErrorLog-" + DateTime.Now.ToString("dd-MMM-yyyy")), FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(tw);
                    sw.WriteLine("----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("Time : {0} {1}", DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToShortTimeString()));
                    sw.WriteLine(String.Format("Source - : " + Ex.Source.ToString() + Environment.NewLine +
                    "StackTrace -  : " + Ex.StackTrace.ToString() + Environment.NewLine +
                    "Message - : " + Ex.Message.ToString()));
                    sw.Close();
                    tw.Close();
                }
            }
        }
        #endregion

    }
}
