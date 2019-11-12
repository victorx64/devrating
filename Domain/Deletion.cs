namespace DevRating.Domain
{
    public interface Deletion : Modification
    {
        Commit PreviousCommit();
        Deletion NewDeletion(uint count);
    }
}