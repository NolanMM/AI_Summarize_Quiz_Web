using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTestAPI.HomeControllerRequest
{
    public class TestHomeController
    {
        private HttpClient client = new HttpClient();
        private string domain_url = "http://localhost:7125";

        public async Task<string> TestIndexRoute()
        {
            var response = await client.GetAsync(domain_url + "/");
            response.EnsureSuccessStatusCode();
            return response.StatusCode.ToString();
        }
    }
}
