using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class LibGit2Hunks : Hunks
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly IRepository _repository;

        public LibGit2Hunks(Commit start, Commit end, IRepository repository)
        {
            _start = start;
            _end = end;
            _repository = repository;
        }

        public IEnumerable<Hunk> Items()
        {
            return Task.WhenAll(ItemTasks())
                .GetAwaiter()
                .GetResult();
        }

        private IEnumerable<Task<Hunk>> ItemTasks()
        {
            var options = new BlameOptions {StartingAt = _start};

            var differences = _repository.Diff.Compare<Patch>(_start.Tree, _end.Tree,
                new CompareOptions {ContextLines = 0});

            foreach (var difference in differences.Where(IsModification))
            {
                Hunk Function()
                {
                    return new LibGit2Hunk(difference.Patch, _repository.Blame(difference.OldPath, options));
                }

                yield return Task.Run(Function);
            }

            foreach (var difference in differences.Where(IsCreation))
            {
                yield return Task.FromResult((Hunk) new LibGit2Hunk(new EmptyDeletions(),
                    new LibGit2Additions(difference.Patch)));
            }
        }

        private bool IsModification(PatchEntryChanges changes)
        {
            return !changes.IsBinaryComparison &&
                   changes.OldMode == Mode.NonExecutableFile &&
                   changes.Mode == Mode.NonExecutableFile &&
                   (changes.Status == ChangeKind.Deleted || changes.Status == ChangeKind.Modified);
        }

        private bool IsCreation(PatchEntryChanges changes)
        {
            return !changes.IsBinaryComparison &&
                   changes.Mode == Mode.NonExecutableFile &&
                   changes.OldMode == Mode.Nonexistent &&
                   changes.Status == ChangeKind.Added;
        }
    }
}