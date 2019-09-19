using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    internal sealed class CommitsDifference : Watchdog
    {
        private readonly IRepository _repo;
        private readonly CompareOptions _options;
        private readonly LibGit2Sharp.Commit _commit;
        private readonly LibGit2Sharp.Commit _parent;
        private readonly string _author;

        public CommitsDifference(IRepository repo, CompareOptions options, LibGit2Sharp.Commit commit,
            LibGit2Sharp.Commit parent, string author)
        {
            _repo = repo;
            _options = options;
            _commit = commit;
            _parent = parent;
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

            var differences = _repo.Diff.Compare<Patch>(_parent.Tree, _commit.Tree, _options);

            foreach (var difference in differences)
            {
                if (!difference.IsBinaryComparison &&
                    difference.OldMode == Mode.NonExecutableFile &&
                    difference.Mode == Mode.NonExecutableFile &&
                    (difference.Status == ChangeKind.Deleted ||
                     difference.Status == ChangeKind.Modified))
                {
                    patches.Add(new FilePatch(_repo, difference, _commit, _parent, _author));
                }
            }

            return patches;
        }
    }
}