using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BussinessAccessLayer
{
    public class CommonEntity
    {
        ConfigurationData _ConfigurationData = new ConfigurationData();
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public static string GetResponseCode(string HostResponseCode, int Source)
        {
            string RespCode = (from RCode in ResponceCodes.AsParallel()
                               where RCode.ResponseCodeHOST == HostResponseCode && RCode.Source == Source
                               select RCode.ResponseCode).FirstOrDefault();

            if (string.IsNullOrEmpty(RespCode))
                RespCode = "91";

            return RespCode;
        }

        public static string GetResponseCodeDescription(string _responseCode, int Source)
        {
            string Msg = string.Empty;
            try
            {
                Msg = (from RCode in ResponceCodes.AsParallel()
                       where RCode.ResponseCodeHOST == _responseCode && RCode.Source == Source
                       select RCode.ErrorMessage).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            if (string.IsNullOrEmpty(Msg))
                Msg = "UNABLE TO PROCESS";

            return Msg;
        }
        private static ConfigurationData _dalConfigurationData;
        public static ConfigurationData DalConfigurationData
        {
            get
            {
                if (_dalConfigurationData == null)
                    _dalConfigurationData = new ConfigurationData();
                return _dalConfigurationData;
            }
            set { _dalConfigurationData = value; }
        }
        static List<ResponseCodeDetails> _responceCodes = null;
        public static List<ResponseCodeDetails> ResponceCodes
        {
            get
            {
                if (_responceCodes == null)
                {
                    try
                    {
                        _responceCodes = new List<ResponseCodeDetails>();
                        DataTable DTResponseCodes = DalConfigurationData.SelectResponseCodes();
                        foreach (DataRow row in DTResponseCodes.Rows)
                        {
                            ResponseCodeDetails _responseCode = new ResponseCodeDetails()
                            {
                                ResponseCode = row[0].ToString(),
                                ResponseCodeHOST = row[1].ToString(),
                                ErrorMessage = row[2].ToString(),
                                Source = Convert.ToInt16(row[3].ToString()),
                            };
                            _responceCodes.Add(_responseCode);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return _responceCodes;
            }
            set { _responceCodes = value; }
        }
        public class ResponseCodeDetails
        {
            public string ResponseCode = string.Empty;
            public string ErrorMessage = string.Empty;
            public string ResponseCodeHOST = string.Empty;
            public int Source;
        }
    }

    public class ConfigurationData
    {

        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        DataSet dataSet = null;
        string OutStatus = null; string OutStatusMsg = null;
        #endregion

        #region "ResponseCodes"

        public DataTable SelectResponseCodes()
        {
            DataTable DThostmessagecodes = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("GetMATMResponseCodes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Fill DataTable with the result of the procedure
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command))
                        {
                            dataAdapter.Fill(DThostmessagecodes);
                        }
                    }
                    connection.Close();
                }
                return DThostmessagecodes;
            }
            catch (Exception Ex)
            {
                DThostmessagecodes = null;
                return DThostmessagecodes;
            }
        }
        #endregion "ResponseCodes"
    }

    [Serializable]
    [XmlRoot("ConstResponseCodes")]
    public class ConstResponseCodes
    {
        public const string Success = "00";
        public const string Fail = "91";
        public const string Insert = "INS00";
        public const string InsertFail = "INS96";
        public const string Update = "UPD00";
        public const string UpdateFail = "UPD96";
        public const string FlagNotExist = "FLG96";
        public const string IsVerfiy = "VERI00";
        public const string Delete = "DEL00";
        public const string DeleteFail = "DEL96";
    }
}
