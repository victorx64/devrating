using System.Threading.Tasks;
using DevRating.AzureTable;
using DevRating.Game;
using DevRating.Git;
using DevRating.Git.GitHubClient;
using DevRating.Rating;
using Octokit;

namespace DevRating.GitHubApp
{
    public class Application
    {
        private readonly string _token;
        private readonly string _name = "DevRating";

        public Application(string path)
        {
            _token = new JsonWebToken(42098, path).Value();
        }

        public async Task HandlePushEvent(Octokit.PushEventPayload payload)
        {
            var installation = await InstallationClient(payload.Installation.Id);

            var connection = GqlConnection(payload.Installation.Id);

            var matches = new AzureMatches("", "key", "match", 1200d);

            var factory = new GamesFactory(new EloFormula(), 2000d);

            foreach (var commit in payload.Commits)
            {
                var games = (Games) factory.Modifications(commit.Sha, commit.Author.Email);
                
                var files = (await installation.Repository.Commit.Get(commit.Repository.Id, commit.Sha)).Files;

                foreach (var file in files)
                {
                    var patch = new FilePatch(file.Patch, new GitHubBlame());
                    
                    patch.WriteInto(games);
                }

                await games.PushInto(matches);
            }
        }

        private async Task<GitHubClient> InstallationClient(long installation)
        {
            var app = new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Credentials(_token, AuthenticationType.Bearer)
            };

            var response = await app.GitHubApps.CreateInstallationToken(installation);

            return new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Credentials(response.Token, AuthenticationType.Oauth)
            };
        }

        private Octokit.GraphQL.Connection GqlConnection(long installation)
        {
            return new Octokit.GraphQL.Connection(new Octokit.GraphQL.ProductHeaderValue(_name),
                new InstallationCredentialStore(installation, _token, _name));
        }
    }
}