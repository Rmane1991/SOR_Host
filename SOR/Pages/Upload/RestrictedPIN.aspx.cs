﻿using AppLogger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessAccessLayer;

namespace SOR.Pages.Upload
{
    public partial class RestrictedPIN : System.Web.UI.Page
    {
        #region ConnectionString

        //SystemLogger _systemLogger = new SystemLogger();
        ImportEntity importEntity = new ImportEntity();
        int _CmdTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
        //public ExcelHelper.ExportFormat exportFormat = new ExcelHelper.ExportFormat();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion

        #region property declaration
        UploadDAL _UploadDAL = new UploadDAL();
        DataTable TypeTableUploadDetails = null; // typetbale paramerters same in database   

        string filePath = null;
        int _successful = 0,
            _unsuessful = 0,
        UploadRestrictedPINFields = 6, _dupRcdsCnt = 0;


        string FilePath = string.Empty;
        string BankCode = string.Empty;
        string UserName = string.Empty;
        string ImportFileName = string.Empty;

        StringBuilder ConnctionString = null;
        StringBuilder _ConnctionString
        {
            get { if (ConnctionString == null) ConnctionString = new StringBuilder(); return ConnctionString; }
            set { ConnctionString = value; }
        }

        OleDbConnection _OleDbConnection = null;


        clsCustomeRegularExpressions _CustomeRegExpValidation = null; //regular expression format and enums for regular-expressions

        #endregion

