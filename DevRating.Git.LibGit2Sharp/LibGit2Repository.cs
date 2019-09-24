using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git.LibGit2Sharp
{
    public class LibGit2Repository : Repository, IDisposable
    {
        private readonly IRepository _repository;

        public LibGit2Repository(string path) : this(new global::LibGit2Sharp.Repository(path))
        {
        }

        public LibGit2Repository(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Task<Watchdog>> FilePatches(string sha)
        {
            var commit = _repository.Lookup<global::LibGit2Sharp.Commit>(sha);

            var options = new CompareOptions
            {
                ContextLines = 0
            };

            foreach (var parent in commit.Parents)
            {
                var differences = _repository.Diff.Compare<Patch>(parent.Tree, commit.Tree, options);

                foreach (var difference in differences)
                {
                    if (!difference.IsBinaryComparison &&
                        difference.OldMode == Mode.NonExecutableFile &&
                        difference.Mode == Mode.NonExecutableFile &&
                        (difference.Status == ChangeKind.Deleted ||
                         difference.Status == ChangeKind.Modified))
                    {
                        yield return Task.Run(() =>
                            (Watchdog) new FilePatch(difference.Patch,
                                new LibGit2Blame(difference.OldPath, parent.Sha, _repository)));
                    }
                }
            }
        }

        public string Author(string sha)
        {
            var sign = _repository.Lookup<global::LibGit2Sharp.Commit>(sha).Author;

            var resolved = _repository.Mailmap.ResolveSignature(sign);

            return resolved.Email;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}