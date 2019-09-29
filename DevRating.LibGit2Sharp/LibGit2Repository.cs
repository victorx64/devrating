using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Git;
using LibGit2Sharp;
using Repository = DevRating.Git.Repository;

namespace DevRating.LibGit2Sharp
{
    public sealed class LibGit2Repository : Repository, IDisposable
    {
        private readonly IRepository _repository;

        public LibGit2Repository(string path) : this(new global::LibGit2Sharp.Repository(path))
        {
        }

        public LibGit2Repository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Modifications> Modifications(ModificationsFactory factory, string sha)
        {
            var commit = _repository.Lookup<Commit>(sha);

            var author = _repository.Mailmap.ResolveSignature(commit.Author).Email;
            
            var modifications = factory.Modifications(sha, author);

            var tasks = new List<Task>();

            foreach (var task in FilePatches(commit))
            {
                tasks.Add(task.ContinueWith(task1 => task1.Result.WriteInto(modifications)));
            }

            await Task.WhenAll(tasks);

            return modifications;
        }
        
        private IEnumerable<Task<FilePatch>> FilePatches(Commit commit)
        {
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
                        {
                            var collection = _repository.Blame(difference.OldPath, new BlameOptions {StartingAt = parent.Sha});

                            return new FilePatch(difference.Patch, collection, _repository);
                        });
                    }
                }
            }
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}