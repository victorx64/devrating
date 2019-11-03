namespace DevRating.Domain
{
    public interface Reward
    {
        Rating Rating();
        Author Author();
        double Value();
    }
}