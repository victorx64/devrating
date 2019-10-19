using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    public sealed class MatchesTransaction : Modifications, Transaction
    {
        private readonly string _connection;
        private readonly Formula _formula;
        private readonly IList<Addition> _additions;
        private readonly IList<Deletion> _deletions;

        public MatchesTransaction(string connection, Formula formula)
        {
            _connection = connection;
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

        public void Start()
        {
            _additions.Clear();
            _deletions.Clear();
        }

        public void Commit()
        {
            var emails = Emails();

            var connection = new SqlConnection(_connection);

            connection.Open();

            try
            {
                var authors = new DbAuthorsCollection(connection);
                var matches = new DbMatchesCollection(connection);
                var ratings = new DbRatingsCollection(connection);

                PushDeletionsInto(authors, matches, ratings);

//                await PushAdditionsInto(authors);
            }
            finally
            {
                connection.Close();
            }
        }

//        private async Task PushAdditionsInto(IDictionary<string, Author> authors)
//        {
//            foreach (var addition in _additions)
//            {
//                var author = addition.Author().Email();
//
//                var rating = await authors[author].Rating();
//
//                await authors[author].AddRewardRecord(rating, addition.Count(), addition.Commit());
//            }
//        }

        private void PushDeletionsInto(AuthorsCollection authors, MatchesCollection matches, RatingsCollection ratings)
        {
            foreach (var deletion in _deletions)
            {
                var author = deletion.Author().Email();
                var victim = deletion.Victim().Email();

                if (author.Equals(victim))
                {
                    continue;
                }

                var winner = authors.Author(author);
                var loser = authors.Author(victim);

                var match = matches.NewMatch(
                    winner.Id(),
                    loser.Id(),
                    deletion.Commit().Sha(),
                    deletion.Commit().Repository(),
                    deletion.Count());

                var wRating = ratings.LastRatingOf(winner.Id());
                var lRating = ratings.LastRatingOf(loser.Id());

                var w = _formula.WinnerNewRating(wRating.Value(), lRating.Value(), deletion.Count());
                var l = _formula.LoserNewRating(wRating.Value(), lRating.Value(), deletion.Count());

                ratings.NewRating(winner.Id(), w, wRating.Id(), match.Id());
                ratings.NewRating(loser.Id(), l, lRating.Id(), match.Id());
            }
        }

        private IEnumerable<string> Emails()
        {
            var emails = _additions
                .Select(a => a.Author().Email())
                .ToList();

            emails.AddRange(_deletions.SelectMany(d => new[]
            {
                d.Author().Email(),
                d.Victim().Email()
            }));

            return emails.Distinct();
        }
    }
}