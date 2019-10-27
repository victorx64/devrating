namespace DevRating.Vcs
{
    public interface ModificationsCollection
    {
        void Clear();
        void AddAddition(Addition addition);
        void AddDeletion(Deletion deletion);
        void PutTo(ModificationsStorage storage);
    }
}