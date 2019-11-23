namespace DevRating.Domain
{
    public interface Author
    {
        string Email();
        Rating Rating();
        bool HasRating();
    }
}