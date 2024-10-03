using AppLogger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BussinessAccessLayer
{
    public class EmailSMSAlertscs
    {
        string url = null;

        #region Property Declaration
        public string subuid { get; set; }
        public string dcode { get; set; }
        public string msgtype { get; set; }
        public string intflag { get; set; }
        public string OTPFlag { get; set; }
        public string ctype { get; set; }
        public string userid { get; set; }
        public string pwd { get; set; }
        public string FROM { get; set; }
        public string tempname { get; set; }
        public string to { get; set; }
        public string var1 { get; set; }
        public string var2 { get; set; }
        public string var3 { get; set; }
        public string var4 { get; set; }

        public string DivisionCode { get; set; }
        public string CurrentTimestampInMilliSeconds { get; set; }
        public string IncrementedCounter { get; set; }

        public string ServerId { get; set; }
        public string ServerInstanceId { get; set; }
        #endregion

        public bool ProcessEmail()
        {
            string EmailResponse = string.Empty;
            try
            {
                string Batch_ID = string.Empty;
                DataTable outDataTableFPS = new DataTable();
              
                //ErrorLog.AgentRegsTrace("***--------------------------------***--------------------------------***--------------------------------***--------------------------------***" + Environment.NewLine);
              //  ErrorLog.AgentRegsTrace(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff") + Environment.NewLine + "*** Agent Registration Received For RRN: " + _requestNumber + " ***" + Environment.NewLine);
               
              //  ErrorLog.AgentRegsTrace("***--------------------------------***--------------------------------***--------------------------------***--------------------------------***" + Environment.NewLine);
               // ErrorLog.AgentRegsTrace(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff") + Environment.NewLine + "Agent Registration Request JSONParseData:" + jsonParse + Environment.NewLine);
                try
                {
                    EmailResponse = HttpGetRequest();
                    EmailResponse _EmailResponse= DeserializeObjects<EmailResponse>(EmailResponse);
                    if (_EmailResponse.DivisionCode == "00")
                    {
                        DivisionCode = _EmailResponse.DivisionCode;
                        CurrentTimestampInMilliSeconds = _EmailResponse.CurrentTimestampInMilliSeconds;
                        IncrementedCounter = _EmailResponse.IncrementedCounter;
                        ServerId = _EmailResponse.ServerId;
                        ServerInstanceId = _EmailResponse.ServerInstanceId;

                        return true;
                    }
                    else
                    {
                        DivisionCode = _EmailResponse.DivisionCode;
                        CurrentTimestampInMilliSeconds = _EmailResponse.CurrentTimestampInMilliSeconds;
                        IncrementedCounter = _EmailResponse.IncrementedCounter;
                        ServerId = _EmailResponse.ServerId;
                        ServerInstanceId = _EmailResponse.ServerInstanceId;
                        return false; //false
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLog.AgentRegsError(ex);
                 
                    return false;
                }
            }
            catch (Exception ex)
            {
               // ErrorLog.AgentRegsError(ex);
                
                return false;
            }
        }

        public string HttpGetRequest()
        {
            string strResponse = string.Empty;
            try
            {
                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequest() => Send SMS Process Started. : ");
                ////http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?dcode=UAT&subuid=UAT&msgtype=E&intflag=1&OTPFlag=1&ctype=1&userid=UAT&pwd=XXXXXX&FROM=info@sbmbank.co.in&tempname=XXXX&to=XXXXX&var1=XXXX&var2=XXXX&var3=XXXX&var4=XXXX
                ///
                ///string requestUrl = "http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?dcode=UAT&subuid=UAT&msgtype=E&intflag=1&OTPFlag=1&ctype=1&userid=UAT&pwd=XXXXXX&FROM=info@sbmbank.co.in&tempname=XXXX&to=XXXXX&var1=XXXX&var2=XXXX&var3=XXXX&var4=XXXX";

                //string requestUrll = "http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?dcode=UAT&subuid=UAT&pwd=XXXXX&msgtype=S&from=UAT&to=9190000000000&tempname=XXXXX&intflag=1&OTPFlag=0&ctype=1&var1=XXXX&var4=XXXX";
                // string requestUrl = "http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?";

                // string requestUrl = "http://100.112.5.78:8181/publisher/http6listener/dynamic?";

                string requestUrl = ConfigurationManager.AppSettings["requestUrl"];
                dcode = ConfigurationManager.AppSettings["dcode"];
                //subuid = ConfigurationManager.AppSettings["subuid"];
                userid = ConfigurationManager.AppSettings["userid"];
                pwd = ConfigurationManager.AppSettings["pwd"];
                msgtype = "S";
                ctype = "1";
                intflag = "2";
                //    string requestUrl = ConfigurationManager.AppSettings["FPSJSONURL"];
                //url = requestUrl + dcode + "&subuid=" + subuid + "&msgtype=" + msgtype + "&intflag=" + intflag + "&OTPFlag=" + OTPFlag + "&ctype=" + ctype + "&userid=" + userid + "&pwd=" + pwd + "&FROM="
                //    + FROM + "&tempname=" + tempname + "&to=" + to + "&var1=" + var1 + "&var2=" + var2 + "&var3=" + var3 + "&var4=" + var4 + "";\

                url = requestUrl + "dcode=" + dcode + "&subuid=" + userid + "&pwd=" + pwd + "&msgtype=" + msgtype + "&from=" + FROM + "&to=" + to
                    + "&intflag=" + intflag + "&OTPFlag=" + OTPFlag + "&ctype=" + ctype + "&tempname=" + tempname + "&var1=" + var1 + "&var2=" + var2;

                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequest() => URL : " + url);

                System.Net.ServicePointManager.Expect100Continue = true;
                ServicePointManager.MaxServicePointIdleTime = 1000;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                WebRequest myWebRequest = WebRequest.Create(url);
                //   myWebRequest.Headers.Add(Key, Value);
                //    myWebRequest.Headers.Add(Key1, Value1);

                HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                int statuscode = (int)myWebResponse.StatusCode;
                string ErrorMsg = myWebResponse.StatusDescription;
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                strResponse = readStream.ReadToEnd();
                readStream.Close();
                ReceiveStream.Close();
                myWebResponse.Close();
                // _FPSEntity.InsertRequestResponse(BatchID, url, strResponse, statuscode, ErrorMsg);
                //_SystemLogger.WriteTransLog(this, "FPS JSON  Response Received  " + Environment.NewLine + strResponse + Environment.NewLine + " BatchID : " + BatchID + Environment.NewLine);
            }
            catch (WebException Ex)
            {
                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequest() => Exception : " + Ex.Message);
                HttpWebResponse mywebresponse = (HttpWebResponse)Ex.Response;
                int statuscode = (int)mywebresponse.StatusCode;
                string ErrorMsg = mywebresponse.StatusDescription;
                // _FPSEntity.InsertRequestResponse(BatchID, url, strResponse, statuscode, ErrorMsg);
                // _SystemLogger.WriteErrorLog(this, Ex);
                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequest() => Exception : " + Ex.Message);
                ErrorLog.SMSError(Ex);
            }
            ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequest() => SMS Response : " + strResponse);
            return strResponse;
        }

        public string HttpGetRequestEmail()
        {
            string strResponse = string.Empty;
            try
            {
                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequestEmail() => Send Email Process Started. : ");
                ////http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?dcode=UAT&subuid=UAT&msgtype=E&intflag=1&OTPFlag=1&ctype=1&userid=UAT&pwd=XXXXXX&FROM=info@sbmbank.co.in&tempname=XXXX&to=XXXXX&var1=XXXX&var2=XXXX&var3=XXXX&var4=XXXX
                ///
                ///string requestUrl = "http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?dcode=UAT&subuid=UAT&msgtype=E&intflag=1&OTPFlag=1&ctype=1&userid=UAT&pwd=XXXXXX&FROM=info@sbmbank.co.in&tempname=XXXX&to=XXXXX&var1=XXXX&var2=XXXX&var3=XXXX&var4=XXXX";

                //string requestUrll = "http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?dcode=UAT&subuid=UAT&pwd=XXXXX&msgtype=S&from=UAT&to=9190000000000&tempname=XXXXX&intflag=1&OTPFlag=0&ctype=1&var1=XXXX&var4=XXXX";
                // string requestUrl = "http://uatsms.finfisher.co.in:8181/publisher/http6listener/dynamic?";

                // string requestUrl = "http://100.112.5.78:8181/publisher/http6listener/dynamic?";

                string requestUrl = ConfigurationManager.AppSettings["requestUrl"];
                dcode = ConfigurationManager.AppSettings["dcode"];
                //subuid = ConfigurationManager.AppSettings["subuid"];
                userid = ConfigurationManager.AppSettings["userid"];
                pwd = ConfigurationManager.AppSettings["pwd"];
                msgtype = "E";
                ctype = "1";
                intflag = "2";
                //    string requestUrl = ConfigurationManager.AppSettings["FPSJSONURL"];
                //url = requestUrl + dcode + "&subuid=" + subuid + "&msgtype=" + msgtype + "&intflag=" + intflag + "&OTPFlag=" + OTPFlag + "&ctype=" + ctype + "&userid=" + userid + "&pwd=" + pwd + "&FROM="
                //    + FROM + "&tempname=" + tempname + "&to=" + to + "&var1=" + var1 + "&var2=" + var2 + "&var3=" + var3 + "&var4=" + var4 + "";\

                //url = requestUrl + dcode + "&subuid=" + userid + "&pwd=" + pwd + "&msgtype=" + msgtype + "&FROM=" + FROM + "&to=" + to + "&tempname=" + tempname
                //    + "&intflag=" + intflag + "&OTPFlag=" + OTPFlag + "&ctype=" + ctype + "&var1=" + var1 + "&var2=" + var2;

                url = requestUrl + "dcode=" + dcode + "&subuid=" + userid + "&pwd=" + pwd + "&msgtype=" + msgtype + "&from=" + FROM + "&to=" + to
                    + "&intflag=" + intflag + "&OTPFlag=" + OTPFlag + "&ctype=" + ctype + "&tempname=" + tempname + "&var1=" + var1 + "&var2=" + var2;

                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequestEmail() => URL : " + url);

                System.Net.ServicePointManager.Expect100Continue = true;
                ServicePointManager.MaxServicePointIdleTime = 1000;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                WebRequest myWebRequest = WebRequest.Create(url);
                //   myWebRequest.Headers.Add(Key, Value);
                //    myWebRequest.Headers.Add(Key1, Value1);

                HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                int statuscode = (int)myWebResponse.StatusCode;
                string ErrorMsg = myWebResponse.StatusDescription;
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                strResponse = readStream.ReadToEnd();
                readStream.Close();
                ReceiveStream.Close();
                myWebResponse.Close();
                // _FPSEntity.InsertRequestResponse(BatchID, url, strResponse, statuscode, ErrorMsg);
                //_SystemLogger.WriteTransLog(this, "FPS JSON  Response Received  " + Environment.NewLine + strResponse + Environment.NewLine + " BatchID : " + BatchID + Environment.NewLine);
            }
            catch (WebException Ex)
            {
                //ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequest() => Exception : " + Ex.Message);
                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequestEmail() => Exception : " + Ex.Message);
                HttpWebResponse mywebresponse = (HttpWebResponse)Ex.Response;
                int statuscode = (int)mywebresponse.StatusCode;
                string ErrorMsg = mywebresponse.StatusDescription;
                // _FPSEntity.InsertRequestResponse(BatchID, url, strResponse, statuscode, ErrorMsg);
                // _SystemLogger.WriteErrorLog(this, Ex);
                ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequestEmail() => Exception : " + Ex.Message);
                ErrorLog.SMSError(Ex);
            }
            ErrorLog.SMSTrace("Page : EmailSMSAlertscs.cs \nFunction : HttpGetRequestEmail() => Email Response : " + strResponse);
            return strResponse;
        }
        public class EmailResponse
        {
            [XmlElement(ElementName = "DivisionCode")]
            public string DivisionCode { get; set; }

            [XmlElement(ElementName = "CurrentTimestampInMilliSeconds")]
            public string CurrentTimestampInMilliSeconds { get; set; }

            [XmlElement(ElementName = "IncrementedCounter")]
            public string IncrementedCounter { get; set; }

            [XmlElement(ElementName = "ServerId")]
            public string ServerId { get; set; }

            [XmlElement(ElementName = "ServerInstanceId")]
            public string ServerInstanceId { get; set; }
        }

        public T DeserializeObjects<T>(string xml) where T : new()
        {
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception Ex)
            {
                return new T();
            }
        }

    }
}
