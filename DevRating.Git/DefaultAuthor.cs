namespace DevRating.Git
{
    public sealed class DefaultAuthor : Author
    {
        private readonly string _email;

        public DefaultAuthor(string email)
        {
            _email = email;
        }
    }
}