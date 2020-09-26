using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using ProduktKatalog.Models;

namespace ProduktKatalog.Functions
{
    public static class PostProductCatalog
    {
        [FunctionName("PostProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await messageHandler(request, log);

            return new OkObjectResult("");
        }

        private static async Task messageHandler(HttpRequest request, ILogger log)
        {
            List<ProductModel> products = new List<ProductModel>();


        }
    }
}
