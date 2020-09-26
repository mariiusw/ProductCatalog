using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

            await MessageHandler(request, log);

            return new OkObjectResult("");
        }

        private static Task MessageHandler(HttpRequest request, ILogger log)
        {
            throw new NotImplementedException();
        }
    }
}
