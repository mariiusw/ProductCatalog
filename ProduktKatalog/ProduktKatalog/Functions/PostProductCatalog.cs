using System;
using System.IO;
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

namespace ProductCatalog.Functions
{
    public static class PostProductCatalog
    {
        [FunctionName("PostProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return await MessageHandler(request, log);
        }

        private static async Task<IActionResult> MessageHandler(HttpRequest request, ILogger log)
        {
            ProductModel product;
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            
            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("Request body was empty.");
            }

            // Deserializes and parses the request body to a product object
            try
            {
                product = JsonConvert.DeserializeObject<ProductModel>(requestBody);
                log.LogInformation("Json parsed successfully.");
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Failed to deserialize Json object from request body.");
            }

            return await StoreProductToTable(product, log);
        }

        private static async Task<IActionResult> StoreProductToTable(ProductModel product, ILogger log)
        {
            CloudTable table;
            var tableName = "producttable";

            // Creates or connects to a Table
            try
            {
                // Retrieve storage account information from connection string.
                var storageAccount = CloudStorageAccount.Parse(@"UseDevelopmentStorage=true");

                // Creates a table client for interacting with the table service
                var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Creates a table client for interacting with the table service 
                table = tableClient.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync();
                
                log.LogInformation($"Connected to {table.Name}.");
            }
            catch (Exception)
            {
                return new InternalServerErrorResult();
            }

            // Creates an object with PartitionKey and RowKey for the Table
            var productItem = new ProductModel(product.ProductId, DateTime.UtcNow)
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };

            // Sends data to the Table
            try
            {
                // Creates the table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(productItem);

                // Executes the operation
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                
                log.LogInformation($"Data added to {table.Name}.");
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Error during insert or merge operation.");
            }

            return new OkObjectResult($"Posted a new product to the {table.Name}.");
        }
    }
}
