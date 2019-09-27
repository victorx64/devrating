using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DevRating.Game;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using CloudStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount;

namespace DevRating.AzureTable
{
    internal sealed class Player
    {
        private readonly string _name;
        private readonly CloudBlockBlob _blob;
        private readonly CloudTable _table;
        private readonly double _points;

        private string _lease;
        private ulong _key;

        public Player(string name, string container, string connection, CloudTable table, double points)
            : this(name, Blob(name, container, connection), table, points)
        {
        }

        public Player(string name, CloudBlockBlob blob, CloudTable table, double points)
        {
            _name = name;
            _blob = blob;
            _table = table;
            _points = points;
            _lease = string.Empty;
        }

        public async Task<double> Points()
        {
            ThrowIfNotLocked();

            var operation = TableOperation.Retrieve<MatchTableEntity>(_name, LastMatchKey());

            var result = await _table.ExecuteAsync(operation);
            
            // TODO if result not found return _points;

            var match = (MatchTableEntity) result.Result;

            return match.Points;
        }

        public Task AddMatch(string contender, string commit, double points, double reward, int rounds, byte type)
        {
            ThrowIfNotLocked();

            MoveLastMatchKey();

            var match = new MatchTableEntity(LastMatchKey(), _name, contender, type, commit, points, reward, rounds);

            var operation = TableOperation.InsertOrReplace(match);

            return _table.ExecuteAsync(operation);
        }

        public Task<IEnumerable<Match>> Matches()
        {
            var query = new TableQuery<MatchTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _name));

            var matches = new List<Match>();

            foreach (var entity in _table.ExecuteQuery(query)) // TODO Consider using ExecuteQuerySegmentedAsync
            {
                matches.Add(new AzureMatch(entity));
            }

            return Task.FromResult((IEnumerable<Match>) matches);
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

        private static CloudBlockBlob Blob(string name, string container, string connection)
        {
            var storageAccount = CloudStorageAccount.Parse(connection);

            var client = storageAccount.CreateCloudBlobClient();

            var reference = client.GetContainerReference(container);

            reference.CreateIfNotExists();

            reference.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob // For what?
                });

            return reference.GetBlockBlobReference(name);
        }
    }
}