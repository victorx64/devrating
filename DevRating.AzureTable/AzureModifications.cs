using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;
using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    public sealed class AzureModifications : Modifications
    {
        private readonly CloudTable _table;
        private readonly Formula _formula;
        private readonly IList<Addition> _additions;
        private readonly IList<Deletion> _deletions;

        public AzureModifications(string connection, string table, Formula formula)
        {
            _table = Table(connection, table);
            _formula = formula;
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

        public void Clear()
        {
            _additions.Clear();
            _deletions.Clear();
        }

        public async Task Upload()
        {
            var authors = Authors();

            await PushDeletionsInto(authors);

            await PushAdditionsInto(authors);

            var operations = new TableBatchOperation();

            foreach (var author in authors)
            {
                author.Value.CopyOperationsTo(operations);
            }

            await _table.ExecuteBatchAsync(operations);
        }

        private async Task PushAdditionsInto(IDictionary<string, Author> authors)
        {
            foreach (var addition in _additions)
            {
                var author = addition.Author().Email();

                var winner = await authors[author].Rating();

                var match = new DefaultMatch(winner, _formula.HighRating(), addition.Count());

                await authors[author].AddWonMatch(string.Empty, match, addition.Commit(), MatchType.AddedNewLine);
            }
        }

        private async Task PushDeletionsInto(IDictionary<string, Author> authors)
        {
            foreach (var deletion in _deletions)
            {
                var author = deletion.Author().Email();
                var victim = deletion.Victim().Email();

                if (author.Equals(victim))
                {
                    continue;
                }

                var match = new DefaultMatch(
                    await authors[author].Rating(),
                    await authors[victim].Rating(),
                    deletion.Count());

                await authors[author].AddWonMatch(victim, match, deletion.Commit(), MatchType.DeletedAnotherAuthorLine);
                await authors[victim].AddLostMatch(author, match, deletion.Commit(), MatchType.DeletedAnotherAuthorLine);
            }
        }

        private IDictionary<string, Author> Authors()
        {
            var authors = _additions
                .Select(a => a.Author().Email())
                .ToList();

            authors.AddRange(_deletions.SelectMany(d => new[]
            {
                d.Author().Email(),
                d.Victim().Email()
            }));

            return authors
                .Distinct()
                .ToDictionary(p => p, p => new Author(p, _table, _formula));
        }

        public string Report()
        {
            var builder = new StringBuilder();

            foreach (var addition in _additions)
            {
                builder.Append($"Addition {addition.Author()} {addition.Count()} {addition.Commit()}");
            }

            foreach (var deletion in _deletions)
            {
                builder.Append(
                    $"Deletion {deletion.Author()} {deletion.Victim()} {deletion.Count()} {deletion.Commit()}");
            }

            return builder.ToString();
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