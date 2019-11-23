using System.Linq;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2WorkKey : WorkKey
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly IRepository _repository;

        public LibGit2WorkKey(Commit start, Commit end, IRepository repository)
        {
            _start = start;
            _end = end;
            _repository = repository;
        }
        
        public string Repository()
        {
            return _repository.Network.Remotes.First().Url;
        }

        public string StartCommit()
        {
            return _start.Sha;
        }

        public string EndCommit()
        {
            return _end.Sha;
        }
    }
}