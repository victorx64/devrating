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
        private readonly Envelope _since;
        private readonly Additions _additions;
        private readonly Deletions _deletions;
        private readonly string _key;
        private readonly string _organization;
        private readonly Envelope _link;

        public LibGit2Diff(
            string start,
            string end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization)
            : this(
                repository.Lookup<Commit>(start),
                repository.Lookup<Commit>(end),
                since,
                repository, key, link,
                organization
            )
        {
        }

        public LibGit2Diff(
            Commit start,
            Commit end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization
        )
            : this(
                start,
                end,
                since,
                new CachedHunks(new LibGit2Hunks(start, end, since, repository)),
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            Commit start,
            Commit end,
            Envelope since,
            Hunks hunks,
            string key,
            Envelope link,
            string organization
        )
            : this(
                start,
                end,
                since,
                new TotalAdditions(hunks),
                new TotalDeletions(hunks),
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            Commit start,
            Commit end,
            Envelope since,
            Additions additions,
            Deletions deletions,
            string key,
            Envelope link,
            string organization
        )
        {
            _start = start;
            _end = end;
            _since = since;
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
            factory.InsertRatings(
                _organization,
                _end.Author.Email,
                _deletions.Items(),
                factory.InsertedWork(
                    _organization,
                    _key,
                    _start.Sha,
                    _end.Sha,
                    _since,
                    _end.Author.Email,
                    _additions.Count(),
                    _link
                ).Id()
            );
        }
    }
}