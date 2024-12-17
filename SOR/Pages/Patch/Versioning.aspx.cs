using AppLogger;
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
using Ionic.Zip; // Add this at the top
using System.Globalization;
//using ICSharpCode.SharpZipLib.Zip;

namespace SOR.Pages.Patch
{
    public partial class Versioning : System.Web.UI.Page
    {
        #region ConnectionString

        //SystemLogger _systemLogger = new SystemLogger();
        ImportEntity importEntity = new ImportEntity();
        int _CmdTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
        #endregion

        #region property declaration
        PatchEntity _PatchEntity = new PatchEntity();
        public static CommonEntity _CommonEntity = new CommonEntity();
        #endregion

        //#region Constructor
        //public Versioning()
        //{
        //    try
        //    {
        //        TypeTableUploadDetails = new DataTable();
        //        if (TypeTableUploadDetails.Columns.Count == 0)
        //        {
        //            TypeTableUploadDetails.Columns.Add("City");
        //            TypeTableUploadDetails.Columns.Add("State");
        //            TypeTableUploadDetails.Columns.Add("PinCode");
        //            TypeTableUploadDetails.Columns.Add("DateOfBlackListing");
        //            TypeTableUploadDetails.Columns.Add("ReasonForBlackListing");
        //            TypeTableUploadDetails.Columns.Add("IsRemoved");
        //            TypeTableUploadDetails.Columns.Add("RecordStatus");
        //            TypeTableUploadDetails.Columns.Add("RecordStatusDescription");
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace("Versioning: RestrictedPIN(): Exception: " + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //}
        //#endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "Versioning.aspx", "34");
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
                            hdnshowmanual.Value = "true";
                            //hdnshowmanualR.Value = "true";
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtReleasedOn.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtPatchDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtPatchTime.Value = DateTime.Now.ToString("HH:mm");
                            FillGrid(EnumCollection.EnumBindingType.BindGrid);
                            UserPermissions.RegisterStartupScriptForNavigationListActive("11", "34");
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
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: Page_Load(): Exception: " + Ex.Message);
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
                gvVersioning.DataSource = null;
                gvVersioning.DataBind();
                SetPropertise();
                ds = _PatchEntity.GetPatchDetails();

                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = ds.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvVersioning.DataSource = dv;
                            gvVersioning.DataBind();
                            gvVersioning.Visible = true;
                        }
                        else
                        {
                            gvVersioning.DataSource = ds.Tables[0];
                            gvVersioning.DataBind();
                            gvVersioning.Visible = true;
                        }
                    }
                    else
                    {
                        gvVersioning.DataSource = null;
                        gvVersioning.DataBind();
                        gvVersioning.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "alert('No Data Found in Search Criteria. Try again', 'Warning');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: FillGrid(): Exception: " + Ex.Message);
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
        private void SetPropertise()
        {
            try
            {
                _PatchEntity.UserName = Session["Username"].ToString();
                _PatchEntity.FromDate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _PatchEntity.ToDate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                //importEntity.FileDescID = importEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadRestrictedPIN);
                //importEntity.FileStatus = (ddlFileTypeStatus.SelectedValue.ToString() != null && ddlFileTypeStatus.SelectedValue.ToString() != "" ? ddlFileTypeStatus.SelectedValue.ToString() : "0");
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: SetPropertise(): Exception: " + Ex.Message);
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
                if (ddlPatchType.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please select patchtype', 'Warning');</script>", false);
                    return;
                }
                else if (string.IsNullOrEmpty(txtVersion.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Version cannot be null or empty', 'Warning');</script>", false);
                    return;
                }
                else
                {
                    Save();
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: btnSave_Click(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
            }
        }
        private bool IsValidVersion(string version)
        {
            // Implement your version validation logic (e.g., regex)
            return !string.IsNullOrEmpty(version);
        }
        private void Save()
        {
            try
            {
                string releaseNotePath = string.Empty;
                string extractedPatchPath = string.Empty;
                string releaseNoteFileName = string.Empty;
                if (UploadPatch.HasFile && Path.GetExtension(UploadPatch.FileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    string zipFileName = Path.GetFileName(UploadPatch.FileName);
                    //string relativePath = @"D:\Data\Aakash\SOR\SOR\SOR\Patch\"; // Specify the D drive path
                    string relativePath = ConfigurationManager.AppSettings["Patches"]; // Specify the D drive path
                    //string relativePath = @"D:\UploadedPatches\"; // Specify the D drive path

                    // Combine the relative path with the zip file name to get the full file path
                    string zipFilePath = Path.Combine(relativePath, zipFileName);

                    // Get the directory from the full zip file path
                    string directoryPath = Path.GetDirectoryName(zipFilePath);

                    // Check if the directory exists, create it if it doesn't
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Save the uploaded ZIP file
                    UploadPatch.SaveAs(zipFilePath);

                    // Create timestamp for folder names
                    string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");

                    // Create directories for extracted files
                    extractedPatchPath = Path.Combine(directoryPath, "Patches", timestamp);
                    string extractedReleaseNotePath = Path.Combine(directoryPath, "ReleaseNotes", timestamp);

                    Directory.CreateDirectory(extractedPatchPath);
                    Directory.CreateDirectory(extractedReleaseNotePath);

                    try
                    {
                        // Extract using DotNetZip
                        using (ZipFile zip = ZipFile.Read(zipFilePath))
                        {
                            zip.ExtractAll(extractedPatchPath, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle extraction error
                        ErrorLog.UploadTrace($"Error extracting file: {ex.Message}");
                        return;
                    }

                    // Proceed with reading the DLL version
                    string[] dllFiles = Directory.GetFiles(extractedPatchPath, "*.dll");
                    string versionFromTextbox = txtVersion.Value; // Assuming txtVersion is your textbox

                    if (dllFiles.Length > 0)
                    {
                        foreach (string dllFilePath in dllFiles)
                        {
                            // Get version information for each DLL found
                            var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(dllFilePath);
                            string dllVersion = versionInfo.FileVersion;

                            // Log the DLL file name and its version
                            ErrorLog.UploadTrace($"DLL File: {Path.GetFileName(dllFilePath)} - Version: {dllVersion}");

                            // Compare DLL version with the version from the textbox
                            if (dllVersion.Equals(versionFromTextbox, StringComparison.OrdinalIgnoreCase))
                            {
                                ErrorLog.UploadTrace($"Match found for DLL: {Path.GetFileName(dllFilePath)} - Version: {dllVersion}");
                            }
                            else
                            {
                                ErrorLog.UploadTrace($"Version Not Match for DLL: {Path.GetFileName(dllFilePath)} - Version: {dllVersion}");
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Version Not Match. Try again', 'Warning');</script>", false);
                                return;
                            }
                        }
                    }
                    else
                    {
                        ErrorLog.UploadTrace("No DLL files found in the extracted files.");
                        return;
                    }

                    // Release Note
                    if (UploadReleaseNote.HasFile &&
                         (Path.GetExtension(UploadReleaseNote.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase) ||
                            Path.GetExtension(UploadReleaseNote.FileName).Equals(".docx", StringComparison.OrdinalIgnoreCase)))
                    {

                        releaseNoteFileName = Path.GetFileName(UploadReleaseNote.FileName);
                        releaseNotePath = Path.Combine(extractedReleaseNotePath, releaseNoteFileName);

                        // Save the release note
                        UploadReleaseNote.SaveAs(releaseNotePath);

                        // Log the path of the release note for future reference
                        ErrorLog.UploadTrace($"Release note uploaded: {releaseNotePath}");
                        ErrorLog.UploadTrace($"Successfully uploaded release note to: {releaseNotePath}");
                    }
                    else
                    {
                        ErrorLog.UploadTrace("No valid release note uploaded. Please upload a .docx file.");
                        return;
                    }



                    //File.Delete(zipFilePath);

                    ErrorLog.UploadTrace("Versioning: btnSave_Click(): Set Propertise...");
                    _PatchEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _PatchEntity.PatchType = ddlPatchType.SelectedValue != "0" ? ddlPatchType.SelectedValue : null;
                    _PatchEntity.PatchPath = !string.IsNullOrEmpty(extractedPatchPath) ? extractedPatchPath.Trim() : null;
                    _PatchEntity.Version = !string.IsNullOrEmpty(txtVersion.Value) ? txtVersion.Value.Trim() : null;
                    _PatchEntity.ReleaseNotePath = !string.IsNullOrEmpty(releaseNotePath) ? releaseNotePath : null;
                    _PatchEntity.ReleasedOn = !string.IsNullOrEmpty(txtReleasedOn.Value) ? Convert.ToDateTime(txtReleasedOn.Value).ToString("yyyy-MM-dd") : null;
                    _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                    _PatchEntity.ReleaseNoteFileName = !string.IsNullOrEmpty(releaseNoteFileName) ? releaseNoteFileName : null;
                    ErrorLog.UploadTrace("Versioning: btnSave_Click(): Going For Insert");
                    string statusCode = _PatchEntity.InsertOrUpdatePatchDetails();
                    if (statusCode == "INS00")
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    else
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    var response = new
                    {
                        StatusMessage = _CommonEntity.ResponseMessage
                    };
                    ErrorLog.RuleTrace("Versioning: btnSave_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: Save(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }

        #endregion

        #region Commented Code
        //#region Insert
        //public bool InsertFileData(string FileID, string Username, DataTable dtTable)
        //{
        //    try
        //    {
        //        importEntity.FileID = FileID;
        //        importEntity.Dtable = dtTable;
        //        importEntity.UserName = Username;
        //        importEntity._FromDate = DateTime.Now.ToString();
        //        importEntity.FileDate = txtDate.Value;
        //        string Status = string.Empty; string StatusMsg = string.Empty;
        //        importEntity.InsertBulkTerminalDetailsPIN(dtTable, out Status, out StatusMsg, FileID);
        //        if (Status == "00" || Status == "02")
        //        {
        //            ErrorLog.UploadTrace(string.Format("Successful Insert File Data Request For Upload RestrictedPIN. Username : {0}. FileId : {1}. Status : {2} StatusMsg : {3}", UserName, FileID, Status, StatusMsg));
        //            return true;
        //        }
        //        else
        //        {
        //            ErrorLog.UploadTrace(string.Format("Failed Insert File Data Request For Upload RestrictedPIN. Username : {0}. FileId : {1}. Status : {2} StatusMsg : {3}", UserName, FileID, Status, StatusMsg));
        //            return false;
        //        }
        //    }

        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace(string.Format("Failed Insert File Data Request For Upload RestrictedPIN. Username : {0}. FileId : {1}. Exception : {2}", UserName, FileID, Ex.Message));
        //        ErrorLog.UploadError(Ex);
        //        return false;
        //    }
        //}
        //#endregion

        //public void AddRowToDataTable<T>(T TObj)
        //{
        //    bool contains;

        //    try
        //    {
        //        ImportEntity _FileIpmort = (ImportEntity)(object)TObj;
        //        ErrorLog.UploadTrace(string.Format("Initiated Insert Add Row To DataTable Request. AgentName : {0}. Address : {1}. City : {2}. State : {3}. PinCode : {4}. AadhaarNo : {5}. PANNo : {6}. MobileNo : {7}. DateOfBlackListing : {8}. ReasonForBlackListing : {9}. CorporateBCName : {10}. BankName : {11}. IFSCCode : {12}. AccountNo : {13}. IsPoliceComplaint : {14}. IfFIRCompliant : {15}. DateofComplaint : {16}. IsBCAgentArrested : {17}. IsRemoved : {18}. IsvalidRecord : {19}. RecordStatus : {20}. FileStatus : {21}. RecordID : {22}. StatusDescription : {23}. FileId. ", _FileIpmort.AgentName, _FileIpmort.Address, _FileIpmort.City, _FileIpmort.State, _FileIpmort.PINCODE, _FileIpmort.AadhaarNo, _FileIpmort.PANNo, _FileIpmort.MobileNo, _FileIpmort.DateOfBlackListing, _FileIpmort.ReasonForBlackListing, _FileIpmort.CorporateBCName, _FileIpmort.BankName, _FileIpmort.IFSCCode, _FileIpmort.AccountNo, _FileIpmort.IsPoliceComplaint, _FileIpmort.IfFIRCompliant, _FileIpmort.DateofComplaint, _FileIpmort.IsBCAgentArrested, _FileIpmort.IsRemoved, _FileIpmort.IsValidRecord, _FileIpmort.RecordStatus, _FileIpmort.FileStatus, _FileIpmort.RecordID, _FileIpmort.RecordStatus, _FileIpmort.FileID));
        //        DataRow _RowTypeTable = TypeTableUploadDetails.NewRow();
        //        _RowTypeTable["City"] = _FileIpmort.City;
        //        _RowTypeTable["State"] = _FileIpmort.State;
        //        _RowTypeTable["PinCode"] = _FileIpmort.PINCODE;
        //        _RowTypeTable["DateOfBlackListing"] = _FileIpmort.DateOfBlackListing;
        //        _RowTypeTable["ReasonForBlackListing"] = _FileIpmort.ReasonForBlackListing;
        //        _RowTypeTable["IsRemoved"] = _FileIpmort.IsRemoved;

        //        //string ATMID = _FileIpmort.ATMID;
        //        //contains = TypeTableUploadDetails.AsEnumerable().Any(row => ATMID == row.Field<String>("ATMID"));

        //        //if (contains && _FileIpmort.RecordStatus == "1")
        //        //{
        //        //    _FileIpmort.RecordStatus = "0";
        //        //    _FileIpmort.IsValidRecord = 0;
        //        //    _FileIpmort.RecordStatusDescription = "Duplicate Reord";
        //        //    _dupRcdsCnt = _dupRcdsCnt + 1;
        //        //}
        //        _RowTypeTable["RecordStatus"] = _FileIpmort.RecordStatus;
        //        _RowTypeTable["RecordStatusDescription"] = _FileIpmort.RecordStatusDescription;
        //        TypeTableUploadDetails.Rows.Add(_RowTypeTable);

        //        ErrorLog.UploadTrace(string.Format("Completed Insert Add Row To DataTable Request. AgentName : {0}. Address : {1}. City : {2}. State : {3}. PinCode : {4}. AadhaarNo : {5}. PANNo : {6}. MobileNo : {7}. DateOfBlackListing : {8}. ReasonForBlackListing : {9}. CorporateBCName : {10}. BankName : {11}. IFSCCode : {12}. AccountNo : {13}. IsPoliceComplaint : {14}. IfFIRCompliant : {15}. DateofComplaint : {16}. IsBCAgentArrested : {17}. IsRemoved : {18}. IsvalidRecord : {19}. RecordStatus : {20}. FileStatus : {21}. RecordID : {22}. StatusDescription : {23}. FileId. ", _FileIpmort.AgentName, _FileIpmort.Address, _FileIpmort.City, _FileIpmort.State, _FileIpmort.PINCODE, _FileIpmort.AadhaarNo, _FileIpmort.PANNo, _FileIpmort.MobileNo, _FileIpmort.DateOfBlackListing, _FileIpmort.ReasonForBlackListing, _FileIpmort.CorporateBCName, _FileIpmort.BankName, _FileIpmort.IFSCCode, _FileIpmort.AccountNo, _FileIpmort.IsPoliceComplaint, _FileIpmort.IfFIRCompliant, _FileIpmort.DateofComplaint, _FileIpmort.IsBCAgentArrested, _FileIpmort.IsRemoved, _FileIpmort.IsValidRecord, _FileIpmort.RecordStatus, _FileIpmort.FileStatus, _FileIpmort.RecordID, _FileIpmort.RecordStatus, _FileIpmort.FileID));
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace("Failed for AddRowToDataTable:" + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //    }
        //}

        //public bool ProcessFile(string FileExtension, string FilePath, out DataTable dataTableExcel)
        //{
        //    bool IsFileProcessed = true;
        //    dataTableExcel = new DataTable();
        //    ErrorLog.UploadTrace(string.Format("Initiated Process File Request. Username : {0}. FilePath : {1}.FileExtension : {2}.", Session["Username"].ToString(), FilePath, FileExtension));
        //    if (FileExtension.ToLower() == ".xls")
        //    {
        //        _ConnctionString.Append("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath.ToString() + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
        //    }
        //    else if (FileExtension.ToLower() == ".xlsx")
        //    {
        //        _ConnctionString.Append("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath.ToString() + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
        //    }
        //    try
        //    {
        //        _OleDbConnection = new OleDbConnection(_ConnctionString.ToString());
        //        OleDbCommand cmd;
        //        _OleDbConnection.Open();
        //        cmd = new OleDbCommand("select * from [BulkData$]", _OleDbConnection);
        //        DataTable dtExcelSheetName1 = _OleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        string getExcelSheetName = dtExcelSheetName1.Rows[0]["Table_Name"].ToString();
        //        cmd = new OleDbCommand("select * from [" + getExcelSheetName + "]", _OleDbConnection);
        //        OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);

        //        dAdapter.SelectCommand = cmd;
        //        dAdapter.Fill(dataTableExcel);

        //        if (_OleDbConnection.State == ConnectionState.Open) _OleDbConnection.Close();
        //        cmd.Dispose();
        //        dAdapter.Dispose();
        //        UploadRestrictedPINFields = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["UploadRestrictedPINFields"].ToString()) ? Convert.ToInt32(ConfigurationManager.AppSettings["UploadRestrictedPINFields"].ToString()) : 0;
        //        if (dataTableExcel.Rows.Count > 0)
        //        {
        //            if (UploadRestrictedPINFields != dataTableExcel.Columns.Count)
        //            {
        //                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please check the number of fields in the file.','File Format');</script>", false);
        //                IsFileProcessed = false;

        //            }

        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please check the number of fields in the file.','File Format');</script>", false);
        //            IsFileProcessed = true;
        //        }

        //        ErrorLog.UploadTrace(string.Format("Completed Process File Request. Username : {0}. FilePath : {1}. FileExtension : {2}.", Session["Username"].ToString(), FilePath, FileExtension));
        //    }
        //    catch (ArgumentException Ex)
        //    {
        //        ErrorLog.UploadError(Ex);
        //        ErrorLog.UploadTrace(string.Format("Failed Process File Request. Username : {0}. FilePath : {1}. Status : {2}", Session["Username"].ToString(), FilePath, Ex.Message));
        //        IsFileProcessed = false;
        //    }
        //    catch (OleDbException Ex)
        //    {
        //        ErrorLog.UploadError(Ex);
        //        ErrorLog.UploadTrace(string.Format("Failed Process File Request. Username : {0}. FilePath : {1}. Status : {2}", Session["Username"].ToString(), FilePath, Ex.Message));
        //        IsFileProcessed = false;
        //    }
        //    catch (InvalidOperationException Ex)
        //    {
        //        IsFileProcessed = false;
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('System does not supports " + FileExtension + " file format.','Warning');</script>", false);
        //        ErrorLog.UploadTrace("RestrictedPIN : Error At ProcessFile(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //    }
        //    return IsFileProcessed;

        //}

        //public bool FileImportEntry(FileValidator _FileValidator, out string FileId)
        //{
        //    FileId = string.Empty;
        //    try
        //    {
        //        importEntity.FileName = _FileValidator.FileName;
        //        importEntity.FilePath = _FileValidator.FilePath;
        //        importEntity.FileType = _FileValidator.FileType;
        //        importEntity.UserName = _FileValidator.Username;
        //        importEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadRestrictedPIN);
        //        importEntity.FileDescName = EnumCollection.EnumFileDesciption.UploadRestrictedPIN.ToString();

        //        string Status = importEntity.InsertFileImport(out FileId);
        //        return Status == "00" ? true : false;
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace("RestrictedPIN : Error At FileImportEntry(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //        return false;
        //    }
        //}

        //#region ValidateFileFormat

        //public bool ValidateFileFormat(string FileExtension)
        //{
        //    try
        //    {
        //        ErrorLog.UploadTrace(string.Format("Initiated Validate File Request. Username : {0}.", Session["Username"].ToString()));
        //        return FileExtension.ToUpper() != ".XLS" && FileExtension.ToUpper() != ".XLSX" ? false : true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace("RestrictedPIN : Error At ValidateFileFormat(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //        return false;
        //    }
        //}




        //#region Save File
        //public string SaveFile(FileUpload fileUpload)
        //{
        //    try
        //    {
        //        ErrorLog.UploadTrace(string.Format("Initiated Save File Request. Username : {0}. FileName: {1}.", Session["Username"].ToString(), fileUpload.PostedFile.FileName));
        //        string PathLocation = ConfigurationManager.AppSettings["UploadRestrictedPIN"].ToString();

        //        ErrorLog.UploadTrace("PathLocation: " + PathLocation);
        //        if (!Directory.Exists(PathLocation))
        //        {
        //            Directory.CreateDirectory(PathLocation);
        //        }
        //        PathLocation += fileUpload.PostedFile.FileName;

        //        if (File.Exists(PathLocation))
        //        {
        //            File.Delete(PathLocation);
        //            fileUpload.SaveAs(PathLocation);
        //            ErrorLog.UploadTrace(string.Format("Completed Save File Request. Username : {0}. FileName: {1}.", Session["Username"].ToString(), fileUpload.PostedFile.FileName));
        //        }
        //        else
        //        {
        //            fileUpload.SaveAs(PathLocation);
        //            ErrorLog.UploadTrace(string.Format("Completed Save File Request. Username : {0}. FileName: {1}.", Session["Username"].ToString(), fileUpload.PostedFile.FileName));
        //        }
        //        return PathLocation;

        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace("RestrictedPIN : Error At SaveFile(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //        return string.Empty;
        //    }
        //}

        #endregion

        #region  Button
        protected void btnsample_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.UploadTrace(string.Format("btnsample_Click() | Initiated Release Note Sample File Download Request."));
                string strURL = string.Empty;


                strURL = "~/ExportedExcelFiles/ReleaseNote.docx";
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ReleaseNote.docx");
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
                ErrorLog.UploadTrace(string.Format("Completed RestrictedPIN Sample File Download."));

            }
            catch (System.Threading.ThreadAbortException Ex)
            {
                ErrorLog.UploadTrace("Versioning: btnsample_Click(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
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
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
            }

            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: btnsearch_Click(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }

        protected void Btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlFileTypeStatus.ClearSelection();
                gvVersioning.Visible = false;
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: Btnclear_Click(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        #endregion

        #region Gridview Events
        protected void gvVersioning_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ExportFormat obj = new ExportFormat();
            try
            {
                Session["IsCancelReqId"] = null;
                Session["ReqId"] = null;
                Session["IsSchedule"] = null;
                Session["IsProductionReqId"] = null;
                if (e.CommandName == "EditPatch")
                {
                    try
                    {
                        string patchPath = e.CommandArgument.ToString();
                        Session["ReqId"] = patchPath;
                        hdnshowmanual.Value = "true";  // Set hidden field to show the modal
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.UploadTrace("Versioning: gvVersioning_RowCommand(): Exception: " + Ex.Message);
                        ErrorLog.UploadError(Ex);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
                        return;
                    }
                }
                if (e.CommandName == "OpenReleaseNote")
                {
                    string releaseNoteRelativePath = e.CommandArgument.ToString();
                    string baseDirectory = ConfigurationManager.AppSettings["ReleaseNote"];
                    string fullPath = Path.Combine(baseDirectory, releaseNoteRelativePath);

                    // Check if the file exists
                    if (File.Exists(fullPath))
                    {
                        string fullPathStr = fullPath.Replace("\\", "/");
                        int startIndex = fullPathStr.IndexOf("Patch/ReleaseNotes");

                        if (startIndex != -1)
                        {
                            string relativeUrl = fullPathStr.Substring(startIndex);
                            string releaseNoteUrl = $"http://localhost:59954/{relativeUrl}";

                            string script = $"window.open('{releaseNoteUrl}', '_blank');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenReleaseNote", script, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FileNotFound", "alert('Invalid file path.');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FileNotFound", "alert('File not found.');", true);
                    }
                }
                if (e.CommandName == "DownloadDoc")
                {
                    string releaseNoteRelativePath = e.CommandArgument.ToString();
                    string baseDirectory = ConfigurationManager.AppSettings["Patches"];
                    string fullPath = Path.Combine(baseDirectory, releaseNoteRelativePath);

                    ////string patchPath = e.CommandArgument.ToString();
                    //string patchPath = @"D:\UploadedPatches\Patches\22112024181723"; // Use @ to denote a verbatim string;
                    ////string modifiedPath = patchPath.Replace("\\\\", "\\");
                    DownloadPatch(fullPath);
                }
                if (e.CommandName == "Cancel")
                {
                    string rowId = e.CommandArgument.ToString();
                    Session["IsCancelReqId"] = rowId;
                }
                if (e.CommandName == "Reschedule")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    Session["ReqId"] = rowIndex;
                    hdnshowmanual.Value = "true";
                    Session["IsSchedule"] = "1";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);


                }
                if (e.CommandName == "Production")
                {
                    string rowId = e.CommandArgument.ToString();
                    Session["IsProductionReqId"] = rowId;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : Error At gvVersioning_RowCommand(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void ZipFilesInDirectory(string sourceDir, string zipFilePath)
        {
            // Create a zip file and add all files from the source directory using DotNetZip (Ionic.Zip)
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(sourceDir); // Add all files in the directory

                // Optionally set compression level
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                // Save the zip file to the specified path
                zip.Save(zipFilePath);
            }
        }
        private void DownloadPatch(string patchPath)
        {
            if (!Directory.Exists(patchPath))
            {
                // Handle directory not found
                ErrorLog.UploadTrace($"Directory not found: {patchPath}");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Directory not found. Try again', 'Warning');</script>", false);
                return;
            }

            string zipFilePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now:yyyyMMddHHmmss}.zip");

            try
            {
                // Create a zip file from the directory using Ionic.Zip (DotNetZip)
                ZipFilesInDirectory(patchPath, zipFilePath);

                // Send the zip file to the browser
                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={Path.GetFileName(zipFilePath)}");
                Response.TransmitFile(zipFilePath);
                Response.End();

                // Optionally, delete the temp zip file after sending it
                File.Delete(zipFilePath);
            }
            catch (Exception ex)
            {
                // Handle errors
                ErrorLog.UploadTrace($"Error zipping or downloading: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "<script>showWarning('Error zipping or downloading the patch. Try again.', 'Error');</script>", false);
            }
        }
        public void BindCugServices()
        {
            try
            {
                DataTable ds = _PatchEntity.BindCugServices();
                ddlcugservices.Items.Clear(); // Clear existing items

                if (ds != null && ds.Rows.Count > 0 && ds.Rows.Count > 0)
                {
                    if (ds.Rows.Count == 1)
                    {
                        ddlcugservices.Items.Clear();
                        ddlcugservices.DataSource = ds.Copy();
                        ddlcugservices.DataTextField = "name";
                        ddlcugservices.DataValueField = "id";
                        ddlcugservices.DataBind();
                        ddlcugservices.Enabled = true;
                        ddlcugservices.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlcugservices.Items.Clear();
                        ddlcugservices.DataSource = ds.Copy();
                        ddlcugservices.DataTextField = "name";
                        ddlcugservices.DataValueField = "id";
                        ddlcugservices.DataBind();
                        ddlcugservices.Enabled = true;
                        ddlcugservices.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlcugservices.Enabled = false;
                    ddlcugservices.DataSource = null;
                    ddlcugservices.DataBind();
                    ddlcugservices.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : Error At BindCugServices(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void ddlpatchscheduletype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlpatchscheduletype.SelectedValue == "1")
                {
                    BindCugServices();
                }
                else
                {
                    ddlcugservices.Enabled = false;
                }
                hdnshowmanual.Value = "true";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : Error At ddlpatchscheduletype_SelectedIndexChanged(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btncancel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                //fileUpload.Dispose();
                gvVersioning.Visible = false;
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : Error At btncancel_ServerClick(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void gvVersioning_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Get the cugstatus value from the current row
                    string cugStatus = DataBinder.Eval(e.Row.DataItem, "cugstatus").ToString();
                    string prodStatus = DataBinder.Eval(e.Row.DataItem, "prodstatus").ToString();
                    // Find the buttons in the current row
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    ImageButton btnCancelAction = (ImageButton)e.Row.FindControl("btnCancelAction");
                    ImageButton btnReschedule = (ImageButton)e.Row.FindControl("btnReschedule");
                    ImageButton btnProduction = (ImageButton)e.Row.FindControl("btnProduction");
                    ImageButton btnProd = (ImageButton)e.Row.FindControl("btnProd");
                    
                    if (cugStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = false; // Hide Edit button
                        if (btnCancelAction != null) btnCancelAction.Visible = true; // Show Cancel button
                        if (btnReschedule != null) btnReschedule.Visible = true; // Show Reschedule button
                        if (btnProduction != null) btnProduction.Visible = false; // Hide Production button
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("CUG Decline", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = false; // Hide Edit button
                        if (btnCancelAction != null) btnCancelAction.Visible = false; // Hide Cancel button
                        if (btnReschedule != null) btnReschedule.Visible = false; // Hide Reschedule button
                        if (btnProduction != null) btnProduction.Visible = false; // Show Production button
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = false; // Hide Edit button
                        if (btnCancelAction != null) btnCancelAction.Visible = false; // Hide Cancel button
                        if (btnReschedule != null) btnReschedule.Visible = false; // Hide Reschedule button
                        if (btnProduction != null) btnProduction.Visible = true; // Show Production button
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("Decline", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = false; // Hide Edit button
                        if (btnCancelAction != null) btnCancelAction.Visible = false; // Hide Cancel button
                        if (btnReschedule != null) btnReschedule.Visible = false; // Hide Reschedule button
                        if (btnProduction != null) btnProduction.Visible = false; // Hide Production button
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("Re-Schedule", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = false; // Hide Edit button
                        if (btnCancelAction != null) btnCancelAction.Visible = true; // Show Cancel button
                        if (btnReschedule != null) btnReschedule.Visible = false; // Hide Reschedule button
                        if (btnProduction != null) btnProduction.Visible = false; // Hide Production button
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("NA", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = true; // Show Edit button
                        if (btnCancelAction != null) btnCancelAction.Visible = false; // Hide Cancel button
                        if (btnReschedule != null) btnReschedule.Visible = false; // Hide Reschedule button
                        if (btnProduction != null) btnProduction.Visible = false; // Hide Production button
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                    {
                        // Hide the buttons
                        if (btnEdit != null) btnEdit.Visible = false;
                        if (btnCancelAction != null) btnCancelAction.Visible = false;
                        if (btnReschedule != null) btnReschedule.Visible = false;
                        if (btnProduction != null) btnProduction.Visible = false;
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("Approve", StringComparison.OrdinalIgnoreCase) && prodStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        // Hide all buttons
                        if (btnEdit != null) btnEdit.Visible = false;
                        if (btnCancelAction != null) btnCancelAction.Visible = false;
                        if (btnReschedule != null) btnReschedule.Visible = false;
                        if (btnProduction != null) btnProduction.Visible = false;
                        if (btnProd != null) btnProd.Visible = false; // Hide Production button
                    }
                    if (cugStatus.Equals("Approve", StringComparison.OrdinalIgnoreCase) && prodStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                    {
                        // Hide all buttons
                        if (btnEdit != null) btnEdit.Visible = false;
                        if (btnCancelAction != null) btnCancelAction.Visible = false;
                        if (btnReschedule != null) btnReschedule.Visible = false;
                        if (btnProduction != null) btnProduction.Visible = false;
                        if (btnProd != null) btnProd.Visible = true;
                    }
                    if (cugStatus.Equals("Approve", StringComparison.OrdinalIgnoreCase) && prodStatus.Equals("Approve", StringComparison.OrdinalIgnoreCase))
                    {
                        // Hide all buttons
                        if (btnEdit != null) btnEdit.Visible = false;
                        if (btnCancelAction != null) btnCancelAction.Visible = false;
                        if (btnReschedule != null) btnReschedule.Visible = false;
                        if (btnProduction != null) btnProduction.Visible = false;
                        if (btnProd != null) btnProd.Visible = false;
                    }
                    if (cugStatus.Equals("Approve", StringComparison.OrdinalIgnoreCase) && prodStatus.Equals("Decline", StringComparison.OrdinalIgnoreCase))
                    {
                        // Hide all buttons
                        if (btnEdit != null) btnEdit.Visible = false;
                        if (btnCancelAction != null) btnCancelAction.Visible = false;
                        if (btnReschedule != null) btnReschedule.Visible = false;
                        if (btnProduction != null) btnProduction.Visible = false;
                        if (btnProd != null) btnProd.Visible = false;
                    }
                    if (cugStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        if (btnEdit != null) btnEdit.Visible = false;
                        if (btnProd != null) btnProd.Visible = false;
                    }
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : Error At gvVersioning_RowDataBound(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void confirmCancel_Click(object sender, EventArgs e)
        {
            try
            {
                _PatchEntity.Remarks = txtRemark.Text;
                _PatchEntity.ReqId = Session["IsCancelReqId"].ToString();
                _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.Export);
                ErrorLog.UploadTrace("Versioning: btnSubmitRemarks_Click(): Going For Insert");

                string statusCode = _PatchEntity.InsertOrUpdateVersioningDetails();

                if (statusCode == "UPD00")
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }
                else
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }

                var response = new
                {
                    StatusMessage = _CommonEntity.ResponseMessage
                };

                ErrorLog.RuleTrace("Versioning: btnSubmitDetails_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : confirmCancel_Click(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gvVersioning_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVersioning.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : Error At gvVersioning_PageIndexChanging(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        //#region ValidateFileData

        //public bool ValidateFileData<T>(T TObj, out string _StatusCode, out string _StatusDesc)

        //{
        //    bool IsValidRecord = true;
        //    _StatusCode = string.Empty;
        //    _StatusDesc = "Valid";
        //    try
        //    {
        //        ImportEntity _CommentUpdateFileIpmort = (ImportEntity)(object)TObj;
        //        ErrorLog.UploadTrace(string.Format("Initiated Validate File Request for Username : {0}.", Session["Username"].ToString()));
        //        _CustomeRegExpValidation = new clsCustomeRegularExpressions();

        //        if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.City))
        //        {
        //            _StatusDesc = "City: City can not be empty or null;";
        //            IsValidRecord = false;
        //            return false;
        //        }

        //        if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.State))
        //        {
        //            _StatusDesc = "State: State can not be empty or null;";
        //            IsValidRecord = false;
        //            return false;
        //        }


        //        //if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.DateOfBlackListing))
        //        //{
        //        //    _StatusDesc = "DateOfBlackListing: DateOfBlackListing can not be empty or null;";
        //        //    IsValidRecord = false;
        //        //    return false;
        //        //}
        //        //if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.ReasonForBlackListing))
        //        //{
        //        //    _StatusDesc = "ReasonForBlackListing: ReasonForBlackListing can not be empty or null;";
        //        //    IsValidRecord = false;
        //        //    return false;
        //        //}

        //        if (string.IsNullOrEmpty(_CommentUpdateFileIpmort.IsRemoved))
        //        {
        //            _StatusDesc = "IsRemoved: IsRemoved can not be empty or null;";
        //            IsValidRecord = false;
        //            return false;
        //        }
        //        if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, _CommentUpdateFileIpmort.PINCODE))
        //        {
        //            _StatusDesc = "Invalid Pincode";
        //            IsValidRecord = false;
        //            return false;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.UploadTrace("RestrictedPIN : Error At ValidateFileData(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
        //        ErrorLog.UploadError(Ex);
        //        IsValidRecord = false;
        //    }
        //    return IsValidRecord;
        //}
        #region Submit
        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["IsSchedule"] != null)
                {
                    string selectedServices = hdnSelectedServices.Value;
                    if (!string.IsNullOrEmpty(selectedServices))
                    {
                        // Assign the comma-separated values directly to cugservices
                        _PatchEntity.cugservices = selectedServices;  // No need to split if it's already a string
                    }
                    else
                    {
                        _PatchEntity.cugservices = null;  // If nothing is selected, set to null
                    }
                    _PatchEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _PatchEntity.schedulepatchtype = ddlpatchscheduletype.SelectedValue != "0" ? ddlpatchscheduletype.SelectedValue : null;
                    //_PatchEntity.cugservices = ddlcugservices.SelectedValue != "0" ? ddlcugservices.SelectedValue : null;
                    string combinedDateTimeString = txtPatchDate.Value + " " + txtPatchTime.Value;
                    _PatchEntity.scheduledatetime = DateTime.Parse(combinedDateTimeString).ToString("yyyy-MM-dd HH:mm:ss");
                    _PatchEntity.ReqId = Session["ReqId"].ToString();

                    _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.ExportBulk);
                    ErrorLog.UploadTrace("Versioning: btnSubmitDetails_Click(): Going For Insert");
                    string statusCode = _PatchEntity.InsertOrUpdateVersioningDetails();
                    if (statusCode == "UPD00")
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    else
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    var response = new
                    {
                        StatusMessage = _CommonEntity.ResponseMessage
                    };
                    ErrorLog.RuleTrace("Versioning: btnSubmitDetails_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                }
                else
                {
                    string selectedServices = hdnSelectedServices.Value;
                    if (!string.IsNullOrEmpty(selectedServices))
                    {
                        // Assign the comma-separated values directly to cugservices
                        _PatchEntity.cugservices = selectedServices;  // No need to split if it's already a string
                    }
                    else
                    {
                        _PatchEntity.cugservices = null;  // If nothing is selected, set to null
                    }
                    _PatchEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _PatchEntity.schedulepatchtype = ddlpatchscheduletype.SelectedValue != "0" ? ddlpatchscheduletype.SelectedValue : null;
                    //_PatchEntity.cugservices = ddlcugservices.SelectedValue != "0" ? ddlcugservices.SelectedValue : null;
                    string combinedDateTimeString = txtPatchDate.Value + " " + txtPatchTime.Value;
                    _PatchEntity.scheduledatetime = DateTime.Parse(combinedDateTimeString).ToString("yyyy-MM-dd HH:mm:ss");
                    _PatchEntity.ReqId = Session["ReqId"].ToString();

                    _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                    ErrorLog.UploadTrace("Versioning: btnSubmitDetails_Click(): Going For Insert");
                    string statusCode = _PatchEntity.InsertOrUpdateVersioningDetails();
                    if (statusCode == "INS00")
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    else
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    var response = new
                    {
                        StatusMessage = _CommonEntity.ResponseMessage
                    };
                    ErrorLog.RuleTrace("Versioning: btnSubmitDetails_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : btnSubmitDetails_Click(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion
        protected void btnClsManual_Click(object sender, EventArgs e)
        {
            try
            {
                hdnshowmanual.Value = "false";
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnClsManual_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }

        protected void btncugstatus_Click(object sender, EventArgs e)
        {
            try
            {
                string cugDocumentPath = string.Empty;
                string cugDocumentFileName = string.Empty;

                // Define allowed extensions for document uploads (you can add more as needed)
                string[] allowedExtensions = { ".pdf", ".docx" };

                // Check if a file is uploaded and if its extension is in the allowed extensions list
                if (FileUploadCUG.HasFile)
                {
                    string fileExtension = Path.GetExtension(FileUploadCUG.FileName).ToLower();

                    // Validate if the file extension is allowed
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Get the file name and path where it will be saved
                        //cugDocumentFileName = Path.GetFileName(FileUploadCUG.FileName);
                        //string cugDocumentDirectory = ConfigurationManager.AppSettings["CUGDocuments"]; // Specify the path where CUG documents should be saved
                        //cugDocumentPath = Path.Combine(cugDocumentDirectory, cugDocumentFileName);

                        //// Save the CUG document
                        //FileUploadCUG.SaveAs(cugDocumentPath);

                        // Get the directory from the configuration settings
                        cugDocumentFileName = Path.GetFileName(FileUploadCUG.FileName);

                        // Get the directory from the configuration settings
                        string cugDocumentDirectory = ConfigurationManager.AppSettings["CUGDocuments"];

                        // Combine the directory path with the file name to get the full file path
                        cugDocumentPath = Path.Combine(cugDocumentDirectory, cugDocumentFileName);

                        // Check if the directory exists, and if not, create it
                        if (!Directory.Exists(cugDocumentDirectory))
                        {
                            Directory.CreateDirectory(cugDocumentDirectory); // Create the directory if it doesn't exist
                        }

                        // Save the uploaded CUG document to the specified path
                        FileUploadCUG.SaveAs(cugDocumentPath);

                        // Log the path of the CUG document for future reference
                        ErrorLog.UploadTrace($"CUG document uploaded: {cugDocumentPath}");
                        ErrorLog.UploadTrace($"Successfully uploaded CUG document to: {cugDocumentPath}");

                        _PatchEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _PatchEntity.Status = ddlcugstatus.SelectedValue != "0" ? ddlcugstatus.SelectedValue : null;
                        _PatchEntity.DocPath = !string.IsNullOrEmpty(cugDocumentPath) ? cugDocumentPath : null;
                        _PatchEntity.DocName = !string.IsNullOrEmpty(cugDocumentFileName) ? cugDocumentFileName : null;
                        _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                        _PatchEntity.Id = Session["IsProductionReqId"].ToString();

                        ErrorLog.UploadTrace("Versioning: btncugstatus_Click(): Going For Insert");
                        string statusCode = _PatchEntity.UpdatecugprodDocumnetsDetails();
                        if (statusCode == "UPD00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        var response = new
                        {
                            StatusMessage = _CommonEntity.ResponseMessage
                        };
                        ErrorLog.RuleTrace("Versioning: btnSave_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);

                    }
                    else
                    {
                        // If the file extension is not allowed, log an error message
                        ErrorLog.UploadTrace($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}.");
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid file type. Only PDF, DOCX. are allowed.', 'Warning');</script>", false);
                    }
                }
                else
                {
                    // If no file is uploaded, log an error message
                    ErrorLog.UploadTrace("Versioning : btncugstatus_Click() No file uploaded. Please select a file to upload.");
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : btncugstatus_Click(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnprodstatus_Click(object sender, EventArgs e)
        {
            try
            {
                string cugDocumentPath = string.Empty;
                string cugDocumentFileName = string.Empty;

                // Define allowed extensions for document uploads (you can add more as needed)
                string[] allowedExtensions = { ".pdf", ".docx" };

                // Check if a file is uploaded and if its extension is in the allowed extensions list
                if (FileUploadProd.HasFile)
                {
                    string fileExtension = Path.GetExtension(FileUploadProd.FileName).ToLower();

                    // Validate if the file extension is allowed
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Get the file name and path where it will be saved
                        //cugDocumentFileName = Path.GetFileName(FileUploadCUG.FileName);
                        //string cugDocumentDirectory = ConfigurationManager.AppSettings["CUGDocuments"]; // Specify the path where CUG documents should be saved
                        //cugDocumentPath = Path.Combine(cugDocumentDirectory, cugDocumentFileName);

                        //// Save the CUG document
                        //FileUploadCUG.SaveAs(cugDocumentPath);

                        // Get the directory from the configuration settings
                        cugDocumentFileName = Path.GetFileName(FileUploadProd.FileName);

                        // Get the directory from the configuration settings
                        string cugDocumentDirectory = ConfigurationManager.AppSettings["ProdDocuments"];

                        // Combine the directory path with the file name to get the full file path
                        cugDocumentPath = Path.Combine(cugDocumentDirectory, cugDocumentFileName);

                        // Check if the directory exists, and if not, create it
                        if (!Directory.Exists(cugDocumentDirectory))
                        {
                            Directory.CreateDirectory(cugDocumentDirectory); // Create the directory if it doesn't exist
                        }

                        // Save the uploaded CUG document to the specified path
                        FileUploadProd.SaveAs(cugDocumentPath);

                        // Log the path of the CUG document for future reference
                        ErrorLog.UploadTrace($"CUG document uploaded: {cugDocumentPath}");
                        ErrorLog.UploadTrace($"Successfully uploaded CUG document to: {cugDocumentPath}");

                        _PatchEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _PatchEntity.Status = ddlcugstatus.SelectedValue != "0" ? ddlcugstatus.SelectedValue : null;
                        _PatchEntity.DocPath = !string.IsNullOrEmpty(cugDocumentPath) ? cugDocumentPath : null;
                        _PatchEntity.DocName = !string.IsNullOrEmpty(cugDocumentFileName) ? cugDocumentFileName : null;
                        _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.Export);
                        _PatchEntity.Id = Session["IsProductionReqId"].ToString();

                        ErrorLog.UploadTrace("Versioning: btncugstatus_Click(): Going For Insert");
                        string statusCode = _PatchEntity.UpdatecugprodDocumnetsDetails();
                        if (statusCode == "UPD00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        var response = new
                        {
                            StatusMessage = _CommonEntity.ResponseMessage
                        };
                        ErrorLog.RuleTrace("Versioning: btnSave_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);

                    }
                    else
                    {
                        // If the file extension is not allowed, log an error message
                        ErrorLog.UploadTrace($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}.");
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid file type. Only PDF, DOCX. are allowed.', 'Warning');</script>", false);
                    }
                }
                else
                {
                    // If no file is uploaded, log an error message
                    ErrorLog.UploadTrace("Versioning : btncugstatus_Click() No file uploaded. Please select a file to upload.");
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning : btncugstatus_Click(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
    }
}