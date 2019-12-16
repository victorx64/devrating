namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        bool HasPreviousRating();
        Rating PreviousRating();
        Work Work();
        Author Author();
    }
}