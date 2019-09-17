namespace DevRating.Rating
{
    public interface Player
    {
        double Points();
        Player Winner(Player contender);
        Player Loser(Player contender);
    }
}