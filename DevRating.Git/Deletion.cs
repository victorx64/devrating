namespace DevRating.Git
{
    public interface Deletion : Modification
    {
        Author Victim();
    }
}