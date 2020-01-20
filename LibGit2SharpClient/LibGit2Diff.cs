using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;
using Diff = DevRating.Domain.Diff;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Diff
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly Additions _additions;
        private readonly Deletions _deletions;
        private readonly string _key;

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

        public Work From(Works works)
        {
            return works.GetOperation().Work(_key, _start.Sha, _end.Sha);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start.Sha, _end.Sha);
        }

        public void AddTo(EntitiesFactory factory)
        {
            var work = factory.InsertedWork(_key, _start.Sha, _end.Sha, _end.Author.Email, _additions.Count());

            factory.InsertRatings(_end.Author.Email, _deletions.Items(), work);
        }
    }
}