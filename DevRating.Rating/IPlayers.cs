namespace DevRating.Rating
{
    public interface IPlayers
    {
        bool Exist(string id);

        void Add(string id, IPlayer player);

        IPlayers UpdatedPlayers(string loser, string winner);
    }
}