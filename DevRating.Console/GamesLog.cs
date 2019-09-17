using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class GamesLog : Log
    {
        private readonly Players _players;
        private readonly Player _default;
        private readonly Player _entropy;
        private readonly PointsFormula _formula;

        public GamesLog(Players players, Player @default, Player entropy, PointsFormula formula)
        {
            _players = players;
            _default = @default;
            _entropy = entropy;
            _formula = formula;
        }

        public void LogDeletion(int count, string victim, string initiator, string commit)
        {
            lock (_players)
            {
                var loser = PlayerOrDefault(victim);
                var winner = PlayerOrDefault(initiator);
                
                // multiplying to 'count' is gross simplification
                var extra = _formula.WinnerExtraPoints(winner.Points(), loser.Points()) * count;
                var reward = _formula.WinProbability(winner.Points(), loser.Points()) * count;

                AddOrUpdatePlayer(victim,
                    loser.NewPlayer(new Game(initiator, commit, loser.Points() - extra, 0d, count)));

                AddOrUpdatePlayer(initiator,
                    winner.NewPlayer(new Game(victim, commit, winner.Points() + extra, reward, count)));
            }
        }

        public void LogAddition(int count, string initiator, string commit)
        {
            lock (_players)
            {
                var winner = PlayerOrDefault(initiator);
                
                var reward = _formula.WinProbability(winner.Points(), _entropy.Points()) * count;

                AddOrUpdatePlayer(initiator,
                    winner.NewPlayer(new Game("entropy", commit, winner.Points(), reward, count)));
            }
        }

        private Player PlayerOrDefault(string name)
        {
            if (_players.Exists(name))
            {
                return _players.Player(name);
            }

            return _default;
        }

        private void AddOrUpdatePlayer(string name, Player player)
        {
            if (_players.Exists(name))
            {
                _players.Update(name, player);
            }
            else
            {
                _players.Add(name, player);
            }
        }
    }
}