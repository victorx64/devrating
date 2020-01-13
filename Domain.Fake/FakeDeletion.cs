namespace DevRating.Domain.Fake
{
    public sealed class FakeDeletion : Deletion
    {
        private readonly string _email;
        private readonly uint _count;

        public FakeDeletion(string email, uint count)
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