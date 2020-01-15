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
        private readonly InsertWorkParams _params;
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
            : this(start, end, new LinkedInsertWorkParams(link, key, start.Sha, end.Sha, additions.Count()), deletions,
                key)
        {
        }

        public PullRequestLibGit2Diff(Commit start, Commit end, InsertWorkParams @params, Deletions deletions,
            string key)
        {
            _start = start;
            _end = end;
            _params = @params;
            _deletions = deletions;
            _key = key;
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
            diffs.Insert(_params, _end.Author.Email, _deletions.Items());
        }
    }
}