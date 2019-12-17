namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(Diffs diffs);
        string RepositoryName();
        string StartCommit();
        string EndCommit();
    }
}