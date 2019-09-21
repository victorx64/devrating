using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Game
{
    public sealed class GamesHistoryFactory : HistoryFactory
    {
        private readonly Formula _formula;
        private readonly double _threshold;

        public GamesHistoryFactory(Formula formula, double threshold)
        {
            _formula = formula;
            _threshold = threshold;
        }
        
        public History History(string commit, string author)
        {
            return new GamesHistory(commit, author, _formula, _threshold);
        }
    }
}