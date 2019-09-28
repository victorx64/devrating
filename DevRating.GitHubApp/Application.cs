using System.Threading.Tasks;
using Connection = Octokit.GraphQL.Connection;
using ProductHeaderValue = Octokit.GraphQL.ProductHeaderValue;

namespace DevRating.GitHubApp
{
    public class Application
    {
        private readonly string _path;

        public Application(string path)
        {
            _path = path;
        }

        public async Task HandlePushEvent(PushEventPayload payload)
        {
            var connection = Connection(payload.Installation.Id);
        }

        private Connection Connection(long installation)
        {
            var token = new JsonWebToken(42098, _path);

            var name = "DevRating";

            return new Connection(new ProductHeaderValue(name),
                new InstallationCredentialStore(installation, token.Value(), name));
        }
    }
}