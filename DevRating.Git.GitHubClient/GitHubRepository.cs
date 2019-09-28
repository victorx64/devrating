using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit.GraphQL;

namespace DevRating.Git.GitHubClient
{
    public class GitHubRepository : Repository
    {
        private readonly Connection _connection;

        public GitHubRepository(Connection connection)
        {
            _connection = connection;
        }
        
        public IEnumerable<Task<Watchdog>> FilePatches(string sha)
        {
            throw new System.NotImplementedException();
        }

        public string Author(string sha)
        {
            throw new System.NotImplementedException();
        }
    }
}