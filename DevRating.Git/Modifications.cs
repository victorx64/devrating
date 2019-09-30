namespace DevRating.Git
{
    public interface Modifications
    {
        void AddDeletion(string author, string victim);
        void AddAdditions(string author, int count);
    }
}