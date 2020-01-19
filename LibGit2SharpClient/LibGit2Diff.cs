using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Domain.Diff
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly string _key;
        private readonly InsertWorkParams _params;

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
            : this(start, end, new CachedHunks(new LibGit2Hunks(start, end, repository)), key)
        {
        }

        public LibGit2Diff(Commit start, Commit end, Hunks hunks, string key)
            : this(start, end, new TotalAdditions(hunks), new TotalDeletions(hunks), key)
        {
        }

        public LibGit2Diff(Commit start, Commit end, Additions additions, Deletions deletions, string key)
            : this(start, end,
                new DefaultDiffs(key, start.Sha, end.Sha, additions.Count(), end.Author.Email,
                    deletions), key)
        {
        }

        public LibGit2Diff(Commit start, Commit end, InsertWorkParams @params, string key)
        {
            _start = start;
            _end = end;
            _key = key;
            _params = @params;
        }

        public Work WorkFrom(Works works)
        {
            return works.GetOperation().Work(_key, _start.Sha, _end.Sha);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start.Sha, _end.Sha);
        }

        public void AddTo(Entities entities, Formula formula)
        {
            _params.InsertionResult(entities, formula);
        }
    }
}