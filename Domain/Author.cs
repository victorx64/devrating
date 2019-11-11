namespace DevRating.Domain
{
    public interface Author
    {
        string Email();
        Rating LastRating();
        bool HasRating();
        Reward LastReward();
        bool HasReward();
    }
}