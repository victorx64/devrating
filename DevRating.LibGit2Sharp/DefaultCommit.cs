using System;
using DevRating.Vcs;

namespace DevRating.LibGit2Sharp
{
    internal sealed class DefaultCommit : Commit
    {
        private readonly string _sha;
        private readonly string _author;
        private readonly string _repository;

        public DefaultCommit(string sha, string author, string repository)
        {
            _sha = sha;
            _author = author;
            _repository = repository;
        }

        public string Sha()
        {
            return _sha;
        }

        public string Repository()
        {
            return _repository;
        }

        public string Author()
        {
            return _author;
        }

        public override string ToString()
        {
            return Sha();
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is DefaultCommit other && Equals(other);
        }

        private bool Equals(DefaultCommit other)
        {
            return string.Equals(_sha, other._sha, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(_author, other._author, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(_repository, other._repository, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(_sha);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(_author);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(_repository);
                return hashCode;
            }
        }
    }
}