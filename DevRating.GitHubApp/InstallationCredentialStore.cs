using System.Threading;
using System.Threading.Tasks;
using Octokit;
using ICredentialStore = Octokit.GraphQL.ICredentialStore;

namespace DevRating.GitHubApp
{
    public class InstallationCredentialStore : ICredentialStore
    {
        private readonly long _installation;
        private readonly string _jwt;
        private readonly string _name;

        public InstallationCredentialStore(long installation, string jwt, string name)
        {
            _installation = installation;
            _jwt = jwt;
            _name = name;
        }
        
        public async Task<string> GetCredentials(CancellationToken cancellationToken = new CancellationToken())
        {
            var application = new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Credentials(_jwt, AuthenticationType.Bearer)
            };

            return (await application.GitHubApps.CreateInstallationToken(_installation)).Token;
        }
    }
}