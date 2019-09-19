using LibGit2Sharp;

namespace DevRating.Git
{
    internal sealed class Author
    {
        private readonly IRepository _repo;
        private readonly Signature _signature;

        public Author(IRepository repo, Signature signature)
        {
            _repo = repo;
            _signature = signature;
        }

        public string Email()
        {
            return string.IsNullOrEmpty(_signature.Name) ||
                   string.IsNullOrEmpty(_signature.Email)
                ? _signature.Email
                : _repo.Mailmap.ResolveSignature(_signature).Email;
        }
    }
}