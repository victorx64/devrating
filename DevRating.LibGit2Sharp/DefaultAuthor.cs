using System;
using DevRating.Vcs;

namespace DevRating.LibGit2Sharp
{
    internal sealed class DefaultAuthor : Author
    {
        private readonly string _email;

        public DefaultAuthor(string email)
        {
            _email = email;
        }
        
        public string Email()
        {
            return _email;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is DefaultAuthor other && Equals(other);
        }

        private bool Equals(DefaultAuthor other)
        {
            return string.Equals(_email, other._email, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_email);
        }
    }
}