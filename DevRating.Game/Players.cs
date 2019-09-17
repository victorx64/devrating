namespace DevRating.Game
{
    public interface Players
    {
        bool Exists(string name);
        Player Player(string name);
        void Add(string name, Player player);
        void Update(string name, Player player);
    }
}