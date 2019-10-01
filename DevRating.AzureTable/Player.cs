using System;
using System.Globalization;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace DevRating.AzureTable
{
    internal sealed class Player
    {
        private readonly string _name;
        private readonly CloudBlockBlob _blob;
        private readonly CloudTable _table;

        private string _lease;
        private ulong _key;

        public Player(string name, CloudBlockBlob blob, CloudTable table)
        {
            _name = name;
            _blob = blob;
            _table = table;
            _lease = string.Empty;
        }

        public async Task<double> Points(double @default)
        {
            ThrowIfNotLocked();

            var operation = TableOperation.Retrieve<MatchTableEntity>(_name, LastMatchKey());

            var result = await _table.ExecuteAsync(operation);

            // TODO if result not found return @default;

            var match = (MatchTableEntity) result.Result;

            return match.Rating;
        }

        public Task AddWonMatch(string contender, Match match, Commit commit, Formula formula, byte type)
        {
            var entity = new MatchTableEntity(
                LastMatchKey(),
                _name,
                contender,
                type,
                commit.Sha(),
                commit.Repository(),
                formula.WinnerNewRating(match),
                formula.WinnerReward(match),
                match.Times());

            return AddMatchEntity(entity);
        }

        public Task AddLostMatch(string contender, Match match, Commit commit, Formula formula, byte type)
        {
            var entity = new MatchTableEntity(
                LastMatchKey(),
                _name,
                contender,
                type,
                commit.Sha(),
                commit.Repository(),
                formula.LoserNewRating(match),
                formula.LoserReward(match),
                match.Times());

            return AddMatchEntity(entity);
        }

        private Task AddMatchEntity(ITableEntity entity)
        {
            ThrowIfNotLocked();

            MoveLastMatchKey();

            var operation = TableOperation.InsertOrReplace(entity);

            return _table.ExecuteAsync(operation);
        }

        public async Task Lock()
        {
            _lease = await _blob.AcquireLeaseAsync(null, Guid.NewGuid().ToString());

            var text = await _blob.DownloadTextAsync();

            _key = string.IsNullOrEmpty(text)
                ? ulong.MaxValue
                : ulong.Parse(text, CultureInfo.InvariantCulture);
        }

        public async Task Unlock()
        {
            ThrowIfNotLocked();

            await _blob.ReleaseLeaseAsync(AccessCondition.GenerateLeaseCondition(_lease));

            _lease = string.Empty;
        }

        public Task Sync()
        {
            ThrowIfNotLocked();

            return _blob.UploadTextAsync(LastMatchKey());
        }

        public void ThrowIfNotLocked()
        {
            if (_lease.Equals(string.Empty))
            {
                throw new Exception("Player is not locked");
            }
        }

        private string LastMatchKey()
        {
            ThrowIfNotLocked();

            return _key.ToString("D20", CultureInfo.InvariantCulture);
        }

        private void MoveLastMatchKey()
        {
            ThrowIfNotLocked();

            --_key;
        }
    }
}