using System.Collections.Generic;
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

                var winner = authors.Exist(author) ? authors.Author(author) : authors.NewAuthor(author);
                var loser = authors.Exist(victim) ? authors.Author(victim) : authors.NewAuthor(victim);

                NewMethod(winner.Id(), loser.Id(), matches, ratings, deletion);
            }
        }

        private void NewMethod(int winner, int loser, MatchesCollection matches, RatingsCollection ratings,
            Modification deletion)
        {
            var match = matches
                .NewMatch(
                    winner,
                    loser,
                    deletion.Commit().Sha(),
                    deletion.Commit().Repository(),
                    deletion.Count())
                .Id();

            var winnerRating = ratings.HasRating(winner)
                ? ratings.LastRatingOf(winner).Value()
                : _formula.DefaultRating();

            var loserRating = ratings.HasRating(loser)
                ? ratings.LastRatingOf(loser).Value()
                : _formula.DefaultRating();

            var winnerNewRating = _formula.WinnerNewRating(winnerRating, loserRating, deletion.Count());

            if (ratings.HasRating(winner))
            {
                var rating = ratings.LastRatingOf(winner).Id();

                ratings.NewRating(winner, winnerNewRating, rating, match);
            }
            else
            {
                ratings.NewRating(winner, winnerNewRating, match);
            }

            var loserNewRating = _formula.LoserNewRating(winnerRating, loserRating, deletion.Count());

            if (ratings.HasRating(loser))
            {
                var rating = ratings.LastRatingOf(loser).Id();

                ratings.NewRating(loser, loserNewRating, rating, match);
            }
            else
            {
                ratings.NewRating(loser, loserNewRating, match);
            }
        }
    }
}