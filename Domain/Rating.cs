namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        bool HasPreviousRating();
        Rating PreviousRating();
        bool HasDeletions();
        uint Deletions();
        Work Work();
        Author Author();
    }
}