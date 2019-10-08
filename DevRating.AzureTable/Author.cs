using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;
using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    internal sealed class Author
    {
        private readonly string _name;
        private readonly CloudTable _table;
        private readonly Formula _formula;
        private MatchTableEntity? _entity;
        private readonly TableBatchOperation _operations;

        public Author(string name, CloudTable table, Formula formula)
        {
            _name = name;
            _table = table;
            _formula = formula;
            _operations = new TableBatchOperation();
        }

        public void CopyOperationsTo(TableBatchOperation destination)
        {
            foreach (var operation in _operations)
            {
                destination.Add(operation);
            }
        }

        public async Task<double> Rating()
        {
            return (await LatestEntity()).Rating;
        }

        public async Task AddRewardRecord(double rating, int count, Commit commit)
        {
            _entity = new MatchTableEntity(
                await NextKey(),
                _name,
                string.Empty,
                MatchType.AddedNewLine,
                commit.Sha(),
                commit.Repository(),
                rating,
                _formula.Reward(rating, count),
                count);

            _operations.Add(TableOperation.Insert(_entity));
        }

        public async Task AddWonMatch(string contender, Match match, Commit commit)
        {
            _entity = new MatchTableEntity(
                await NextKey(),
                _name,
                contender,
                MatchType.DeletedAnotherAuthorLine,
                commit.Sha(),
                commit.Repository(),
                _formula.WinnerNewRating(match),
                0d,
                match.Count());

            _operations.Add(TableOperation.Insert(_entity));
        }

        public async Task AddLostMatch(string contender, Match match, Commit commit)
        {
            _entity = new MatchTableEntity(
                await NextKey(),
                _name,
                contender,
                MatchType.DeletedAnotherAuthorLine,
                commit.Sha(),
                commit.Repository(),
                _formula.LoserNewRating(match),
                0d,
                match.Count());

            _operations.Add(TableOperation.Insert(_entity));
        }

        private async Task<string> NextKey()
        {
            var key = ulong.Parse(await Key(), CultureInfo.InvariantCulture);

            return (--key).ToString("D20", CultureInfo.InvariantCulture);
        }

        private async Task<string> Key()
        {
            return (await LatestEntity()).RowKey;
        }

        private async Task<MatchTableEntity> LatestEntity()
        {
            if (_entity == null)
            {
                var query = new TableQuery<MatchTableEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _name))
                    .Take(1);

                var entities = await _table.ExecuteQuerySegmentedAsync(query, null);

                _entity = entities.FirstOrDefault() ?? NewEntity();
            }

            return _entity;
        }

        private MatchTableEntity NewEntity()
        {
            var key = ulong.MaxValue.ToString("D20", CultureInfo.InvariantCulture);

            return new MatchTableEntity(key, _name, string.Empty, MatchType.Initialization, string.Empty, string.Empty,
                _formula.DefaultRating(), 0d, 0);
        }
    }
}