using DevRating.Domain.RatingSystem;
using DevRating.SqlClient.Collections;

namespace DevRating.SqlClient
{
    internal sealed class RatingsPair
    {
        private readonly int _winner;
        private readonly int _loser;
        private readonly RatingsCollection _ratings;
        private readonly Formula _formula;
        private readonly uint _count;

        public RatingsPair(int winner, int loser, RatingsCollection ratings, Formula formula, uint count)
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

        private double Rating(int author)
        {
            return _ratings.HasRating(author)
                ? _ratings.LastRatingOf(author).Value()
                : _formula.DefaultRating();
        }
    }
}