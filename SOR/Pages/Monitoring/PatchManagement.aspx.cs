using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Monitoring
{
    public partial class PatchManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public class Service
        {
            public string ServiceName { get; set; }
            public string ServiceStatus { get; set; }
            public string Status { get; set; }
            public string UserId { get; set; }
            public string DownTime { get; set; }
            public string UpTime { get; set; }
            public string Error { get; set; }
            public string SourcePath { get; set; }
            public string DestinationPath { get; set; }
        }


        [System.Web.Services.WebMethod]
        public static string ManageFileReplacement(string Source, string Distination, string Type)
        {
            try
            {
                string url = "";
                if (Type == "Backup")
                {
                    url = ConfigurationManager.AppSettings["BackupUrl"];//"http://localhost:9052/api/PatchManagement/BackupFiles";
                }
                else if (Type == "Revert")
                {
                    url = ConfigurationManager.AppSettings["RevertUrl"];// "http://localhost:9052/api/PatchManagement/RevertPatch"; 
                }
                else
                {
                    url = ConfigurationManager.AppSettings["PatchReplacemetUrl"]; // "http://localhost:9052/api/PatchManagement/ReplacePatchFile"; 
                }

                var service = new
                {
                    ServiceName = "",
                    ServiceStatus = "",
                    Status = "",
                    UserId = "1",
                    DownTime = "",
                    UpTime = "",
                    Error = "",
                    SourcePath = Source,
                    DestinationPath = Distination
                };

                string jsonRequest = JsonConvert.SerializeObject(service);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return JsonConvert.SerializeObject(new { success = false, message = errorResponse });
                    }

                    string jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return jsonResponse;
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { success = false, message = ex.Message });
            }
        }


    }
}