using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Domain.Diff
    {
        private readonly DiffFingerprint _fingerprint;

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
            : this(new DefaultDiffFingerprint(key, start.Sha, end.Sha, additions.Count(), end.Author.Email, deletions))
        {
        }

        public LibGit2Diff(DiffFingerprint fingerprint)
        {
            _fingerprint = fingerprint;
        }

        public DiffFingerprint Fingerprint()
        {
            return _fingerprint;
        }
    }
}