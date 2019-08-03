namespace DevRating
{
    public interface IPlayer
    {
        double Points();
        int Games();
        IPlayer Winner(IPlayer opponent);
        IPlayer Loser(IPlayer opponent);
    }
}