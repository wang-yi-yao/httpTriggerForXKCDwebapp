using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace backend.Function
{
    public static class httpTrigger
    {

        [FunctionName("httpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //create new http object
            HttpClient client = new HttpClient(); 
            log.LogInformation("C# HTTP trigger function received a request.");

            //extract number from HTTP request query
            string num = req.Query["num"];

            //if no number is given, return an error
            if (num == null) {
                string noNumResponse = "{\"error\":\"no number given\"}";
                return new OkObjectResult(noNumResponse);
            }

            //initalise request url
            string requestComic = "";

            //if number is 0, return the latest comic
            //otherwise, return the specified comic
            if (Int32.Parse(num) == 0) {
                requestComic = "https://xkcd.com/info.0.json";
            } else {
                requestComic = $"https://xkcd.com/{num}/info.0.json";
            }
            string response = await client.GetStringAsync(requestComic);
            return new OkObjectResult(response);
        }
    }
}
