using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    internal sealed class Difference : Watchdog
    {
        private readonly IRepository _repository;
        private readonly CompareOptions _options;
        private readonly LibGit2Sharp.Commit _first;
        private readonly LibGit2Sharp.Commit _second;
        private readonly string _author;

        public Difference(IRepository repository, CompareOptions options, LibGit2Sharp.Commit first,
            LibGit2Sharp.Commit second, string author)
        {
            _repository = repository;
            _options = options;
            _first = first;
            _second = second;
            _author = author;
        }

        public async Task WriteInto(Log log)
        {
            foreach (var patch in FilePatches())
            {
                await patch.WriteInto(log);
            }
        }

        private IEnumerable<FilePatch> FilePatches()
        {
            var patches = new List<FilePatch>();

            var differences = _repository.Diff.Compare<Patch>(_second.Tree, _first.Tree, _options);

            foreach (var difference in differences)
            {
                if (!difference.IsBinaryComparison &&
                    difference.OldMode == Mode.NonExecutableFile &&
                    difference.Mode == Mode.NonExecutableFile &&
                    (difference.Status == ChangeKind.Deleted ||
                     difference.Status == ChangeKind.Modified))
                {
                    patches.Add(new FilePatch(_repository, difference, _first.Sha, _second, _author));
                }
            }

            return patches;
        }
    }
}