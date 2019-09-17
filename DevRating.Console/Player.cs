namespace DevRating.Console
{
    public interface Player
    {
        Player NewPlayer(Game game);
        double Points();
    }
}