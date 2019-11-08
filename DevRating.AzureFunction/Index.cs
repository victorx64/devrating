using System;
using System.Text;
using DevRating.SqlClient;
using DevRating.WebApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DevRating.AzureFunction
{
    public static class Index
    {
        [FunctionName("Index")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var application =
                new Application(new SqlAuthorsRepository(Environment.GetEnvironmentVariable("AzureWebJobsStorage")!));

            var builder = new StringBuilder();

            foreach (var author in application.TopAuthors())
            {
                builder.AppendLine($"{author.Email()} {author.LastReward().Rating().Value():F2}");
            }

            return new OkObjectResult(builder.ToString());
        }
    }
}