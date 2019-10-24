namespace DevRating.Git
{
    public interface Deletion : Modification
    {
        Commit PreviousCommit();
    }
}