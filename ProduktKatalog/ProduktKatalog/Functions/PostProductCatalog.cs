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
using ProductCatalog.Models;

namespace ProductCatalog.Functions
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

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                throw new System.ArgumentNullException("Request body was empty.");
            }

            ProductModel product = null;

            try
            {
                product = JsonConvert.DeserializeObject<ProductModel>(requestBody);
                products.Add(product);
            }
            catch (JsonSerializationException jse)
            {

                throw new JsonSerializationException("Failed to deserialize Json object from request body", jse);
            }

            await storeProductToTable(products, log);
        }

        private static Task storeProductToTable(List<ProductModel> products, ILogger log)
        {
            
        }
    }
}
