namespace DevRating.Git
{
    public interface AuthorChanges
    {
        void AddChange(Author previous, Author next, string commit);
    }
}