namespace DevRating.Domain
{
    public interface Reward
    {
        bool HasRating();
        Rating Rating();
        Author Author();
        double Value();
    }
}