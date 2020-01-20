using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class VersionControlDeletion : Deletion
    {
        private readonly string _email;
        private readonly uint _count;

        public VersionControlDeletion(string email, uint count)
        {
            _email = email;
            _count = count;
        }

        public string Email()
        {
            return _email;
        }

        public uint Count()
        {
            return _count;
        }
    }
}