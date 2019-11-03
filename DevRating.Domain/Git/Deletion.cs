namespace DevRating.Domain.Git
{
    public interface Deletion : Modification
    {
        Commit PreviousCommit();
        Deletion NewDeletion(uint count);
    }
}