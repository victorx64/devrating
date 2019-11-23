using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Modification : Modification
    {
        private readonly Signature _signature;
        private readonly uint _count;

        public LibGit2Modification(Signature signature, uint count)
        {
            _signature = signature;
            _count = count;
        }
        
        public string Email()
        {
            return _signature.Email;
        }

        public uint Count()
        {
            return _count;
        }
    }
}