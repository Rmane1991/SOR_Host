using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Monitoring
{
    public partial class AppManagement : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        [System.Web.Services.WebMethod]
        public static async Task<string> GetUpdatedServices_()
        {
            var services = await FetchServicesFromApiAsync();
            return JsonConvert.SerializeObject(services);
        }

        private static async Task<List<Service>> FetchServicesFromApiAsync()
        {
            string url = ConfigurationManager.AppSettings["ServiceStatusUrl"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Service>>(json);
                }
                catch (HttpRequestException e)
                {

                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
                catch (JsonException e)
                {

                    Console.WriteLine($"JSON error: {e.Message}");
                    return null;
                }
                catch (Exception e)
                {

                    Console.WriteLine($"An unexpected error occurred: {e.Message}");
                    return null;
                }
            }
        }



        [System.Web.Services.WebMethod]
        public static string GetUpdatedServices()
        {
            var services = FetchServicesFromApi();
            return JsonConvert.SerializeObject(services);
        }

        public static List<Service> FetchServicesFromApi()
        {
            string url = ConfigurationManager.AppSettings["ServiceStatusUrl"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(url).GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<List<Service>>(json);
            }
        }

        [System.Web.Services.WebMethod]
        public static string StartService(string serviceName, string status, string error)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["StartServicesUrl"];
                var services = new List<Service>
        {
            new Service
            {
                ServiceName = serviceName,
                ServiceStatus = status,
                UserId = "1",
                DownTime = "",
                UpTime = "",
                Error = ""
            }
        };
                string jsonRequest = JsonConvert.SerializeObject(services);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Console.WriteLine("Error Response: " + errorResponse);
                        return errorResponse;
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

        [System.Web.Services.WebMethod]
        public static string StopService(string serviceName, string status, string error)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["StopServiceUrl"];//"http://localhost:9852/api/CustServices/ServiceStop";
                var services = new List<Service>
        {
            new Service
            {
                ServiceName = serviceName,
                ServiceStatus = status,
                UserId = "1",
                DownTime = "",
                UpTime = "",
                Error = ""
            }
        };
                string jsonRequest = JsonConvert.SerializeObject(services);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Console.WriteLine("Error Response: " + errorResponse);
                        return errorResponse;
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

        [System.Web.Services.WebMethod]
        public static string ManageServices(List<Service> services, string Type)
        {
            try
            {
                string url = "";
                if (Type == "Start")
                {
                    url = ConfigurationManager.AppSettings["StartServicesUrl"];
                }
                else
                {
                    url = ConfigurationManager.AppSettings["StopServiceUrl"];
                }
                   
                string jsonRequest = JsonConvert.SerializeObject(services);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return errorResponse;
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




        private async Task<string> CallApiAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {

                    return $"Error: {ex.Message}";
                }
            }
        }


        private List<Service> GetTestServices()
        {
            return new List<Service>
    {
        new Service { ServiceName = "Test Service 1", ServiceStatus="Running", Error="",UpTime = "99.9%", DownTime = "0.1%" },
        new Service { ServiceName = "Test Service 2", ServiceStatus="Running", Error="",UpTime = "95.0%", DownTime = "5.0%" },
        new Service { ServiceName = "Test Service 3", ServiceStatus="Running", Error="",UpTime = "97.5%", DownTime = "2.5%" },
    };
        }


        public class Service
        {
            public string ServiceName { get; set; }
            public string DownTime { get; set; }
            public string UpTime { get; set; }
            public string Error { get; set; }
            public string ServiceStatus { get; set; }
            public string Status { get; set; }
            public string UserId { get; set; }
             
        }
    }
}
