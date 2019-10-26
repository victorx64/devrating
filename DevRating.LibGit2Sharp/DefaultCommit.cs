using System;
using DevRating.Vcs;

namespace DevRating.LibGit2Sharp
{
    internal sealed class DefaultCommit : Commit
    {
        private readonly string _sha;
        private readonly Author _author;
        private readonly string _repository;

        public DefaultCommit(string sha, Author author, string repository)
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

        public Author Author()
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
                   _author.Equals(other._author) &&
                   string.Equals(_repository, other._repository, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(_sha);
                hashCode = (hashCode * 397) ^ _author.GetHashCode();
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(_repository);
                return hashCode;
            }
        }
    }
}