namespace DevRating.Rating
{
    public interface Match
    {
        double Winner();
        double Loser();
        int Count();
    }
}