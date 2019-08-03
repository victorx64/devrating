namespace DevRating
{
    public interface IRankedPlayers : IPlayers
    {
        IRankedPlayers RankedPlayers(string loserId, string winnerId);
    }
}