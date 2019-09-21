using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    internal sealed class MatchTableEntity : TableEntity
    {
        public string Player { get; set; }
        public string Contender { get; set; }
        public byte Type { get; set; }
        public string Commit { get; set; }
        public double Points { get; set; }
        public int Rounds { get; set; }
        public double Reward { get; set; }
    }
}