namespace DevRating.Domain.Git
{
    public interface ModificationsCollection
    {
        void Clear();
        void AddAddition(Addition addition);
        void AddDeletion(Deletion deletion);
        string PutTo(ModificationsStorage storage);
    }
}