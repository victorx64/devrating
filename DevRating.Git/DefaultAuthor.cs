namespace DevRating.Git
{
    public class DefaultAuthor : Author
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

        public override string ToString()
        {
            return Email();
        }
    }
}