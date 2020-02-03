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
        private readonly string _organization;
        private readonly Envelope _link;

        public LibGit2Diff(string start, string end, IRepository repository, string key, Envelope link,
            string organization)
            : this(repository.Lookup<Commit>(start), repository.Lookup<Commit>(end), repository, key, link,
                organization)
        {
        }

        public LibGit2Diff(Commit start, Commit end, IRepository repository, string key, Envelope link,
            string organization)
            : this(start, end, new CachedHunks(new LibGit2Hunks(start, end, repository)), key, link, organization)
        {
        }

        public LibGit2Diff(Commit start, Commit end, Hunks hunks, string key, Envelope link, string organization)
            : this(start, end, new TotalAdditions(hunks), new TotalDeletions(hunks), key, link, organization)
        {
        }

        public LibGit2Diff(Commit start, Commit end, Additions additions, Deletions deletions, string key,
            Envelope link, string organization)
        {
            _start = start;
            _end = end;
            _additions = additions;
            _deletions = deletions;
            _key = key;
            _link = link;
            _organization = organization;
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_key, _start.Sha, _end.Sha);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start.Sha, _end.Sha);
        }

        public void AddTo(EntityFactory factory)
        {
            var work = factory.InsertedWork(_organization, _key, _start.Sha, _end.Sha, _end.Author.Email,
                _additions.Count(), _link);

            factory.InsertRatings(_organization, _end.Author.Email, _deletions.Items(), work.Id());
        }
    }
}