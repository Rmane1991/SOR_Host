using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR
{
    public partial class pdfhandler : System.Web.UI.Page
    {
        public string filepath { get; set; }
        public string Doctype;
        string pdfPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string val = Request.QueryString["value"].ToString();
                SetSession(val);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void savepdf(string doctype)
        {
            try
            {
                string dsfgdfsg = Session["doctype"].ToString();

                if (dsfgdfsg == "' + IdDoc + '")
                {
                    pdfPath = Session["pdfPathID"].ToString();
                }
                if (dsfgdfsg == "AddDoc")
                {
                    pdfPath = Session["pdfPathAdd"].ToString();
                }
                if (dsfgdfsg == "Sigdoc")
                {
                    pdfPath = Session["pdfPathSig"].ToString();
                }

                string filepath = AppDomain.CurrentDomain.BaseDirectory;
                string FileName = filepath.ToUpper() + pdfPath.ToUpper();
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(pdfPath);
                //if (pdfPath.EndsWith(".pdf")) {
                //    Response.ContentType = "application/pdf";
                //}
                //else
                if (FileName.EndsWith(".PDF"))
                {
                    Response.ContentType = "application/pdf";
                }
                else if (FileName.EndsWith(".JPG"))
                {
                    Response.ContentType = "image/jpg";
                }
                else if (FileName.EndsWith(".PNG"))
                {
                    Response.ContentType = "image/png";
                }
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public  string SetSession(string sessionval)
        {
            //Session["Doctype"] = sessionval;
            //savepdf(sessionval);

            try
            {
               // string dsfgdfsg = Session["doctype"].ToString();

                if (sessionval == "IdProof")
                {
                    pdfPath = Session["pdfPathID"].ToString();
                }
                if (sessionval == "AddProof")
                {
                    pdfPath = Session["pdfPathAdd"].ToString();
                }
                if (sessionval == "SigProof")
                {
                    pdfPath = Session["pdfPathSig"].ToString();
                }

                string filepath = AppDomain.CurrentDomain.BaseDirectory;
                string FileName = filepath.ToUpper() + pdfPath.ToUpper();
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(FileName);
                //if (pdfPath.EndsWith(".pdf")) {
                //    Response.ContentType = "application/pdf";
                //}
                //else
                if (FileName.EndsWith(".PDF"))
                {
                    Response.ContentType = "application/pdf";
                }
                else if (FileName.EndsWith(".JPG"))
                {
                    Response.ContentType = "image/jpg";
                }
                else if (FileName.EndsWith(".JPEG"))
                {
                    Response.ContentType = "image/jpeg";
                }
                else if (FileName.EndsWith(".PNG"))
                {
                    Response.ContentType = "image/png";
                }
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return sessionval;
        }


    }
}