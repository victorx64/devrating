using System;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : Watchdog, IDisposable
    {
        private readonly Repository _repo;
        private readonly string _commit;

        public Git(string path, string commit) : this(new Repository(path), commit)
        {
        }

        public Git(Repository repo, string commit)
        {
            _repo = repo;
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

            return new Commit(_repo, options, _repo.Lookup<LibGit2Sharp.Commit>(_commit));
        }

        public void Dispose()
        {
            _repo.Dispose();
        }
    }
}