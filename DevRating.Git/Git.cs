using System;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : Watchdog, IDisposable
    {
        private readonly IRepository _repository;
        private readonly string _commit;

        public Git(string path, string commit) : this(new Repository(path), commit)
        {
        }

        public Git(IRepository repository, string commit)
        {
            _repository = repository;
            _commit = commit;
        }

        public Task WriteInto(Log log)
        {
            return Commit().WriteInto(log);
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