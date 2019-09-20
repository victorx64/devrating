using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Game
{
    public sealed class GamesLog : Log
    {
        private readonly Matches _matches;
        private readonly Formula _formula;
        private readonly double _threshold;
        private readonly string _commit;
        private readonly string _author;
        private readonly IDictionary<string, int> _deletions;
        private int _additions;

        public GamesLog(Matches matches, Formula formula, double threshold, string commit, string author)
        {
            _matches = matches;
            _formula = formula;
            _threshold = threshold;
            _commit = commit;
            _author = author;

            _deletions = new Dictionary<string, int>();
        }

        public void LogDeletion(string victim)
        {
            if (_deletions.ContainsKey(victim))
            {
                ++_deletions[victim];
            }
            else
            {
                _deletions.Add(victim, 1);
            }
        }

        public void LogAddition()
        {
            ++_additions;
        }

        public async Task Push()
        {
            var authors = _deletions.Keys.ToList();

            if (!authors.Contains(_author))
            {
                authors.Add(_author);
            }

            authors.Sort();

            try
            {
                foreach (var author in authors)
                {
                    await _matches.Lock(author);
                }

                await PushDeletionMatches();

                await PushAdditionMatches();

                await _matches.Sync();
            }
            finally
            {
                authors.Reverse();

                foreach (var author in authors)
                {
                    await _matches.Unlock(author);
                }
            }
        }

        private async Task PushAdditionMatches()
        {
            var winner = await _matches.Points(_author);

            var reward = _formula.WinProbability(winner, _threshold) * _additions;

            await _matches.Add(_author, _commit, winner, reward, _additions);
        }

        private async Task PushDeletionMatches()
        {
            foreach (var deletion in _deletions)
            {
                var victim = deletion.Key;
                var count = deletion.Value;

                var loser = await _matches.Points(victim);
                var winner = await _matches.Points(_author);

                // multiplying to 'count' is gross simplification
                var extra = _formula.WinnerExtraPoints(winner, loser) * count;
                var reward = _formula.WinProbability(winner, loser) * count;

                await _matches.Add(victim, _author, _commit, loser - extra, 0d, count);
                await _matches.Add(_author, victim, _commit, winner + extra, reward, count);
            }
        }
    }
}