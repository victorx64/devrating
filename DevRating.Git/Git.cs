using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : Watchdog, IDisposable
    {
        private readonly Repository _repo;
        private readonly string _oldest;
        private readonly string _newest;

        public Git(string path, string oldest, string newest) : this(new Repository(path), oldest, newest)
        {
        }

        public Git(Repository repo, string oldest, string newest)
        {
            _oldest = oldest;
            _newest = newest;
            _repo = repo;
        }

        public async Task WriteInto(Log log)
        {
            foreach (var commit in Commits())
            {
                await commit.WriteInto(log);
            }
        }

        private IEnumerable<Commit> Commits()
        {
            var filter = CommitFilter();

            var options = new CompareOptions
            {
                ContextLines = 0
            };

            var commits = new List<Commit>();

            foreach (var commit in _repo.Commits.QueryBy(filter))
            {
                commits.Add(new Commit(_repo, options, commit));
            }

            return commits;
        }

        private CommitFilter CommitFilter()
        {
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Topological |
                         CommitSortStrategies.Reverse
            };

            if (!string.IsNullOrEmpty(_oldest))
            {
                filter.ExcludeReachableFrom = _oldest;
            }

            if (!string.IsNullOrEmpty(_newest))
            {
                filter.IncludeReachableFrom = _newest;
            }

            return filter;
        }

        public void Dispose()
        {
            _repo.Dispose();
        }
    }
}