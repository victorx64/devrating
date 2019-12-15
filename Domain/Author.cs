namespace DevRating.Domain
{
    public interface Author : IdObject
    {
        string Email();
        Rating Rating();
        bool HasRating();
    }
}