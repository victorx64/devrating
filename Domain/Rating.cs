namespace DevRating.Domain
{
    public interface Rating
    {
        Author Author();
        double Value();
        Rating LastRating();
        bool HasLastRating();
    }
}