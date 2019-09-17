namespace DevRating.Game
{
    public interface Players
    {
        Player PlayerOrDefault(string name);
        void AddOrUpdatePlayer(string name, Player player);
    }
}