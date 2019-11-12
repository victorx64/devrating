using DevRating.Domain.RatingSystem;
using DevRating.SqlClient.Collections;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient
{
    internal sealed class RatingsPair
    {
        private readonly IdentifiableAuthor _winner;
        private readonly IdentifiableAuthor _loser;
        private readonly RatingsCollection _ratings;
        private readonly Formula _formula;
        private readonly uint _count;

        public RatingsPair(IdentifiableAuthor winner, IdentifiableAuthor loser, RatingsCollection ratings,
            Formula formula, uint count)
        {
            _winner = winner;
            _loser = loser;
            _ratings = ratings;
            _formula = formula;
            _count = count;
        }

        public double WinnerRating()
        {
            return Rating(_winner);
        }

        public double LoserRating()
        {
            return Rating(_loser);
        }

        public double WinnerNewRating()
        {
            return _formula.WinnerNewRating(Rating(_winner), Rating(_loser), _count);
        }

        public double LoserNewRating()
        {
            return _formula.LoserNewRating(Rating(_winner), Rating(_loser), _count);
        }

        private double Rating(IdentifiableAuthor author)
        {
            return _ratings.HasRatingOf(author)
                ? _ratings.LastRatingOf(author).Value()
                : _formula.DefaultRating();
        }
    }
}