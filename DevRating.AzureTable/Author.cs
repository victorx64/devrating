using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace DevRating.AzureTable
{
    internal sealed class Author
    {
        private readonly string _name;
        private readonly CloudBlockBlob _blob;
        private readonly CloudTable _table;
        private readonly Formula _formula;

        private string _lease;
        private ulong _key;

        public Author(string name, CloudBlockBlob blob, CloudTable table, Formula formula)
        {
            _name = name;
            _blob = blob;
            _table = table;
            _formula = formula;
            _lease = string.Empty;
        }

        public async Task<double> Rating()
        {
            ThrowIfNotLocked();

            if (_key == ulong.MaxValue)
            {
                return _formula.NewPlayerRating();
            }

            var operation = TableOperation.Retrieve<MatchTableEntity>(_name, CurrentMatchKey());

            var result = await _table.ExecuteAsync(operation);

            var match = (MatchTableEntity) result.Result;

            return match.Rating;
        }

        public Task AddWonMatch(string contender, Match match, Commit commit, byte type)
        {
            var entity = new MatchTableEntity(
                NextMatchKey(),
                _name,
                contender,
                type,
                commit.Sha(),
                commit.Repository(),
                _formula.WinnerNewRating(match),
                _formula.WinnerReward(match),
                match.Count());

            return AddMatchEntity(entity);
        }

        public Task AddLostMatch(string contender, Match match, Commit commit, byte type)
        {
            var entity = new MatchTableEntity(
                NextMatchKey(),
                _name,
                contender,
                type,
                commit.Sha(),
                commit.Repository(),
                _formula.LoserNewRating(match),
                _formula.LoserReward(match),
                match.Count());

            return AddMatchEntity(entity);
        }

        private async Task AddMatchEntity(ITableEntity entity)
        {
            ThrowIfNotLocked();

            var operation = TableOperation.InsertOrReplace(entity);

            await _table.ExecuteAsync(operation);

            MoveCurrentMatchKey();
        }

        public async Task Lock()
        {
            if (!_lease.Equals(string.Empty))
            {
                throw new Exception("Author is already locked");
            }
            
            if (!_blob.Exists())
            {
                await _blob.UploadTextAsync(ulong.MaxValue.ToString("D20", CultureInfo.InvariantCulture), Encoding.UTF8,
                    null, null, null);
            }

            _lease = await _blob.AcquireLeaseAsync(null, Guid.NewGuid().ToString());

            var text = await _blob.DownloadTextAsync(Encoding.UTF8,
                null, null, null);

            _key = ulong.Parse(text, CultureInfo.InvariantCulture);
        }

        public async Task Unlock()
        {
            ThrowIfNotLocked();

            await _blob.ReleaseLeaseAsync(AccessCondition.GenerateLeaseCondition(_lease));

            _lease = string.Empty;
        }

        public Task Upload()
        {
            ThrowIfNotLocked();

            return _blob.UploadTextAsync(CurrentMatchKey(), Encoding.UTF8,
                AccessCondition.GenerateLeaseCondition(_lease), null, null);
        }

        public void ThrowIfNotLocked()
        {
            if (_lease.Equals(string.Empty))
            {
                throw new Exception("Author is not locked");
            }
        }

        private string CurrentMatchKey()
        {
            ThrowIfNotLocked();

            return _key.ToString("D20", CultureInfo.InvariantCulture);
        }

        private void MoveCurrentMatchKey()
        {
            ThrowIfNotLocked();

            --_key;
        }

        private string NextMatchKey()
        {
            ThrowIfNotLocked();
            
            return (_key - 1).ToString("D20", CultureInfo.InvariantCulture);
        }
    }
}