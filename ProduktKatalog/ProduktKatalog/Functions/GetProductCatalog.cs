using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProduktKatalog.Functions;
using System.IO;
using System.Threading.Tasks;

namespace ProduktKatalog
{
    public static class GetProductCatalog
    {
        [FunctionName("GetProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,

            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            


            return new OkObjectResult("");
        }
    }
}
