namespace DevRating.Domain.Git
{
    public interface Commit
    {
        string Sha();
        string Repository();
        string Author();
    }
}