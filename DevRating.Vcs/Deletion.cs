namespace DevRating.Vcs
{
    public interface Deletion : Modification
    {
        Commit PreviousCommit();
        Deletion UpdatedDeletion(int count);
    }
}