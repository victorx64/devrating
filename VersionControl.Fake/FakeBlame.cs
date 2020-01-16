namespace DevRating.VersionControl.Fake
{
    public sealed class FakeBlame : Blame
    {
        private readonly string _email;
        private readonly uint _start;
        private readonly uint _count;

        public FakeBlame(string email, uint start, uint count)
        {
            _email = email;
            _start = start;
            _count = count;
        }

        public string AuthorEmail()
        {
            return _email;
        }

        public uint StartLineNumber()
        {
            return _start;
        }

        public uint LineCount()
        {
            return _count;
        }

        public bool ContainsLine(uint line)
        {
            return StartLineNumber() <= line && line < StartLineNumber() + LineCount();
        }
    }
}