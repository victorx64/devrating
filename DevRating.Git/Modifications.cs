namespace DevRating.Git
{
    public interface Modifications
    {
        void AddDeletion(string victim);
        void AddAdditions(int count);
    }
}