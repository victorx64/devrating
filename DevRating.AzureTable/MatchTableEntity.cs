using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    internal sealed class MatchTableEntity : TableEntity
    {
        public MatchTableEntity()
        {
        }

        public MatchTableEntity(string key, string player, string contender, byte type, string commit, string repository, double rating,
            double reward, int rounds)
        {
            Commit = commit;
            Repository = repository;
            Contender = contender;
            Type = type;
            Player = player;
            Rating = rating;
            Reward = reward;
            Rounds = rounds;
            PartitionKey = player;
            RowKey = key;
        }
        
        public string Player { get; set; } = string.Empty;
        public string Contender { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Commit { get; set; } = string.Empty;
        public string Repository { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int Rounds { get; set; }
        public double Reward { get; set; }
    }
}