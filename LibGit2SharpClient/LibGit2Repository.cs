using System;
using System.Collections.Generic;
using LibGit2Sharp;
using Commit = DevRating.Domain.Git.Commit;
using Repository = DevRating.Domain.Git.Repository;

namespace DevRating.LibGit2SharpClient
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

        public IEnumerable<Commit> Commits(string since, string until)
        {
            foreach (var c in _repository.Commits.QueryBy(
                new CommitFilter
                {
                    SortBy = CommitSortStrategies.Topological,
                    ExcludeReachableFrom = since,
                    IncludeReachableFrom = until
                }))
            {
                yield return new DefaultCommit(c, _repository);
            }
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}