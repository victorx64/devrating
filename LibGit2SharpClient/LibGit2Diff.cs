using System.Linq;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Domain.Diff
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly string _key;
        private readonly Additions _additions;
        private readonly Deletions _deletions;

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
        {
            _start = start;
            _end = end;
            _additions = additions;
            _deletions = deletions;
            _key = key;
        }

        public Work WorkFrom(Works works)
        {
            return works.Work(_key, _start.Sha, _end.Sha);
        }

        public bool ExistIn(Works works)
        {
            return works.Contains(_key, _start.Sha, _end.Sha);
        }

        public void AddTo(Diffs diffs)
        {
            diffs.Insert(_key, _start.Sha, _end.Sha, _end.Author.Email, _additions.Count(), _deletions.Items());
        }
    }
}