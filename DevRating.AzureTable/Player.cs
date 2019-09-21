using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace DevRating.AzureTable
{
    internal sealed class Player
    {
        private readonly CloudBlockBlob _blob;
        private string _lease;
        private ulong _key;

        public Player(string name, string container, string connection) : this(Blob(name, container, connection))
        {
        }

        public Player(CloudBlockBlob blob)
        {
            _blob = blob;
        }

        public string LastMatchKey()
        {
            ThrowIfNotLocked();

            return _key.ToString("D20", CultureInfo.InvariantCulture);
        }

        public void MoveLastMatchKey()
        {
            ThrowIfNotLocked();

            --_key;
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
            if (!string.IsNullOrEmpty(_lease))
            {
                throw new Exception("Player is not locked");
            }
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