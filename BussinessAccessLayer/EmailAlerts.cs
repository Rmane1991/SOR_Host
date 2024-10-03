using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AppLogger;
using System.Net;
using System.IO;
using MaxiSwitch.EncryptionDecryption;

namespace BussinessAccessLayer
{
    public class EmailAlerts
    {
        #region Object Declarations
        AppSecurity _AppSecurity = new AppSecurity();
        DataSet dataSet = null;
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        #endregion

        #region Variable Declarations
        public string _Status = string.Empty;
        public string _Status_Out = string.Empty;
        public string UserName { get; set; }
        public string AlertTypeId { get; set; }
        public string CategoryTypeId { get; set; }
        public string SubCategoryTypeId { get; set; }
        public string ClientID { get; set; }
        public string UserID { get; set; }
        public string numClient { get; set; }
        public string RoleID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Flag { get; set; }
        public string RRN { get; set; }
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
        public string BCCEmail { get; set; }
        public string ClientName { get; set; }
        public string ContactPersonDept { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string ContactNumber3 { get; set; }
        public string MultiContactNumber4 { get; set; }
        public string TerminalID { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ID { get; set; }
        public string ProcessType { get; set; }
        public string ProcessTypeId { get; set; }
        public string TargetType { get; set; }
        public string TargetTypeId { get; set; }
        public string BatchType { get; set; }
        public string BatchTypeId { get; set; }
        public string UserType { get; set; }
        public string UserTypeId { get; set; }
        public string TxnTypeName { get; set; }
        public string FrequencyType { get; set; }
        public string Frequency { get; set; }
        public string TimeSpan { get; set; }
        public string BatchID { get; set; }
        #endregion

        // Used Methods  //
        #region Eamil Send Main Code
        public string SendEmailNew(string _UserID, string _RoleID, string _numClientID, string _ClientID, string ToEmailIDs, string CCEmailIDs, string Subject, string MailBody)
        {
            string Flag = string.Empty;
            string FromAddress = null, MailPassword = null;
            MailMessage myMail = new MailMessage();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 10000;

            try
            {
                ErrorLog.writeLogEmail("Email Send Request Received. UserId : "+ _UserID +" RoleID : " +_RoleID + " numClientID : " +_numClientID + " ClientID : " +_ClientID +" ToEmailIDs : "+ ToEmailIDs + " CCEmailIDs : " +  CCEmailIDs + " Subject : "+ Subject + " MailBody : "+ MailBody);
                if( string.IsNullOrEmpty(ToEmailIDs) || string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(MailBody))
                {
                    ErrorLog.writeLogEmail("Email Send Request Failed. Mandatory Fields Should Not Empty (ToEmailIDs/Subject/MailBody)");
                    Flag = "Fail";
                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                    return Flag;
                }
                try
                {
                    FromAddress = ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.AppSettings["FromAddress"]);
                    MailPassword = ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.AppSettings["MailPassword"]);
                    //FromAddress = ConfigurationManager.AppSettings["FromAddress"].ToString();
                    //MailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
                }
                catch (Exception Ex)
                {
                    ErrorLog.writeLogEmail("Email Send Request Failed While Decrypt Credentials. Exception : " + Ex.Message);
                    Flag = "Fail";
                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                    return Flag;
                }

                try
                {
                    foreach (var address in ToEmailIDs.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        myMail.To.Add(address);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.writeLogEmail("Email Send Request Failed While Split ToEmailIDs. Exception : " + Ex.Message);
                    Flag = "Fail";
                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                    return Flag;
                }
                try
                {
                    foreach (var address in CCEmailIDs.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        myMail.CC.Add(address);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.writeLogEmail("Email Send Request Failed While Split CCEmailIDs. Exception : " + Ex.Message);
                    Flag = "Fail";
                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                    return Flag;
                }

                myMail.From = new MailAddress(FromAddress, "PayRakam Notification");
                myMail.Subject = Subject.Trim();
                myMail.IsBodyHtml = true;
                myMail.Body = MailBody;
                try
                {
                    SmtpClient mySmtpClient = new SmtpClient();
                    System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential(FromAddress, MailPassword);
                    mySmtpClient.Host = ConfigurationManager.AppSettings["SMTPAddress"];
                    mySmtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPortNo"]);
                    mySmtpClient.EnableSsl = true;
                    mySmtpClient.Credentials = myCredential;
                    try
                    {
                        mySmtpClient.Send(myMail);
                        Flag = "Done";
                        InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "1", 1);
                        ErrorLog.writeLogEmail("Email Send Request Successful. UserId : " + _UserID + " RoleID : " + _RoleID + " numClientID : " + _numClientID + " ClientID : " + _ClientID + " ToEmailIDs : " + ToEmailIDs + " CCEmailIDs : " + CCEmailIDs + " Subject : " + Subject + " MailBody : " + MailBody);
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                            {
                                ErrorLog.writeLogEmail("Delivery failed - retrying in 5 seconds. UserId : " + _UserID + " RoleID : " + _RoleID + " numClientID : " + _numClientID + " ClientID : " + _ClientID + " ToEmailIDs : " + ToEmailIDs + " CCEmailIDs : " + CCEmailIDs + " Subject : " + Subject + " MailBody : " + MailBody);
                                System.Threading.Thread.Sleep(1000);
                                try
                                {
                                    mySmtpClient.Send(myMail);
                                    Flag = "Done";
                                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "1", 1);
                                    ErrorLog.writeLogEmail("Email Send Request Successful (Retry). UserId : " + _UserID + " RoleID : " + _RoleID + " numClientID : " + _numClientID + " ClientID : " + _ClientID + " ToEmailIDs : " + ToEmailIDs + " CCEmailIDs : " + CCEmailIDs + " Subject : " + Subject + " MailBody : " + MailBody);
                                    return Flag;
                                }
                               catch(Exception Ex)
                                {
                                    ErrorLog.writeLogEmail("Email Send Request Failed While Hit (Retry) SMTP Host. Exception : " + Ex.Message);
                                    Flag = "Fail";
                                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                                    return Flag;
                                }
                            }
                            else
                            {
                                ErrorLog.writeLogEmail("Failed to deliver message to "+ ex.InnerExceptions[i].FailedRecipient + " UserId : " + _UserID + " RoleID : " + _RoleID + " numClientID : " + _numClientID + " ClientID : " + _ClientID + " ToEmailIDs : " + ToEmailIDs + " CCEmailIDs : " + CCEmailIDs + " Subject : " + Subject + " MailBody : " + MailBody);
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.writeLogEmail("Email Send Request Failed While Hit SMTP Host. Exception : " + Ex.Message);
                    Flag = "Fail";
                    InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                    return Flag;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.writeLogEmail("Email Send Request Failed. Exception : " + Ex.Message);
                Flag = "Fail";
                InsertEmailAlert(_numClientID, _ClientID, _RoleID, _UserID, ToEmailIDs, CCEmailIDs, Subject, MailBody, "2", 2);
                return Flag;
            }
            return Flag;
        }
        #endregion

        #region Prepaid Format For Email
        public void PrepareEmailFormat()
        {
            DataSet dataset = new DataSet();
            string _UserID = string.Empty; string _RoleID = string.Empty; string _numClientID = string.Empty; string _ClientID = string.Empty;
            string _ToEmailIDs = string.Empty; string _CCEmailIDs = string.Empty; string _Subject = string.Empty; string _MailBody = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        ErrorLog.writeLogEmail("EMAIL Format Preperation Request Received. AlertTypeId : " + AlertTypeId + " CategoryTypeId : " + CategoryTypeId + " SubCategoryTypeId : " + SubCategoryTypeId + " ClientID : " + ClientID + " UserName : " + UserName + " UserID : " + UserID + " RRN : " + RRN + " Flag : " + Flag);
                        SqlParameter[] _Params =
                        {
                                  new SqlParameter("@AlertTypeId",AlertTypeId),
                                  new SqlParameter("@CategoryTypeId",CategoryTypeId),
                                  new SqlParameter("@SubCategoryTypeId",SubCategoryTypeId),
                                  new SqlParameter("@ClientID",ClientID),
                                  new SqlParameter("@Username",UserName),
                                  new SqlParameter("@UserID",UserID),
                                  new SqlParameter("@RRN",RRN),
                                  new SqlParameter("@Flag",Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "GetEmailDetailsToSend";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataset);
                        cmd.Dispose();
                    }
                }

                if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dataset.Tables.Count; i++)
                    {
                        _UserID = dataset.Tables[i].Rows[0]["UserID"].ToString();
                        _RoleID = dataset.Tables[i].Rows[0]["RoleID"].ToString();
                        _numClientID = dataset.Tables[i].Rows[0]["numClientID"].ToString();
                        _ClientID = dataset.Tables[i].Rows[0]["ClientID"].ToString();
                        _ToEmailIDs = dataset.Tables[i].Rows[0]["ToEmails"].ToString();
                        _CCEmailIDs = dataset.Tables[i].Rows[0]["CCEmails"].ToString();
                        _Subject = dataset.Tables[i].Rows[0]["Subject"].ToString();
                        _MailBody = dataset.Tables[i].Rows[0]["Body"].ToString();
                        if (!string.IsNullOrEmpty(_MailBody) && !string.IsNullOrEmpty(_Subject) && !string.IsNullOrEmpty(_ToEmailIDs))
                        {
                            ErrorLog.writeLogEmail("EMAIL Format Preperation Request Successful. UserId : " + _UserID + " RoleID : " + _RoleID + " numClientID : " + _numClientID + " ClientID : " + _ClientID + " ToEmailIDs : " + _ToEmailIDs + " CCEmailIDs : " + _CCEmailIDs + " Subject : " + Subject + " MailBody : " + _MailBody);
                            SendEmailNew(_UserID, _RoleID, _numClientID, _ClientID, _ToEmailIDs, _CCEmailIDs, _Subject, _MailBody);
                        }
                        else
                            ErrorLog.writeLogEmail("EMAIL Format Preperation Request Failed. Mandatory Fields Should Not Empty (ToEmailIDs/Subject/MailBody)");
                    }
                }
                else
                {
                    ErrorLog.writeLogEmail("EMAIL Format Preperation Failed Due To No Data Found In Database. AlertTypeId : " + AlertTypeId + " CategoryTypeId : " + CategoryTypeId + " SubCategoryTypeId : " + SubCategoryTypeId + " ClientID : " + ClientID + " UserName : " + UserName + " UserID : " + UserID + " RRN : " + RRN + " Flag : " + Flag);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.writeLogEmail("EMAIL Format Preperation Request Failed. Exception : " + Ex.Message);
                ErrorLog.DBError(Ex);
            }
        }
        #endregion

