using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Domain.Diff, IDisposable
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly IRepository _repository;
        private readonly string _key;

        public LibGit2Diff(string path, string start, string end) : this(start, end, new Repository(path))
        {
        }

        public LibGit2Diff(string path, string start, string end, string key)
            : this(start, end, new Repository(path), key)
        {
        }

        public LibGit2Diff(string start, string end, IRepository repository)
            : this(repository.Lookup<Commit>(start),
                repository.Lookup<Commit>(end),
                repository,
                repository.Network.Remotes.First().Url)
        {
        }

        public LibGit2Diff(string start, string end, IRepository repository, string key)
            : this(repository.Lookup<Commit>(start), repository.Lookup<Commit>(end), repository, key)
        {
        }

        public LibGit2Diff(Commit start, Commit end, IRepository repository, string key)
        {
            _start = start;
            _end = end;
            _repository = repository;
            _key = key;
        }

        public string RepositoryName()
        {
            return _key;
        }

        public string StartCommit()
        {
            return _start.Sha;
        }

        public string EndCommit()
        {
            return _end.Sha;
        }

        public void AddTo(Diffs diffs)
        {
            var hunks = Task.WhenAll(HunkTasks()).GetAwaiter().GetResult();

            diffs.Insert(RepositoryName(), StartCommit(), EndCommit(), _end.Author.Email, Additions(hunks), Deletions(hunks));
        }

        private uint Additions(IEnumerable<Hunk> hunks)
        {
            return (uint) hunks.Sum(HunkAdditions);
        }

        private long HunkAdditions(Hunk hunk)
        {
            return hunk.Additions().Count();
        }

        // TODO Extract the region

        #region Converts hunks into a deletions dictionary

        private IDictionary<string, uint> Deletions(IEnumerable<Hunk> hunks)
        {
            return hunks
                .SelectMany(HunkDeletions)
                .GroupBy(ModificationAuthor)
                .ToDictionary(ModificationsAuthor, ModificationCountsSum);
        }

        private IEnumerable<Modification> HunkDeletions(Hunk hunk)
        {
            return hunk.Deletions().Items();
        }

        private string ModificationAuthor(Modification modification)
        {
            return modification.Email();
        }

        private string ModificationsAuthor(IGrouping<string, Modification> grouping)
        {
            return grouping.Key;
        }

        private uint ModificationCountsSum(IGrouping<string, Modification> grouping)
        {
            return (uint) grouping.Sum(ModificationCount);
        }

        private long ModificationCount(Modification modification)
        {
            return modification.Count();
        }

        #endregion

        private IEnumerable<Task<LibGit2Hunk>> HunkTasks()
        {
            var options = new BlameOptions {StartingAt = _start};

            foreach (var difference in Differences())
            {
                LibGit2Hunk Function()
                {
                    return new LibGit2Hunk(difference.Patch, _repository.Blame(difference.OldPath, options));
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

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}