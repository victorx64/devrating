using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GitHubJwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Octokit;

namespace DevRating.GitHubApp
{
    public static class Application
    {
        [FunctionName("HandleEvent")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request, ExecutionContext context, ILogger log)
        {
            if (!request.Headers["X-GitHub-Event"].Equals("push"))
            {
                return new OkObjectResult(request.Headers["X-GitHub-Event"]);
            }

            var payload = JsonSerializer.Deserialize<PushEventPayload>(new StreamReader(request.Body).ReadToEnd());

            var installation = await InstallationClient(JwToken(context), payload.Installation.Id);

            var commit = payload.Commits.First().Id;

            await installation.Repository.Comment.Create("victorx64", "test",
                commit, new NewCommitComment("GREAT JOB!"));

            return new OkObjectResult($"Commenting");
        }

        private static string JwToken(ExecutionContext context)
        {
            var path = Path.Combine(context.FunctionAppDirectory, "private-key",
                "devrating.2019-09-26.private-key.pem");

            var generator = new GitHubJwtFactory(
                new FilePrivateKeySource(path),
                new GitHubJwtFactoryOptions
                {
                    AppIntegrationId = 42098,
                    ExpirationSeconds = 600
                }
            );

            return generator.CreateEncodedJwtToken();
        }

        private static async Task<GitHubClient> InstallationClient(string jwToken, long installation)
        {
            var app = new GitHubClient(new ProductHeaderValue("DevRating"))
            {
                Credentials = new Credentials(jwToken, AuthenticationType.Bearer)
            };

            var response = await app.GitHubApps.CreateInstallationToken(installation);

            return new GitHubClient(new ProductHeaderValue("DevRating"))
            {
                Credentials = new Credentials(response.Token)
            };
        }
    }

    internal class PushEventPayload
    {
        public List<Commit> Commits { get; set; } = new List<Commit>();

        public Installation Installation { get; set; } = new Installation();
    }

    internal class Commit
    {
        public string Id { get; set; } = string.Empty;
    }

    internal class Installation
    {
        public long Id { get; set; } = 0;
    }
}