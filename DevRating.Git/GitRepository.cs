using System;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class GitRepository : IDisposable
    {
        private readonly IRepository _repository;
        private readonly string _commit;

        public GitRepository(string path, string commit) : this(new Repository(path), commit)
        {
        }

        public GitRepository(IRepository repository, string commit)
        {
            _repository = repository;
            _commit = commit;
        }

        public async Task<History> History(HistoryFactory factory)
        {
            var history = factory.History(_commit, AuthorEmail());

            await Commit()
                .WriteInto(history);

            return history;
        }

        private string AuthorEmail()
        {
            return new Author(_repository, _repository.Lookup<LibGit2Sharp.Commit>(_commit).Author)
                .Email();
        }

        private Commit Commit()
        {
            var options = new CompareOptions
            {
                ContextLines = 0
            };

            return new Commit(_repository, options, _repository.Lookup<LibGit2Sharp.Commit>(_commit));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}