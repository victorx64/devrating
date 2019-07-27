namespace DevRating.Rating
{
    public interface IRating
    {
        IRating UpdatedRating(string loserId, string winnerId);

        void PrintToConsole();
    }
}