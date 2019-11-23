namespace DevRating.Domain
{
    public interface Rating
    {
        double Value();
        bool HasPreviousRating();
        Rating PreviousRating();
        Work Work();
        Author Author();
    }
}