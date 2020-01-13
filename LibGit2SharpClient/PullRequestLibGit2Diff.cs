using System.Linq;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class PullRequestLibGit2Diff : Domain.Diff
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly string _key;
        private readonly string _link;
        private readonly Additions _additions;
        private readonly Deletions _deletions;

        public PullRequestLibGit2Diff(string start, string end, IRepository repository, string link)
            : this(repository.Lookup<Commit>(start),
                repository.Lookup<Commit>(end),
                repository,
                repository.Network.Remotes.First().Url,
                link)
        {
        }

        public PullRequestLibGit2Diff(string start, string end, IRepository repository, string key, string link)
            : this(repository.Lookup<Commit>(start), repository.Lookup<Commit>(end), repository, key, link)
        {
        }

        public PullRequestLibGit2Diff(Commit start, Commit end, IRepository repository, string key, string link)
            : this(start, end, new CachedHunks(new LibGit2Hunks(start, end, repository)), key, link)
        {
        }

        public PullRequestLibGit2Diff(Commit start, Commit end, Hunks hunks, string key, string link)
            : this(start, end, new TotalAdditions(hunks), new TotalDeletions(hunks), key, link)
        {
        }

        public PullRequestLibGit2Diff(Commit start, Commit end, Additions additions, Deletions deletions, string key,
            string link)
        {
            _start = start;
            _end = end;
            _additions = additions;
            _deletions = deletions;
            _key = key;
            _link = link;
        }

        public Work WorkFrom(Works works)
        {
            return works.GetOperation().Work(_key, _start.Sha, _end.Sha);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start.Sha, _end.Sha);
        }

        public void AddTo(Diffs diffs)
        {
            diffs.Insert(_key, _link, _start.Sha, _end.Sha, _end.Author.Email, _additions.Count(), _deletions.Items());
        }
    }
}