using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Domain.Git;
using LibGit2Sharp;
using Commit = DevRating.Domain.Git.Commit;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class DefaultCommit : Commit
    {
        private readonly LibGit2Sharp.Commit _commit;
        private readonly IRepository _repository;

        public DefaultCommit(LibGit2Sharp.Commit commit, IRepository repository)
        {
            _commit = commit;
            _repository = repository;
        }

        public string Sha()
        {
            return _commit.Sha;
        }

        public string RepositoryFirstUrl()
        {
            return _repository.Network.Remotes.First().Url;
        }

        public string Author()
        {
            return _repository.Mailmap.ResolveSignature(_commit.Author).Email;
        }

        public async Task WriteInto(ModificationsCollection modifications)
        {
            await Task.WhenAll(WriteCommitTasks(modifications));
        }

        private IEnumerable<Task> WriteCommitTasks(ModificationsCollection modifications)
        {
            foreach (var parent in _commit.Parents)
            {
                foreach (var task in WriteDifferencesTasks(modifications, parent))
                {
                    yield return task;
                }
            }
        }

        private IEnumerable<Task> WriteDifferencesTasks(ModificationsCollection modifications,
            global::LibGit2Sharp.Commit parent)
        {
            var differences = _repository.Diff.Compare<Patch>(
                parent.Tree,
                _commit.Tree,
                new CompareOptions {ContextLines = 0});

            var options = new BlameOptions {StartingAt = parent};

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
                        var blames = _repository.Blame(difference.OldPath, options);

                        WritePatchInto(modifications, difference.Patch, blames);
                    });
                }
            }
        }

        private void WritePatchInto(ModificationsCollection modifications, string patch, BlameHunkCollection blames)
        {
            foreach (var line in patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    // line must be like "@@ -3,9 +3,9 @@ blah..."
                    // TODO Throw exception on a contextual line. Patch must be without contextual lines (git log -U0)
                    var parts = line.Split(' ');

                    WriteDeletionInto(modifications, parts[1], blames);
                    WriteAdditionInto(modifications, parts[2]);
                }
            }
        }

        private void WriteDeletionInto(ModificationsCollection modifications, string hunk, BlameHunkCollection blames)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToUInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToUInt32(parts[1]);

            uint d;

            for (var i = index; i < index + count; i += d)
            {
                var blame = blames.HunkForLine((int) i);
                d = Math.Min((uint) (blame.FinalStartLineNumber + blame.LineCount), index + count) - i;

                var previous = new DefaultCommit(blame.FinalCommit, _repository);

                modifications.AddDeletion(new DefaultDeletion(this, previous, d));
            }
        }

        private void WriteAdditionInto(ModificationsCollection modifications, string hunk)
        {
            modifications.AddAddition(new DefaultAddition(this, AdditionsCount(hunk)));
        }

        private uint AdditionsCount(string hunk)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            var count = parts.Length == 1 ? 1 : Convert.ToUInt32(parts[1]);

            return count;
        }

        public override string ToString()
        {
            return Sha();
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is DefaultCommit other && Equals(other);
        }

        private bool Equals(DefaultCommit other)
        {
            return string.Equals(Sha(), other.Sha(), StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(RepositoryFirstUrl(), other.RepositoryFirstUrl(), StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(Sha());
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(RepositoryFirstUrl());
                return hashCode;
            }
        }
    }
}