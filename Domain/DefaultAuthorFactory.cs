namespace DevRating.Domain
{
    public sealed class DefaultAuthorFactory : AuthorFactory
    {
        private readonly string _email;

        public DefaultAuthorFactory(string email)
        {
            _email = email;
        }

        public Author Author(Entities entities)
        {
            return Author(entities, _email);
        }

        public Author Author(Entities entities, string email)
        {
            return entities.Authors().ContainsOperation().Contains(email)
                ? entities.Authors().GetOperation().Author(email)
                : entities.Authors().InsertOperation().Insert(email);
        }
    }
}