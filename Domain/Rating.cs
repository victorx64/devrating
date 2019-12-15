namespace DevRating.Domain
{
    public interface Rating : IdObject
    {
        double Value();
        bool HasPreviousRating();
        Rating PreviousRating();
        Work Work();
        Author Author();
    }
}