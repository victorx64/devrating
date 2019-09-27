using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    internal sealed class MatchTableEntity : TableEntity
    {
        public MatchTableEntity()
        {
        }

        public MatchTableEntity(string key, string player, string contender, byte type, string commit, double points,
            double reward, int rounds)
        {
            Commit = commit;
            Contender = contender;
            Type = type;
            Player = player;
            Points = points;
            Reward = reward;
            Rounds = rounds;
            PartitionKey = player;
            RowKey = key;
        }
        
        public string Player { get; set; } = string.Empty;
        public string Contender { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Commit { get; set; } = string.Empty;
        public double Points { get; set; }
        public int Rounds { get; set; }
        public double Reward { get; set; }
    }
}