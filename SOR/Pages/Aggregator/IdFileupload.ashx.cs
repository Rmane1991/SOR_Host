using AppLogger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace SOR.Pages.Aggregator
{
    /// <summary>
    /// Summary description for IdFileupload
    /// </summary>
    public class IdFileupload : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string filedata = string.Empty;
            string fname, filepath = string.Empty;
            string filename = string.Empty;


            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    //  var employee = Convert.ToInt32(context.Request["Id"]);

                    if (Path.GetExtension(file.FileName).ToLower() != ".jpg" &&
                        Path.GetExtension(file.FileName).ToLower() != ".png" &&
                        Path.GetExtension(file.FileName).ToLower() != ".gif" &&
                        Path.GetExtension(file.FileName).ToLower() != ".jpeg" &&
                        Path.GetExtension(file.FileName).ToLower() != ".pdf"
                    )
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("Only jpg, png , gif, .jpeg, .pdf are allowed.!");
                        return;
                    }
                    decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                    if (size > 2048)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("File size should not exceed 2 MB.!");
                        return;
                    }

                    if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE" || HttpContext.Current.Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] {
                        '\\'
                    });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    filedata = context.Session["PanNo"].ToString();
                    //filedata ="ADF4857bG";
                    SaveFile(file, filedata, out filename, out filepath);
                }
            }

            if (filepath != string.Empty && filename != string.Empty)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully:" + filedata + "!");
            }
            else
            {
                context.Response.ContentType = "text/plain";
                //context.Response.Write("" + filedata + "");
            }

            context.Session["IdFileName"] = filename;
            context.Session["IdFilePath"] = filepath;

            //if you want to use file path in aspx.cs page , then assign it in to session  
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region SaveFile IdentityProof
        private string SaveFile(HttpPostedFile fileUpload, string id, out string FileName, out string FilePath)
        {
            try
            {
                string FileThumbnail = string.Empty;
                string FinalPathLocation = AppDomain.CurrentDomain.BaseDirectory + "OnBoarding Documents\\BCDocuments\\" + id + "\\" + "IdentityProof" + "\\";
                //string PathLocation = ConfigurationManager.AppSettings["BcDocumentPath"].ToString();
                //string FinalPathLocation = PathLocation + "\\" + id + "\\" + "IdentityProof" + "\\";
                if (!Directory.Exists(FinalPathLocation))
                {
                    Directory.CreateDirectory(FinalPathLocation);
                }
                //FileThumbnail = FinalPathLocation + Path.GetFileNameWithoutExtension(fileUpload.FileName) + "_Thumbnail" + Path.GetExtension(fileUpload.FileName);
                FileThumbnail = FinalPathLocation + Path.GetFileNameWithoutExtension(fileUpload.FileName) + "_Thumbnail.png";
                FinalPathLocation += fileUpload.FileName;

                // string pdf_filename = @"D:\Transit\Sbm_Payrakam_new\taimur\New folder\Images\document-1_220422_110414 (1) (1).pdf";

                if (File.Exists(FinalPathLocation))
                {
                    //File.Delete(FinalPathLocation);
                    fileUpload.SaveAs(FinalPathLocation);

                    if (Path.GetExtension(fileUpload.FileName) == ".pdf")
                    {
                        string png_filename = Path.GetDirectoryName(FinalPathLocation) + "\\" + Path.GetFileNameWithoutExtension(FinalPathLocation) + "_Thumbnail.png";
                        List<string> errors = cs_pdf_to_image.Pdf2Image.Convert(FinalPathLocation, png_filename);
                    }
                    else
                    {
                        fileUpload.SaveAs(FileThumbnail);
                    }
                }
                else
                {
                    fileUpload.SaveAs(FinalPathLocation);

                    if (Path.GetExtension(fileUpload.FileName) == ".pdf")
                    {
                        string png_filename = Path.GetDirectoryName(FinalPathLocation) + "\\" + Path.GetFileNameWithoutExtension(FinalPathLocation) + "_Thumbnail.png";
                        List<string> errors = cs_pdf_to_image.Pdf2Image.Convert(FinalPathLocation, png_filename);
                    }
                    else
                    {
                        fileUpload.SaveAs(FileThumbnail);
                    }
                }

                FileName = fileUpload.FileName;
                FilePath = "OnBoarding Documents\\BCDocuments\\" + id + "\\" + "IdentityProof" + "\\" + fileUpload.FileName;
                return FinalPathLocation;
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : IdFileUpload.ashx \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
                FileName = string.Empty;
                FilePath = string.Empty;
                return string.Empty; ;
            }
        }
        #endregion
    }
}