namespace DevRating.Rating
{
    public interface Player
    {
        double Points();
        Player Winner(Player opponent);
        Player Loser(Player opponent);
    }
}