using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    internal sealed class Commit : Watchdog
    {
        private readonly IRepository _repo;
        private readonly CompareOptions _options;
        private readonly LibGit2Sharp.Commit _commit;

        public Commit(IRepository repo, CompareOptions options, LibGit2Sharp.Commit commit)
        {
            _repo = repo;
            _options = options;
            _commit = commit;
        }

        public async Task WriteInto(Log log)
        {
            foreach (var difference in ParentsDifferences())
            {
                await difference.WriteInto(log);
            }
            
            await log.Save();
        }

        private IEnumerable<CommitsDifference> ParentsDifferences()
        {
            var differences = new List<CommitsDifference>();

            var author = new Author(_repo, _commit.Author);

            foreach (var parent in _commit.Parents)
            {
                differences.Add(new CommitsDifference(_repo, _options, _commit, parent, author.Email()));
            }

            return differences;
        }
    }
}