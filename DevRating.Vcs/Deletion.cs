namespace DevRating.Vcs
{
    public interface Deletion : Modification
    {
        Commit PreviousCommit();
        Deletion NewDeletion(uint count);
    }
}