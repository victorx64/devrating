namespace DevRating.Domain
{
    public interface Author : Entity
    {
        string Email();
        Rating Rating();
        bool HasRating();
    }
}