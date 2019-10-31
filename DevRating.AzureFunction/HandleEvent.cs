using System;
using System.IO;
using System.Threading.Tasks;
using DevRating.GitHubApp;
using DevRating.GitHubApp.Models;
using DevRating.Rating;
using DevRating.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Octokit.Internal;

namespace DevRating.AzureFunction
{
    public static class HandleEvent
    {
        [FunctionName("HandleEvent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                var @event = request.Headers["X-GitHub-Event"];

                if (@event.Equals("push"))
                {
                    var serializer = new SimpleJsonSerializer();

                    var payload =
                        serializer.Deserialize<PushWebhookPayload>(new StreamReader(request.Body).ReadToEnd());

                    var token = new JsonWebToken(42098,
                        Path.Combine(context.FunctionAppDirectory, "PrivateKey",
                            "devrating.2019-09-26.private-key.pem"));

                    var storage =
                        new SqlModificationsStorage(Environment.GetEnvironmentVariable("AzureWebJobsStorage")!,
                            new EloFormula());

                    await new Application(token, "DevRating", storage)
                        .HandlePushEvent(payload, Path.GetTempPath());
                }

                return new OkObjectResult(@event);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}