using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    internal sealed class Commit : Watchdog
    {
        private readonly IRepository _repository;
        private readonly CompareOptions _options;
        private readonly LibGit2Sharp.Commit _commit;

        public Commit(IRepository repository, CompareOptions options, LibGit2Sharp.Commit commit)
        {
            _repository = repository;
            _options = options;
            _commit = commit;
        }

        public async Task WriteInto(History history)
        {
            foreach (var difference in DifferencesFromParents())
            {
                await difference.WriteInto(history);
            }
        }

        private IEnumerable<Difference> DifferencesFromParents()
        {
            var differences = new List<Difference>();

            foreach (var parent in _commit.Parents)
            {
                differences.Add(new Difference(_repository, _options, _commit, parent));
            }

            return differences;
        }
    }
}