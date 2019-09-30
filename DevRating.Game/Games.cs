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
        private readonly Formula _formula;
        private readonly double _threshold;
        private readonly IDictionary<PlayersPair, int> _deletions;
        private readonly IDictionary<string, int> _additions;

        public Games(string sha, Formula formula, double threshold)
        {
            _sha = sha;
            _formula = formula;
            _threshold = threshold;

            _deletions = new Dictionary<PlayersPair, int>();
            _additions = new Dictionary<string, int>();
        }

        public void AddDeletion(string author, string victim)
        {
            lock (_deletions)
            {
                var pair = new PlayersPair(author, victim);

                if (_deletions.ContainsKey(pair))
                {
                    ++_deletions[pair];
                }
                else
                {
                    _deletions.Add(pair, 1);
                }
            }
        }

        public void AddAdditions(string author, int count)
        {
            lock (_deletions)
            {
                if (_additions.ContainsKey(author))
                {
                    _additions[author] += count;
                }
                else
                {
                    _additions.Add(author, count);
                }
            }
        }

        public async Task PushInto(Matches matches)
        {
            var authors = UniqueAuthors();

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

        private List<string> UniqueAuthors()
        {
            var keys = _deletions.Keys
                .SelectMany(k => new[] {k.First(), k.Second()})
                .ToList();

            keys.AddRange(_additions.Keys);

            var authors = keys
                .Distinct()
                .ToList();

            return authors;
        }

        private async Task PushAdditionsInto(Matches matches)
        {
            foreach (var addition in _additions)
            {
                var winner = await matches.Points(addition.Key);

                var reward = _formula.WinProbability(winner, _threshold) * addition.Value;

                await matches.Add(addition.Key, _sha, winner, reward, addition.Value);
            }
        }

        private async Task PushDeletionsInto(Matches matches)
        {
            foreach (var deletion in _deletions)
            {
                var author = deletion.Key.First();
                var victim = deletion.Key.Second();
                var count = deletion.Value;

                var loser = await matches.Points(victim);
                var winner = await matches.Points(author);

                // multiplying to 'count' is gross simplification
                var extra = _formula.WinnerExtraPoints(winner, loser) * count;
                var reward = _formula.WinProbability(winner, loser) * count;

                await matches.Add(victim, author, _sha, loser - extra, 0d, count);
                await matches.Add(author, victim, _sha, winner + extra, reward, count);
            }
        }
    }
}