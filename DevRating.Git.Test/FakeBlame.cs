namespace DevRating.Git.Test
{
    public class FakeBlame : Blame
    {
        private readonly string _author;

        public FakeBlame(string author)
        {
            _author = author;
        }

        public string AuthorOf(int line)
        {
            return _author;
        }
    }
}