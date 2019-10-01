using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage.Blob;

namespace DevRating.AzureTable
{
    public sealed class AzureModifications : Modifications
    {
        private readonly string _container;
        private readonly string _connection;
        private readonly CloudTable _table;
        private readonly Formula _formula;
        private readonly IList<Addition> _additions;
        private readonly IList<Deletion> _deletions;

        public AzureModifications(string connection, string container, string table, Formula formula)
        {
            _connection = connection;
            _table = Table(connection, table);
            _formula = formula;
            _container = container;
            _additions = new List<Addition>();
            _deletions = new List<Deletion>();
        }

        public void AddAddition(Addition addition)
        {
            _additions.Add(addition);
        }

        public void AddDeletion(Deletion deletion)
        {
            _deletions.Add(deletion);
        }

        public async Task Sync()
        {
            var authors = SortedAuthors();

            try
            {
                foreach (var author in authors)
                {
                    await author.Value.Lock();
                }

                await PushDeletionsInto(authors);

                await PushAdditionsInto(authors);

                foreach (var author in authors)
                {
                    await author.Value.Sync();
                }
            }
            finally
            {
                foreach (var author in authors.Reverse())
                {
                    await author.Value.Unlock();
                }
            }
        }

        private async Task PushAdditionsInto(IDictionary<string, Player> authors)
        {
            foreach (var addition in _additions)
            {
                var author = addition.Author().Email();

                var winner = await authors[author].Points(_formula.NewPlayerRating());

                var match = new DefaultMatch(winner, _formula.Threshold(), addition.Count());

                await authors[author].AddWonMatch(string.Empty, match, addition.Commit(), _formula, 1);
            }
        }

        private async Task PushDeletionsInto(IDictionary<string, Player> authors)
        {
            foreach (var deletion in _deletions)
            {
                var author = deletion.Author().Email();
                var victim = deletion.Victim().Email(); // TODO Check author.Equals(victim)

                var winner = await authors[author].Points(_formula.NewPlayerRating());
                var loser = await authors[victim].Points(_formula.NewPlayerRating());
                var count = deletion.Count();

                var match = new DefaultMatch(winner, loser, count);

                await authors[author].AddWonMatch(victim, match, deletion.Commit(), _formula, 1);
                await authors[victim].AddLostMatch(author, match, deletion.Commit(), _formula, 1);
            }
        }

        private IDictionary<string, Player> SortedAuthors()
        {
            var players = _additions
                .Select(a => a.Author().Email())
                .ToList();

            players.AddRange(_deletions.SelectMany(d => new[]
            {
                d.Author().Email(), d.Victim().Email()
            }));

            players.Sort();

            return players
                .Distinct()
                .ToDictionary(p => p, p => new Player(p, Blob(_connection, _container, p), _table));
        }

        private CloudBlockBlob Blob(string connection, string container, string blob)
        {
            var storageAccount = Microsoft.Azure.Storage.CloudStorageAccount.Parse(connection);

            var client = storageAccount.CreateCloudBlobClient();

            var reference = client.GetContainerReference(container);

            reference.CreateIfNotExists();

            reference.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob // For what?
                });

            return reference.GetBlockBlobReference(blob);
        }

        private CloudTable Table(string connection, string table)
        {
            var storageAccount = CloudStorageAccount.Parse(connection);

            var client = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            var reference = client.GetTableReference(table);

            reference.CreateIfNotExists();

            return reference;
        }
    }
}