namespace DevRating.VersionControl.Fake
{
    public sealed class FakeAdditions : Additions
    {
        private readonly uint _count;

        public FakeAdditions(uint count)
        {
            _count = count;
        }

        public uint Count()
        {
            return _count;
        }
    }
}