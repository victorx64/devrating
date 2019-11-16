using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Domain.Diff
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly IRepository _repository;

        public LibGit2Diff(string start, string end, string path) : this(start, end, new Repository(path))
        {
        }

        public LibGit2Diff(string start, string end, IRepository repository)
            : this(repository.Lookup<Commit>(start), repository.Lookup<Commit>(end), repository)
        {
        }

        public LibGit2Diff(Commit start, Commit end, IRepository repository)
        {
            _start = start;
            _end = end;
            _repository = repository;
        }

        public WorkKey Key()
        {
            return new LibGit2WorkKey(_start, _end, _repository);
        }

        public void AddTo(WorksRepository works)
        {
            var hunks = Task.WhenAll(HunkTasks()).GetAwaiter().GetResult();

            works.Add(Key(), new LibGit2Modification(Author(), Additions(hunks)), Deletions(hunks));
        }

        private Signature Author()
        {
            return _repository.Mailmap.ResolveSignature(_end.Author);
        }

        private uint Additions(IEnumerable<Hunk> hunks)
        {
            var additions = 0u;

            foreach (var h in hunks)
            {
                additions += h.Additions().Count();
            }

            return additions;
        }

        private IEnumerable<Modification> Deletions(IEnumerable<Hunk> hunks)
        {
            foreach (var hunk in hunks)
            {
                foreach (var deletions in hunk.Deletions().Items())
                {
                    yield return deletions;
                }
            }
        }

        private IEnumerable<Task<LibGit2Hunk>> HunkTasks()
        {
            var options = new BlameOptions {StartingAt = _start};

            foreach (var difference in Differences())
            {
                LibGit2Hunk Function()
                {
                    return new LibGit2Hunk(_repository, difference.Patch,
                        _repository.Blame(difference.OldPath, options));
                }

                yield return Task.Run(Function);
            }
        }

        private IEnumerable<PatchEntryChanges> Differences()
        {
            foreach (var difference in _repository.Diff.Compare<Patch>(_start.Tree, _end.Tree,
                new CompareOptions {ContextLines = 0}))
            {
                if (!difference.IsBinaryComparison &&
                    difference.OldMode == Mode.NonExecutableFile &&
                    difference.Mode == Mode.NonExecutableFile &&
                    (difference.Status == ChangeKind.Deleted ||
                     difference.Status == ChangeKind.Modified))
                {
                    yield return difference;
                }
            }
        }
    }
}