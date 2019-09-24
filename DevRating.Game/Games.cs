using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Game
{
    public sealed class Games : Modifications
    {
        private readonly string _sha;
        private readonly string _author;
        private readonly Formula _formula;
        private readonly double _threshold;
        private readonly IDictionary<string, int> _deletions;
        private int _additions;

        public Games(string sha, string author, Formula formula, double threshold)
        {
            _sha = sha;
            _author = author;
            _formula = formula;
            _threshold = threshold;

            _deletions = new Dictionary<string, int>();
        }

        public void AddDeletion(string victim)
        {
            lock (_deletions)
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
        }

        public void AddAdditions(int count)
        {
            lock (_deletions)
            {
                _additions += count;
            }
        }

        public async Task PushInto(Matches matches)
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
                    await matches.Lock(author);
                }

                await PushDeletionsInto(matches);

                await PushAdditionsInto(matches);

                await matches.Sync();
            }
            finally
            {
                authors.Reverse();

                foreach (var author in authors)
                {
                    await matches.Unlock(author);
                }
            }
        }

        private async Task PushAdditionsInto(Matches matches)
        {
            if (_additions == 0)
            {
                return;
            }

            var winner = await matches.Points(_author);

            var reward = _formula.WinProbability(winner, _threshold) * _additions;

            await matches.Add(_author, _sha, winner, reward, _additions);
        }

        private async Task PushDeletionsInto(Matches matches)
        {
            foreach (var deletion in _deletions)
            {
                var victim = deletion.Key;
                var count = deletion.Value;

                var loser = await matches.Points(victim);
                var winner = await matches.Points(_author);

                // multiplying to 'count' is gross simplification
                var extra = _formula.WinnerExtraPoints(winner, loser) * count;
                var reward = _formula.WinProbability(winner, loser) * count;

                await matches.Add(victim, _author, _sha, loser - extra, 0d, count);
                await matches.Add(_author, victim, _sha, winner + extra, reward, count);
            }
        }
    }
}