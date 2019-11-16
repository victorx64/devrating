namespace DevRating.Domain
{
    public interface WorkKey
    {
        string Repository();
        string StartCommit();
        string EndCommit();
    }
}