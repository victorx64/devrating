namespace DevRating.Rating
{
    public interface IRating
    {
        IRating Update(string loserId, string winnerId);

        void PrintToConsole();
    }
}