        #region Constructor
        public RestrictedPIN()
        {
            try
            {
                TypeTableUploadDetails = new DataTable();
                if (TypeTableUploadDetails.Columns.Count == 0)
                {
                    TypeTableUploadDetails.Columns.Add("City");
                    TypeTableUploadDetails.Columns.Add("State");
                    TypeTableUploadDetails.Columns.Add("PinCode");
                    TypeTableUploadDetails.Columns.Add("DateOfBlackListing");
                    TypeTableUploadDetails.Columns.Add("ReasonForBlackListing");
                    TypeTableUploadDetails.Columns.Add("IsRemoved");
                    TypeTableUploadDetails.Columns.Add("RecordStatus");
                    TypeTableUploadDetails.Columns.Add("RecordStatusDescription");
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN: RestrictedPIN(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "RestrictedPIN.aspx", "33");
                    if (!HasPagePermission)
                    {
                        try
                        {
                            Response.Redirect(ConfigurationManager.AppSettings["RedirectTo404"].ToString(), false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        catch (ThreadAbortException)
                        {
                        }
                    }
                    else
                    {
                        if (!IsPostBack && HasPagePermission)
                        {
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            UserPermissions.RegisterStartupScriptForNavigationListActive("10", "33");
                            FillGrid(EnumCollection.EnumBindingType.BindGrid);
                        }
                    }
                }
                else
                {
                    try
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["RedirectToLogin"].ToString(), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                }
                ErrorLog.UploadTrace("RestrictedPIN | Page_Load() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN: Page_Load(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }

        #region Fillgrid
        public DataSet FillGrid(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet ds = new DataSet();
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvRestrictedPIN.DataSource = null;
                gvRestrictedPIN.DataBind();
                SetPropertise(ref importEntity);
                ds = importEntity.ExportRestrictedPIN();

                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = ds.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvRestrictedPIN.DataSource = dv;
                            gvRestrictedPIN.DataBind();
                            gvRestrictedPIN.Visible = true;
                        }
                        else
                        {
                            gvRestrictedPIN.DataSource = ds.Tables[0];
                            gvRestrictedPIN.DataBind();
                            gvRestrictedPIN.Visible = true;
                        }
                    }
                    else
                    {
                        gvRestrictedPIN.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "alert('No Data Found in Search Criteria. Try again', 'Warning');", true);
                    }
                }
                ErrorLog.UploadTrace("RestrictedPIN | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN: FillGrid(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
            }
            return ds;
        }

        #endregion

        #region Sorting
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        #endregion

        #region setproperty
        private void SetPropertise(ref ImportEntity importEntity)
        {
            try
            {
                importEntity.UserName = Session["Username"].ToString();
                importEntity._FromDate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                importEntity._ToDate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                importEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                importEntity.FileDescID = importEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadRestrictedPIN);
                importEntity.FileStatus = (ddlFileTypeStatus.SelectedValue.ToString() != null && ddlFileTypeStatus.SelectedValue.ToString() != "" ? ddlFileTypeStatus.SelectedValue.ToString() : "0");
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN: SetPropertise(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                throw;
            }
        }
        #endregion

        #region  button click 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | btnSave_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Upload-RestrictedPIN";
                _auditParams[2] = "btnSave";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                Save();
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.UploadTrace("RestrictedPIN | btnSave_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN: btnSave_Click(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
            }
        }
        private void Save()
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | Save() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadTrace("Process Request Recieved for Upload RestrictedPIN File");
                string TotalRecords = null;
                string _FileImport = fileUpload.FileName;
                string _fileExtension = Path.GetExtension(_FileImport);
                DataTable dtExcelData = new DataTable();
                if (_FileImport != "" && fileUpload.HasFile)
                {
                    if (ValidateFileFormat(_fileExtension))
                    {
                        filePath = null; string FileId = string.Empty; ;
                        filePath = SaveFile(fileUpload);
                        FileValidator _FileValidator = new FileValidator();
                        _FileValidator.FileName = _FileImport;
                        _FileValidator.FileDateTime = txtDate.Value.Trim();
                        _FileValidator.FilePath = filePath;
                        _FileValidator.FileType = _fileExtension;
                        _FileValidator.ClientID = null;
                        _FileValidator.Username = Convert.ToString(Session["Username"]);
                        _FileValidator.FileDateTime = DateTime.Now.ToString();
                        if (FileImportEntry(_FileValidator, out FileId))
                        {
                            if (ProcessFile(_fileExtension, filePath, out dtExcelData))
                            {
                                for (int rows = 0; rows < dtExcelData.Rows.Count; rows++)
                                {
                                    string StatusCode = null;
                                    string StatusDesc = null;
                                    ImportEntity _ImportEntity = new ImportEntity();
                                    _ImportEntity.City = dtExcelData.Rows[rows][0].ToString() != null && dtExcelData.Rows[rows][0].ToString() != "" ? dtExcelData.Rows[rows][0].ToString() : null;
                                    _ImportEntity.State = dtExcelData.Rows[rows][1].ToString() != null && dtExcelData.Rows[rows][1].ToString() != "" ? dtExcelData.Rows[rows][1].ToString() : null;
                                    _ImportEntity.PINCODE = dtExcelData.Rows[rows][2].ToString() != null && dtExcelData.Rows[rows][2].ToString() != "" ? dtExcelData.Rows[rows][2].ToString() : null;
                                    _ImportEntity.DateOfBlackListing = dtExcelData.Rows[rows][3].ToString() != null && dtExcelData.Rows[rows][3].ToString() != "" ? Convert.ToDateTime(dtExcelData.Rows[rows][3]).ToString("yyyy-MM-dd") : null;
                                    _ImportEntity.ReasonForBlackListing = dtExcelData.Rows[rows][4].ToString() != null && dtExcelData.Rows[rows][4].ToString() != "" ? dtExcelData.Rows[rows][4].ToString() : "0";
                                    _ImportEntity.IsRemoved = dtExcelData.Rows[rows][5].ToString() != null && dtExcelData.Rows[rows][5].ToString() != "" ? dtExcelData.Rows[rows][5].ToString() : "0";

                                    if (ValidateFileData(_ImportEntity, out StatusCode, out StatusDesc))
                                    {
                                        _successful += 1;
                                        _ImportEntity.RecordStatusDescription = StatusDesc;
                                        _ImportEntity.RecordStatus = "1";
                                        _ImportEntity.IsValidRecord = 1;
                                        _ImportEntity.RecordID = rows;
                                        AddRowToDataTable(_ImportEntity);
                                    }
                                    else
                                    {
                                        _unsuessful += 1;
                                        _ImportEntity.RecordStatusDescription = StatusDesc;
                                        _ImportEntity.RecordStatus = "0";
                                        _ImportEntity.IsValidRecord = 0;
                                        _ImportEntity.RecordID = rows;
                                        AddRowToDataTable(_ImportEntity);
                                    }
                                }
                                if (InsertFileData(FileId, Session["Username"].ToString(), TypeTableUploadDetails))
                                {
                                    _successful = _successful - _dupRcdsCnt;
                                    _unsuessful = _unsuessful + _dupRcdsCnt;
                                    if (_unsuessful == 0)
                                    {
                                        TotalRecords = Convert.ToString(_successful + _unsuessful);
                                        FillGrid(EnumCollection.EnumBindingType.BindGrid);

                                        ErrorLog.UploadTrace("Page : RestrictedPIN.aspx.cs Function : btnSave_Click() :" + " Total record processed : " + TotalRecords + " Successful : " + _successful + " Unsuccessful : " + _unsuessful);
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showSuccess('Total record processed : " + TotalRecords + " Successful : " + _successful + " Unsuccessful : " + _unsuessful + " ', 'Successful');</script>", false);
                                        return;
                                    }
                                    else if (_unsuessful > 0)
                                    {
                                        TotalRecords = Convert.ToString(_successful + _unsuessful);
                                        FillGrid(EnumCollection.EnumBindingType.BindGrid);
                                        ErrorLog.UploadTrace("Page : RestrictedPIN.aspx.cs Function : btnSave_Click() :" + " Total record processed : " + TotalRecords + " Successful : " + _successful + " Unsuccessful : " + _unsuessful);
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Total record processed : " + TotalRecords + " Valid records : " + _successful + " | Invalid records : " + _unsuessful + " ');</script>", false);
                                        return;
                                    }
                                }
                                else
                                {
                                    ErrorLog.UploadTrace("RestrictedPIN: btnSave_Click: Unable To Insert Upload Details");
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable To Process Details Or Invalid data.', 'Warning');", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('System does not supports " + _fileExtension + " file format.','Warning');</script>", false);
                                ErrorLog.UploadTrace("Page : RestrictedPIN.aspx.cs Function :btnSave_Click() : " + "System does not supports " + _fileExtension + " file format.");
                                return;
                            }
                        }
                        else
                        {
                            ErrorLog.UploadTrace("RestrictedPIN: btnSave_Click: Unable To Insert Upload Details");
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable To Process Data. Try again', 'Warning');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid file .</br> Extension of file should be .xls or .xlsx only.');</script>", false);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please select the file to upload .','Warning');</script>", false);
                    return;
                }
                ErrorLog.UploadTrace("RestrictedPIN | Save() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace(" Failed for Save():" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #region Insert
        public bool InsertFileData(string FileID, string Username, DataTable dtTable)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | InsertFileData() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                importEntity.FileID = FileID;
                importEntity.Dtable = dtTable;
                importEntity.UserName = Username;
                importEntity._FromDate = DateTime.Now.ToString();
                importEntity.FileDate = txtDate.Value;
                string Status = string.Empty; string StatusMsg = string.Empty;
                importEntity.InsertBulkTerminalDetailsPIN(dtTable, out Status, out StatusMsg, FileID);
                if (Status == "00" || Status == "02")
                {
                    ErrorLog.UploadTrace(string.Format("Successful Insert File Data Request For Upload RestrictedPIN. Username : {0}. FileId : {1}. Status : {2} StatusMsg : {3}", UserName, FileID, Status, StatusMsg));
                    return true;
                }
                else
                {
                    ErrorLog.UploadTrace(string.Format("Failed Insert File Data Request For Upload RestrictedPIN. Username : {0}. FileId : {1}. Status : {2} StatusMsg : {3}", UserName, FileID, Status, StatusMsg));
                    return false;
                }
                //ErrorLog.UploadTrace("RestrictedPIN | InsertFileData() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }

            catch (Exception Ex)
            {
                ErrorLog.UploadTrace(string.Format("Failed Insert File Data Request For Upload RestrictedPIN. Username : {0}. FileId : {1}. Exception : {2}", UserName, FileID, Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString()));
                ErrorLog.UploadError(Ex);
                return false;
            }
        }
        #endregion

        public void AddRowToDataTable<T>(T TObj)
        {
            bool contains;

            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | AddRowToDataTable() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ImportEntity _FileIpmort = (ImportEntity)(object)TObj;
                ErrorLog.UploadTrace(string.Format("Initiated Insert Add Row To DataTable Request. AgentName : {0}. Address : {1}. City : {2}. State : {3}. PinCode : {4}. AadhaarNo : {5}. PANNo : {6}. MobileNo : {7}. DateOfBlackListing : {8}. ReasonForBlackListing : {9}. CorporateBCName : {10}. BankName : {11}. IFSCCode : {12}. AccountNo : {13}. IsPoliceComplaint : {14}. IfFIRCompliant : {15}. DateofComplaint : {16}. IsBCAgentArrested : {17}. IsRemoved : {18}. IsvalidRecord : {19}. RecordStatus : {20}. FileStatus : {21}. RecordID : {22}. StatusDescription : {23}. FileId. ", _FileIpmort.AgentName, _FileIpmort.Address, _FileIpmort.City, _FileIpmort.State, _FileIpmort.PINCODE, _FileIpmort.AadhaarNo, _FileIpmort.PANNo, _FileIpmort.MobileNo, _FileIpmort.DateOfBlackListing, _FileIpmort.ReasonForBlackListing, _FileIpmort.CorporateBCName, _FileIpmort.BankName, _FileIpmort.IFSCCode, _FileIpmort.AccountNo, _FileIpmort.IsPoliceComplaint, _FileIpmort.IfFIRCompliant, _FileIpmort.DateofComplaint, _FileIpmort.IsBCAgentArrested, _FileIpmort.IsRemoved, _FileIpmort.IsValidRecord, _FileIpmort.RecordStatus, _FileIpmort.FileStatus, _FileIpmort.RecordID, _FileIpmort.RecordStatus, _FileIpmort.FileID));
                DataRow _RowTypeTable = TypeTableUploadDetails.NewRow();
                _RowTypeTable["City"] = _FileIpmort.City;
                _RowTypeTable["State"] = _FileIpmort.State;
                _RowTypeTable["PinCode"] = _FileIpmort.PINCODE;
                _RowTypeTable["DateOfBlackListing"] = _FileIpmort.DateOfBlackListing;
                _RowTypeTable["ReasonForBlackListing"] = _FileIpmort.ReasonForBlackListing;
                _RowTypeTable["IsRemoved"] = _FileIpmort.IsRemoved;

                //string ATMID = _FileIpmort.ATMID;
                //contains = TypeTableUploadDetails.AsEnumerable().Any(row => ATMID == row.Field<String>("ATMID"));

                //if (contains && _FileIpmort.RecordStatus == "1")
                //{
                //    _FileIpmort.RecordStatus = "0";
                //    _FileIpmort.IsValidRecord = 0;
                //    _FileIpmort.RecordStatusDescription = "Duplicate Reord";
                //    _dupRcdsCnt = _dupRcdsCnt + 1;
                //}
                _RowTypeTable["RecordStatus"] = _FileIpmort.RecordStatus;
                _RowTypeTable["RecordStatusDescription"] = _FileIpmort.RecordStatusDescription;
                TypeTableUploadDetails.Rows.Add(_RowTypeTable);
                ErrorLog.UploadTrace("RestrictedPIN | AddRowToDataTable() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadTrace(string.Format("Completed Insert Add Row To DataTable Request. AgentName : {0}. Address : {1}. City : {2}. State : {3}. PinCode : {4}. AadhaarNo : {5}. PANNo : {6}. MobileNo : {7}. DateOfBlackListing : {8}. ReasonForBlackListing : {9}. CorporateBCName : {10}. BankName : {11}. IFSCCode : {12}. AccountNo : {13}. IsPoliceComplaint : {14}. IfFIRCompliant : {15}. DateofComplaint : {16}. IsBCAgentArrested : {17}. IsRemoved : {18}. IsvalidRecord : {19}. RecordStatus : {20}. FileStatus : {21}. RecordID : {22}. StatusDescription : {23}. FileId. ", _FileIpmort.AgentName, _FileIpmort.Address, _FileIpmort.City, _FileIpmort.State, _FileIpmort.PINCODE, _FileIpmort.AadhaarNo, _FileIpmort.PANNo, _FileIpmort.MobileNo, _FileIpmort.DateOfBlackListing, _FileIpmort.ReasonForBlackListing, _FileIpmort.CorporateBCName, _FileIpmort.BankName, _FileIpmort.IFSCCode, _FileIpmort.AccountNo, _FileIpmort.IsPoliceComplaint, _FileIpmort.IfFIRCompliant, _FileIpmort.DateofComplaint, _FileIpmort.IsBCAgentArrested, _FileIpmort.IsRemoved, _FileIpmort.IsValidRecord, _FileIpmort.RecordStatus, _FileIpmort.FileStatus, _FileIpmort.RecordID, _FileIpmort.RecordStatus, _FileIpmort.FileID));
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Failed for AddRowToDataTable:" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
            }
        }

        public bool ProcessFile(string FileExtension, string FilePath, out DataTable dataTableExcel)
        {
            ErrorLog.UploadTrace("RestrictedPIN | ProcessFile() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            bool IsFileProcessed = true;
            dataTableExcel = new DataTable();
            ErrorLog.UploadTrace(string.Format("Initiated Process File Request. Username : {0}. FilePath : {1}.FileExtension : {2}.", Session["Username"].ToString(), FilePath, FileExtension));
            if (FileExtension.ToLower() == ".xls")
            {
                _ConnctionString.Append("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath.ToString() + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            }
            else if (FileExtension.ToLower() == ".xlsx")
            {
                _ConnctionString.Append("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath.ToString() + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
            }
            try
            {
                _OleDbConnection = new OleDbConnection(_ConnctionString.ToString());
                OleDbCommand cmd;
                _OleDbConnection.Open();
                cmd = new OleDbCommand("select * from [BulkData$]", _OleDbConnection);
                DataTable dtExcelSheetName1 = _OleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName1.Rows[0]["Table_Name"].ToString();
                cmd = new OleDbCommand("select * from [" + getExcelSheetName + "]", _OleDbConnection);
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);

                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dataTableExcel);

                if (_OleDbConnection.State == ConnectionState.Open) _OleDbConnection.Close();
                cmd.Dispose();
                dAdapter.Dispose();
                UploadRestrictedPINFields = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["UploadRestrictedPINFields"].ToString()) ? Convert.ToInt32(ConfigurationManager.AppSettings["UploadRestrictedPINFields"].ToString()) : 0;
                if (dataTableExcel.Rows.Count > 0)
                {
                    if (UploadRestrictedPINFields != dataTableExcel.Columns.Count)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please check the number of fields in the file.','File Format');</script>", false);
                        IsFileProcessed = false;

                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please check the number of fields in the file.','File Format');</script>", false);
                    IsFileProcessed = true;
                }
                ErrorLog.UploadTrace("RestrictedPIN | ProcessFile() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadTrace(string.Format("Completed Process File Request. Username : {0}. FilePath : {1}. FileExtension : {2}.", Session["Username"].ToString(), FilePath, FileExtension));
            }
            catch (ArgumentException Ex)
            {
                ErrorLog.UploadError(Ex);
                ErrorLog.UploadTrace(string.Format("Failed Process File Request. Username : {0}. FilePath : {1}. Status : {2}", Session["Username"].ToString(), FilePath, Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString()));
                IsFileProcessed = false;
            }
            catch (OleDbException Ex)
            {
                ErrorLog.UploadError(Ex);
                ErrorLog.UploadTrace(string.Format("Failed Process File Request. Username : {0}. FilePath : {1}. Status : {2}", Session["Username"].ToString(), FilePath, Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString()));
                IsFileProcessed = false;
            }
            catch (InvalidOperationException Ex)
            {
                IsFileProcessed = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('System does not supports " + FileExtension + " file format.','Warning');</script>", false);
                ErrorLog.UploadTrace("RestrictedPIN : Error At ProcessFile(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
            }
            return IsFileProcessed;

        }

        public bool FileImportEntry(FileValidator _FileValidator, out string FileId)
        {
            FileId = string.Empty;
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | FileImportEntry() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                importEntity.FileName = _FileValidator.FileName;
                importEntity.FilePath = _FileValidator.FilePath;
                importEntity.FileType = _FileValidator.FileType;
                importEntity.UserName = _FileValidator.Username;
                importEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadRestrictedPIN);
                importEntity.FileDescName = EnumCollection.EnumFileDesciption.UploadRestrictedPIN.ToString();

                string Status = importEntity.InsertFileImport(out FileId);
                ErrorLog.UploadTrace("RestrictedPIN | FileImportEntry() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                return Status == "00" ? true : false;
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At FileImportEntry(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                return false;
            }
        }

        #region ValidateFileFormat

        public bool ValidateFileFormat(string FileExtension)
        {
            try
            {
                ErrorLog.UploadTrace(string.Format("Initiated Validate File Request. Username : {0}.", Session["Username"].ToString()));
                return FileExtension.ToUpper() != ".XLS" && FileExtension.ToUpper() != ".XLSX" ? false : true;
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At ValidateFileFormat(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                return false;
            }
        }

        #endregion


        #region Save File
        public string SaveFile(FileUpload fileUpload)
        {
            try
            {
                ErrorLog.UploadTrace(string.Format("Initiated Save File Request. Username : {0}. FileName: {1}.", Session["Username"].ToString(), fileUpload.PostedFile.FileName));
                string PathLocation = ConfigurationManager.AppSettings["UploadRestrictedPIN"].ToString();

                ErrorLog.UploadTrace("PathLocation: " + PathLocation);
                if (!Directory.Exists(PathLocation))
                {
                    Directory.CreateDirectory(PathLocation);
                }
                PathLocation += fileUpload.PostedFile.FileName;

                if (File.Exists(PathLocation))
                {
                    File.Delete(PathLocation);
                    fileUpload.SaveAs(PathLocation);
                    ErrorLog.UploadTrace(string.Format("Completed Save File Request. Username : {0}. FileName: {1}.", Session["Username"].ToString(), fileUpload.PostedFile.FileName));
                }
                else
                {
                    fileUpload.SaveAs(PathLocation);
                    ErrorLog.UploadTrace(string.Format("Completed Save File Request. Username : {0}. FileName: {1}.", Session["Username"].ToString(), fileUpload.PostedFile.FileName));
                }
                return PathLocation;

            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At SaveFile(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                return string.Empty;
            }
        }
        #endregion

        protected void btnsample_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | btnsample_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadTrace(string.Format("Initiated Ticket Upload Sample File Download Request."));
                string strURL = string.Empty;


                strURL = "~/ExportedExcelFiles/RestrictedPIN.xls";
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=RestrictedPIN.xls");
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
                ErrorLog.UploadTrace(string.Format("Completed RestrictedPIN Sample File Download."));
                ErrorLog.UploadTrace("RestrictedPIN | btnsample_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (System.Threading.ThreadAbortException Ex)
            {
                ErrorLog.UploadTrace(string.Format("Error RestrictedPIN Sample File Download." + " | LoginKey : " + Session["LoginKey"].ToString()));
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | btnsearch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (string.IsNullOrEmpty(txtFromDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select From Date.', 'Warning');</script>", false);
                    return;
                }
                if (string.IsNullOrEmpty(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select To Date.', 'Warning');</script>", false);
                    return;
                }

                else
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Upload-RestrictedPIN";
                    _auditParams[2] = "btnsearch";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
                ErrorLog.UploadTrace("RestrictedPIN | btnsearch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }

            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At btnsearch_Click(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void Btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | Btnclear_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Upload-RestrictedPIN";
                _auditParams[2] = "Btnclear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlFileTypeStatus.ClearSelection();
                gvRestrictedPIN.Visible = false;
                ErrorLog.UploadTrace("RestrictedPIN | Btnclear_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At Btnclear_Click(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        #endregion

        #region Gridview Events
        protected void gvRestrictedPIN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ExportFormat obj = new ExportFormat();
            try
            {
                if (e.CommandName.Contains("DownloadDoc"))
                {
                    ErrorLog.UploadTrace("RestrictedPIN | RowCommand-DownloadDoc | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    string status = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    importEntity.FileID = (gvRestrictedPIN.DataKeys[gvr.RowIndex].Values["FileID"]).ToString();
                    Session["FileID"] = importEntity.FileID;
                    importEntity.UserName = Page.User.Identity.Name;
                    SetPropertise(ref importEntity);
                    importEntity.Flag = (int)EnumCollection.EnumBindingType.Export;
                    DataSet Ds = importEntity.ExportRestrictedPIN();
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                    {
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "RowCommand-DownloadDoc";
                        _auditParams[2] = "Btnclear";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        obj.ExporttoExcel(Session["UserName"].ToString(), "CMS", "Upload RestrictedPIN Details", Ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Warning');", true);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadError(Ex);
                ErrorLog.UploadTrace("Failed For gvRestrictedPIN_RowCommand:" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btncancel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace("RestrictedPIN | btncancel_ServerClick | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                fileUpload.Dispose();
                gvRestrictedPIN.Visible = false;
                ErrorLog.UploadTrace("RestrictedPIN | btncancel_ServerClick | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At btncancel_ServerClick(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }



        protected void gvRestrictedPIN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRestrictedPIN.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At gvRestrictedPIN_PageIndexChanging(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region ValidateFileData

        public bool ValidateFileData<T>(T TObj, out string _StatusCode, out string _StatusDesc)
        {
            ErrorLog.UploadTrace("RestrictedPIN | ValidateFileData() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            bool IsValidRecord = true;
            _StatusCode = string.Empty;
            _StatusDesc = "Valid";
            try
            {
                ImportEntity _CommentUpdateFileIpmort = (ImportEntity)(object)TObj;
                ErrorLog.UploadTrace(string.Format("Initiated Validate File Request for Username : {0}.", Session["Username"].ToString()));
                _CustomeRegExpValidation = new clsCustomeRegularExpressions();
                
                if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.City))
                {
                    _StatusDesc = "City: City can not be empty or null;";
                    IsValidRecord = false;
                    return false;
                }

                if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.State))
                {
                    _StatusDesc = "State: State can not be empty or null;";
                    IsValidRecord = false;
                    return false;
                }
                

                //if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.DateOfBlackListing))
                //{
                //    _StatusDesc = "DateOfBlackListing: DateOfBlackListing can not be empty or null;";
                //    IsValidRecord = false;
                //    return false;
                //}
                //if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.ReasonForBlackListing))
                //{
                //    _StatusDesc = "ReasonForBlackListing: ReasonForBlackListing can not be empty or null;";
                //    IsValidRecord = false;
                //    return false;
                //}

                if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.IsRemoved))
                {
                    _StatusDesc = "IsRemoved: IsRemoved can not be empty or null;";
                    IsValidRecord = false;
                    return false;
                }
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, _CommentUpdateFileIpmort.PINCODE))
                {
                    _StatusDesc = "Invalid Pincode";
                    IsValidRecord = false;
                    return false;
                }
                ErrorLog.UploadTrace("RestrictedPIN | ValidateFileData() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedPIN : Error At ValidateFileData(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                IsValidRecord = false;
            }
            return IsValidRecord;
        }

        #endregion
    }
}