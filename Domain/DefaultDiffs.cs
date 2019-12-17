using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain
{
    public sealed class DefaultDiffs : Diffs
    {
        private readonly Database _database;
        private readonly Formula _formula;

        public DefaultDiffs(Database database, Formula formula)
        {
            _database = database;
            _formula = formula;
        }

        public Database Database()
        {
            return _database;
        }

        public Formula Formula()
        {
            return _formula;
        }

        public void Insert(string repository, string start, string end, string email, uint additions,
            IDictionary<string, uint> deletions)
        {
            var author = Author(email);

            var work = InsertedWork(repository, start, end, additions, author);

            deletions.Remove(email);

            if (deletions.Count > 0)
            {
                InsertAuthorNewRating(author, deletions, work);

                InsertVictimsNewRatings(deletions, work, RatingOf(author));
            }
        }

        private Author Author(string email)
        {
            return _database.Authors().Contains(email)
                ? _database.Authors().Author(email)
                : _database.Authors().Insert(email);
        }

        private Work InsertedWork(string repository, string start, string end, uint additions, Entity author)
        {
            if (_database.Ratings().ContainsRatingOf(author))
            {
                return _database.Works().Insert(repository, start, end, author, additions,
                    _database.Ratings().RatingOf(author));
            }
            else
            {
                return _database.Works().Insert(repository, start, end, author, additions);
            }
        }

        private double RatingOf(Entity author)
        {
            return _database.Ratings().ContainsRatingOf(author)
                ? _database.Ratings().RatingOf(author).Value()
                : _formula.DefaultRating();
        }

        private void InsertAuthorNewRating(Entity author, IDictionary<string, uint> deletions, Entity work)
        {
            var @new = _formula.WinnerNewRating(RatingOf(author), deletions.Select(Match));

            if (_database.Ratings().ContainsRatingOf(author))
            {
                _database.Ratings().Insert(author, @new, _database.Ratings().RatingOf(author), work);
            }
            else
            {
                _database.Ratings().Insert(author, @new, work);
            }
        }

        private void InsertVictimsNewRatings(IDictionary<string, uint> deletions, Entity work, double rating)
        {
            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Key);

                var current = RatingOf(victim);

                var @new = _formula.LoserNewRating(current, new DefaultMatch(rating, deletion.Value));

                if (_database.Ratings().ContainsRatingOf(victim))
                {
                    _database.Ratings().Insert(victim, @new, _database.Ratings().RatingOf(victim), work);
                }
                else
                {
                    _database.Ratings().Insert(victim, @new, work);
                }
            }
        }

        private Match Match(KeyValuePair<string, uint> deletion)
        {
            return new DefaultMatch(RatingOf(Author(deletion.Key)), deletion.Value);
        }
    }
}