        #region Insert Email Alert into Table
        public void InsertEmailAlert(string _numClient, string _ClientID, string _RoleID, string _UserID, string _ToEmailIDs, string _CCEmailIDs, string _Subject, string _MailBody, string _Flag, int _issent)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                new SqlParameter("@numClient",_numClient),
                                new SqlParameter("@ClientID",_ClientID),
                                new SqlParameter("@RoleID",_RoleID),
                                new SqlParameter("@UserID",_UserID),
                                new SqlParameter("@Subject",_Subject),
                                new SqlParameter("@SentTo",_ToEmailIDs),
                                new SqlParameter("@SentCC",_CCEmailIDs),
                                new SqlParameter("@IsSent",_issent),
                                new SqlParameter("@MailBody",_MailBody),
                                new SqlParameter("@Username",UserName),
                                new SqlParameter("@Flag",_Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "InsertOrUpdateEmailAlertDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.writeLogEmailError("Class : EmailAlerts.cs \nFunction : InsertEmailAlert() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion
       
        #region SMS Format
        public void PrepareSMSFormat()
        {
            DataSet dataset = new DataSet();
            string _UserID = string.Empty; string _RoleID = string.Empty; string _numClientID = string.Empty; string _ClientID = string.Empty;
            string _ToEmailIDs = string.Empty; string _CCEmailIDs = string.Empty; string _Subject = string.Empty; string _MailBody = string.Empty; string _ContNumber = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        ErrorLog.SMSTrace("SMS Format Preperation Request Received. AlertTypeId : " + AlertTypeId + " CategoryTypeId : " + CategoryTypeId + " SubCategoryTypeId : " + SubCategoryTypeId + " ClientID : " + ClientID + " UserName : " + UserName + " UserID : " + UserID + " RRN : " + RRN + " Flag : " + Flag);
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@AlertTypeId",AlertTypeId),
                            new SqlParameter("@CategoryTypeId",CategoryTypeId),
                            new SqlParameter("@SubCategoryTypeId",SubCategoryTypeId),
                            new SqlParameter("@ClientID",ClientID),
                            new SqlParameter("@Username",UserName),
                            new SqlParameter("@UserID",UserID),
                            new SqlParameter("@RRN",RRN),
                            new SqlParameter("@Flag",Flag),
                            new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                            new SqlParameter("@Status_Out", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "GetSMSDetailsToSend";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        _Status_Out = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                    }
                }

                if (_Status == "00")
                {
                    if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dataset.Tables.Count; i++)
                        {
                            _UserID = dataset.Tables[i].Rows[0]["UserID"].ToString();
                            _RoleID = dataset.Tables[i].Rows[0]["RoleID"].ToString();
                            _numClientID = dataset.Tables[i].Rows[0]["numClientID"].ToString();
                            _ClientID = dataset.Tables[i].Rows[0]["ClientID"].ToString();
                            _ToEmailIDs = dataset.Tables[i].Rows[0]["ToEmails"].ToString();
                            _CCEmailIDs = dataset.Tables[i].Rows[0]["CCEmails"].ToString();
                            _Subject = dataset.Tables[i].Rows[0]["Subject"].ToString();
                            _MailBody = dataset.Tables[i].Rows[0]["Body"].ToString();
                            _ContNumber = dataset.Tables[i].Rows[0]["ContactNumber"].ToString();
                            if (!string.IsNullOrEmpty(_MailBody) && !string.IsNullOrEmpty(_ContNumber))
                            {
                                ErrorLog.SMSTrace("SMS Format Preperation Request Successful. UserId : " + _UserID + " RoleID : " + _RoleID + " numClientID : " + _numClientID + " ClientID : " + _ClientID + " ContNumber : " + _ContNumber + " Subject : " + Subject + " MailBody : " + _MailBody);
                                SendSMS(_MailBody, _ContNumber);
                            }
                            else
                                ErrorLog.SMSTrace("SMS Format Preperation Request Failed. Mandatory Fields Should Not Empty (ContNumber/MailBody)");
                        }
                    }
                    else
                    {
                        ErrorLog.SMSTrace("SMS Format Preperation Failed Due To No Data Found In Database. AlertTypeId : " + AlertTypeId + " CategoryTypeId : " + CategoryTypeId + " SubCategoryTypeId : " + SubCategoryTypeId + " ClientID : " + ClientID + " UserName : " + UserName + " UserID : " + UserID + " RRN : " + RRN + " Flag : " + Flag);
                    }
                }
                else
                {
                    ErrorLog.SMSTrace("SMS Format Preperation Failed Due To No Configuration Found In Database. Resp.Code : "+ _Status+ "Resp.Description : " + _Status_Out+ " AlertTypeId : " + AlertTypeId + " CategoryTypeId : " + CategoryTypeId + " SubCategoryTypeId : " + SubCategoryTypeId + " ClientID : " + ClientID + " UserName : " + UserName + " UserID : " + UserID + " RRN : " + RRN + " Flag : " + Flag);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.SMSTrace("SMS Format Preperation Request Failed. Exception : " + Ex.Message);
                ErrorLog.SMSError(Ex);
            }
        }
        #endregion

        #region SendSMS
        public string SendSMS(string smsText, string sendTo, string alternative_smsText = null)
        {
            string response = string.Empty;
            try
            {
                ErrorLog.SMSTrace("SMS Request Received. sendTo : " + sendTo + ". smsText : " + smsText);
                if (!string.IsNullOrEmpty(sendTo) && !string.IsNullOrEmpty(smsText))
                {
                    string userId = ConfigurationManager.AppSettings["SmsUserId"].ToString();
                    string pwd = ConfigurationManager.AppSettings["SmsPwd"].ToString();
                    StringBuilder postData = new StringBuilder();
                    postData.Append("action=send");
                    postData.Append("&username=" + userId);
                    postData.Append("&passphrase=" + pwd);
                    postData.Append("&phone=" + sendTo);
                    postData.Append("&message=" + smsText);
                    string baseurl = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=sendMessage&send_to=" + sendTo + "&msg=" + smsText + "&userid=" + userId + "&password=" + pwd + "&v=1.1&msg_type=TEXT&auth_scheme=PLAIN";
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    HttpWebRequest request = WebRequest.Create(baseurl) as HttpWebRequest;
                    request.Method = "POST";
                    request.KeepAlive = false;
                    Byte[] byteArray = Encoding.UTF8.GetBytes(baseurl);
                    request.ContentLength = byteArray.Length;
                    request.ContentType = "application/xml";
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    ErrorLog.SMSTrace("SMS Request Sent To smsgupshup. sendTo : " + sendTo + ". smsText : " + smsText);
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = myHttpWebResponse.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                    response = myStreamReader.ReadToEnd();
                    ErrorLog.SMSTrace("SMS Response Received From smsgupshup." + response);
                    myStreamReader.Close();
                    responseStream.Close();
                    myHttpWebResponse.Close();

                    string[] OuterRespDelemeterSplit = new string[] { "success", "error" };
                    string[] InnerRespDelemeterSplit = new string[] { "|" };
                    string[] OuterResponse = response.Count() > 0 ? response.Split(OuterRespDelemeterSplit, StringSplitOptions.RemoveEmptyEntries) : null;
                    string[] InnerResponse = null;
                    foreach (string strResponse in OuterResponse)
                    {
                        try
                        {
                            InnerResponse = OuterResponse.Count() > 0 ? strResponse.Split(InnerRespDelemeterSplit, StringSplitOptions.RemoveEmptyEntries) : null;
                            if (InnerResponse.Count() > 2 && InnerResponse[1].Trim().Length == 12)
                            {
                                InsertEmailAlert(numClient, ClientID, RoleID, UserID, sendTo, (InnerResponse.Count() > 2 ? InnerResponse[1].ToString() : strResponse), smsText, strResponse, "4", 1);
                                ErrorLog.SMSTrace("SMS Sent Success \n Mobile No : " + (InnerResponse.Count() > 2 ? InnerResponse[1].ToString() : strResponse) + " Partner RRN : " + (InnerResponse.Count() > 2 ? InnerResponse[2].ToString() : strResponse) + Environment.NewLine + Environment.NewLine);
                            }
                            else
                            {
                                ErrorLog.SMSTrace("SMS Sent Failed \n Error Code : " + (InnerResponse.Count() > 2 ? InnerResponse[1].ToString() : strResponse) + " Error Message: " + (InnerResponse.Count() > 2 ? InnerResponse[2].ToString() : strResponse) + Environment.NewLine + Environment.NewLine);
                            }
                        }
                        catch (Exception Ex)
                        {
                            ErrorLog.SMSError(Ex);
                        }
                    }
                }
                else
                {
                    ErrorLog.SMSTrace("SMS Send Request Failed. Mandatory Fields Should Not Empty (ContNumber/smsText). ContNumber : " + sendTo + ". smsText : " + smsText);
                    response = "Mandatory Fields Should Not Empty (ContNumber/smsText). ContNumber : " + sendTo + ". smsText : " + smsText;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.SMSTrace("SMS Send Request Failed. Exception : " + Ex.Message + " ContNumber : " + sendTo + ". smsText : " + smsText);
                ErrorLog.SMSError(Ex);
            }
            return response;
        }
        #endregion

        #region GetDetailsToSendSMSEmail
        public string GetDetailsToSendSMSEmail()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                new SqlParameter("@CategoryTypeId",CategoryTypeId),
                                new SqlParameter("@ClientID",ClientID),
                                new SqlParameter("@Flag",Flag),
                                new SqlParameter("@Status", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output }
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GETSMSFormat";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                        return _Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.writeLogEmailError("Class : EmailAlerts.cs \nFunction : GetDetailsToSendSMSEmail() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

    }
}
