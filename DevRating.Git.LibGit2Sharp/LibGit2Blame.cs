using LibGit2Sharp;

namespace DevRating.Git.LibGit2Sharp
{
    public class LibGit2Blame : Blame
    {
        private readonly BlameHunkCollection _collection;
        private readonly IRepository _repository;

        public LibGit2Blame(string path, string sha, IRepository repository)
            : this(repository.Blame(path, new BlameOptions {StartingAt = sha}), repository)
        {
        }

        public LibGit2Blame(BlameHunkCollection collection, IRepository repository)
        {
            _collection = collection;
            _repository = repository;
        }

        public string AuthorOf(int line)
        {
            return _repository.Mailmap.ResolveSignature(_collection.HunkForLine(line).FinalSignature).Email;
        }
    }
}