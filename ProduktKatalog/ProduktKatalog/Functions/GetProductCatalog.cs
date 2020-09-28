using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalog.Models;
using Microsoft.Azure.Cosmos.Table;
using System.Web.Http;
using System.Collections.Generic;

namespace ProductCatalog
{
    public static class GetProductCatalog
    {
        [FunctionName("GetProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request,

            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return await MessageHandler(log);
        }

        private static async Task<IActionResult> MessageHandler(ILogger log)
        {
            CloudTable table;
            List<ProductModel> entities = new List<ProductModel>();

            // Creates or connects to a Table
            try
            {
                // Retrieve storage account information from connection string.
                var storageAccount = CloudStorageAccount.Parse(@"UseDevelopmentStorage=true");

                // Creates a table client for interacting with the table service
                var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Creates a table client for interacting with the table service 
                table = tableClient.GetTableReference("producttable");

                log.LogInformation($"Connected to {table.Name}.");
            }
            catch (Exception)
            {
                return new InternalServerErrorResult();
            }

            // Gets products from Storage Table
            TableContinuationToken token = null;
            do
            {
                var queryResult = table.ExecuteQuerySegmented(new TableQuery<ProductModel>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);
            log.LogInformation($"Got {entities.Count} entries from from {table.Name}");

            // Serialize a string with the products
            var result = JsonConvert.SerializeObject(entities);

            if (string.IsNullOrEmpty(result))
            {
                return new BadRequestResult();
            }
            return new OkObjectResult(result);
        }
    }
}
