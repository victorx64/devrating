using DevRating.Rating;
using DevRating.SqlClient.Collections;

namespace DevRating.SqlClient
{
    internal class RatingsPair
    {
        private readonly int _winner;
        private readonly int _loser;
        private readonly RatingsCollection _ratings;
        private readonly Formula _formula;

        public RatingsPair(int winner, int loser, RatingsCollection ratings, Formula formula)
        {
            _winner = winner;
            _loser = loser;
            _ratings = ratings;
            _formula = formula;
        }

        public double WinnerNewRating(int count)
        {
            var winner = Rating(_winner);
            var loser = Rating(_loser);

            return _formula.WinnerNewRating(winner, loser, count);
        }

        public double LoserNewRating(int count)
        {
            var winner = Rating(_winner);
            var loser = Rating(_loser);

            return _formula.LoserNewRating(winner, loser, count);
        }

        private double Rating(int author)
        {
            return _ratings.HasRating(author)
                ? _ratings.LastRatingOf(author).Value()
                : _formula.DefaultRating();
        }
    }
}