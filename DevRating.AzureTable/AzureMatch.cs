using DevRating.Game;

namespace DevRating.AzureTable
{
    public class AzureMatch : Match
    {
        private readonly MatchTableEntity _entity;

        public AzureMatch(string player, string contender, byte type, string commit, double points, double reward, int rounds)
        {
            _entity = new MatchTableEntity
            {
                Commit = commit,
                Contender = contender,
                Type = type,
                Player = player,
                Points = points,
                Reward = reward,
                Rounds = rounds,
                PartitionKey = player,
                RowKey = commit
            };
        }

        public string Player()
        {
            return _entity.Player;
        }

        public double Points()
        {
            return _entity.Points;
        }

        public string Commit()
        {
            return _entity.Commit;
        }

        public string Contender()
        {
            return _entity.Contender;
        }

        public int Rounds()
        {
            return _entity.Rounds;
        }

        public double Reward()
        {
            return _entity.Reward;
        }
    }
}