using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DevRating.GitHubApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Application = DevRating.GitHubApp.Application;

namespace DevRating.AzureFunction
{
    public static class HandleEvent
    {
        [FunctionName("HandleEvent")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request, ExecutionContext context, ILogger log)
        {
            var @event = request.Headers["X-GitHub-Event"];
            
            if (@event.Equals("push"))
            {
                var payload = JsonSerializer.Deserialize<PushEventPayload>(new StreamReader(request.Body).ReadToEnd());

                await new Application(Path.Combine(context.FunctionDirectory, "(PrivateKey)", "devrating.2019-09-26.private-key.pem"))
                    .HandlePushEvent(payload);
            }
            
            return new OkObjectResult(@event);
        }
    }
}