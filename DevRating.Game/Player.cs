namespace DevRating.Game
{
    public interface Player
    {
        Player NewPlayer(Game game);
        double Points();
    }
}