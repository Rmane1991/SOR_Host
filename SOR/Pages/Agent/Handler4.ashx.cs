using AppLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SOR.Pages.Agent
{
    /// <summary>
    /// Summary description for Handler4
    /// </summary>
    public class Handler4 : IHttpHandler, IRequiresSessionState
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
                    //here UploadFile is define my folder name , where files will be store.  
                    filedata = context.Session["PanNo"].ToString();
                    SaveFileAddprof(file, filedata, out filename, out filepath);
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

            context.Session["AddAgentFileName"] = filename;
            context.Session["AddAgentFilePath"] = filepath;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #region SaveFile AddressProof
        private string SaveFileAddprof(HttpPostedFile fileUpload, string id, out string FileName, out string FilePath)
        {
            try
            {
                string FileThumbnail = string.Empty;
                string FinalPathLocation = AppDomain.CurrentDomain.BaseDirectory + "OnBoarding Documents\\AgentDocuments\\" + id + "\\" + "AddressProof" + "\\";
                //string PathLocation = ConfigurationManager.AppSettings["BcDocumentPath"].ToString();
                //string FinalPathLocation = PathLocation + "\\" + id + "\\" + "AddressProof" + "\\";
                if (!Directory.Exists(FinalPathLocation))
                {
                    Directory.CreateDirectory(FinalPathLocation);
                }
                FileThumbnail = FinalPathLocation + Path.GetFileNameWithoutExtension(fileUpload.FileName) + "_Thumbnail.png";
                FinalPathLocation += fileUpload.FileName;

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
                FilePath = "OnBoarding Documents\\AgentDocuments\\" + id + "\\" + "AddressProof" + "\\" + fileUpload.FileName;
                return FinalPathLocation;
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : AddFileUpload.ashx \nFunction : SaveFileAddprof() \nException Occured\n" + Ex.Message);
                FileName = string.Empty;
                FilePath = string.Empty;
                return string.Empty; ;
            }
        }
        #endregion
    }
}