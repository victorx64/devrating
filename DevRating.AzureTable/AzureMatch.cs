using DevRating.Game;

namespace DevRating.AzureTable
{
    internal class AzureMatch : Match
    {
        private readonly MatchTableEntity _entity;

        public AzureMatch(MatchTableEntity entity)
        {
            _entity = entity;
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