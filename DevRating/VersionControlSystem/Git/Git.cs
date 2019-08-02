using System;
using System.Collections.Generic;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.VersionControlSystem.Git
{
    public class Git : IVersionControlSystem
    {
        private readonly IRating _rating;

        public Git(IRating rating)
        {
            _rating = rating;
        }

        public IRating UpdatedRating()
        {
            var rating = _rating;

            var files = new Dictionary<string, File>();

            using (var repo = new Repository("."))
            {
                var options = new CompareOptions()
                {
                    ContextLines = 0
                };

                var filter = new CommitFilter
                {
                    SortBy = CommitSortStrategies.Topological |
                             CommitSortStrategies.Reverse
                };

                Tree tree = null;

                var i = 0;

                foreach (var current in repo.Commits.QueryBy(filter))
                {
                    Console.WriteLine(++i);

                    var differences = repo.Diff.Compare<Patch>(tree, current.Tree, options);

                    UpdateRating(files, current.Author.Email, differences, rating);

                    tree = current.Tree;
                }
            }

            return rating;
        }

        private void UpdateRating(IDictionary<string, File> files, string author, Patch differences, IRating rating)
        {
            var before = new Dictionary<string, File>(files);

            foreach (var difference in differences)
            {
                switch (difference.Status)
                {
                    case ChangeKind.Added:
                    {
                        var file = new File(new string[0], difference.IsBinaryComparison);

                        UpdateFile(file, author, difference.Patch);

                        files.Add(difference.Path, file);
                    }
                        break;
                    case ChangeKind.Deleted:
                    {
                        var file = new File(before[difference.OldPath].Authors(),before[difference.OldPath].Binary() || difference.IsBinaryComparison);

                        UpdateFile(file, author, difference.Patch);

                        files.Remove(difference.OldPath);
                    }
                        break;
                    case ChangeKind.Modified:
                    {
                        var file = new File(before[difference.OldPath].Authors(),before[difference.OldPath].Binary() || difference.IsBinaryComparison);

                        UpdateFile(file, author, difference.Patch);

                        files.Remove(difference.OldPath);

                        files.Add(difference.Path, file);
                    }
                        break;
                    case ChangeKind.Renamed:
                    {
                        var file = new File(before[difference.OldPath].Authors(),before[difference.OldPath].Binary() || difference.IsBinaryComparison);

                        UpdateFile(file, author, difference.Patch);

                        files.Remove(difference.OldPath);

                        files.Add(difference.Path, file);
                    }
                        break;
                    case ChangeKind.Copied:
                    {
                        var file = new File(before[difference.OldPath].Authors(),before[difference.OldPath].Binary() || difference.IsBinaryComparison);

                        UpdateFile(file, author, difference.Patch);

                        files.Add(difference.Path, file);
                    }
                        break;
                    case ChangeKind.Ignored:
                    case ChangeKind.Untracked:
                    case ChangeKind.TypeChanged:
                    case ChangeKind.Unreadable:
                    case ChangeKind.Conflicted:
                    case ChangeKind.Unmodified:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void UpdateFile(File file, string author, string patch)
        {
            var lines = patch.Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ');

                    file.AddDeletion(new LinesBlock(author, parts[1]));
                    file.AddAddition(new LinesBlock(author, parts[2]));
                }
            }
        }
    }
}