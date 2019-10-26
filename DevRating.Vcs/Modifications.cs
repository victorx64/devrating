namespace DevRating.Vcs
{
    public interface Modifications
    {
        void AddAddition(Addition addition);
        void AddDeletion(Deletion deletion);
    }
}