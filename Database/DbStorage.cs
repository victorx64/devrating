using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.Database
{
    public sealed class DbStorage : Storage
    {
        private readonly Works _works;
        private readonly Authors _authors;
        private readonly Ratings _ratings;
        private readonly Formula _formula;

        public DbStorage(Works works, Authors authors, Ratings ratings, Formula formula)
        {
            _works = works;
            _authors = authors;
            _ratings = ratings;
            _formula = formula;
        }

        public void AddWork(Diff diff, string email, uint additions, IDictionary<string, uint> deletions)
        {
            var author = Author(email);

            var work = InsertedWork(diff, additions, author);

            deletions.Remove(email);

            if (deletions.Count > 0)
            {
                InsertAuthorNewRating(author, deletions, work);

                InsertVictimsNewRatings(deletions, work, RatingOf(author));
            }
        }

        private DbAuthor Author(string email)
        {
            return _authors.Exist(email)
                ? _authors.Author(email)
                : _authors.Insert(email);
        }

        private DbWork InsertedWork(Diff diff, uint additions, DbObject author)
        {
            if (_ratings.HasRatingOf(author))
            {
                var rating = _ratings.RatingOf(author);

                return _works.Insert(diff.Key(), diff.StartCommit(), diff.EndCommit(), author,
                    _formula.Reward(rating.Value(), additions), rating);
            }
            else
            {
                return _works.Insert(diff.Key(), diff.StartCommit(), diff.EndCommit(), author,
                    _formula.Reward(_formula.DefaultRating(), additions));
            }
        }

        private double RatingOf(DbObject author)
        {
            return _ratings.HasRatingOf(author)
                ? _ratings.RatingOf(author).Value()
                : _formula.DefaultRating();
        }

        private void InsertAuthorNewRating(DbObject author, IDictionary<string, uint> deletions, DbObject work)
        {
            var current = RatingOf(author);

            var @new = _formula.WinnerNewRating(current, deletions.Select(Match));

            if (_ratings.HasRatingOf(author))
            {
                _ratings.Insert(author, @new, _ratings.RatingOf(author), work);
            }
            else
            {
                _ratings.Insert(author, @new, work);
            }
        }

        private void InsertVictimsNewRatings(IDictionary<string, uint> deletions, DbObject work, double rating)
        {
            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Key);

                var current = RatingOf(victim);

                var @new = _formula.LoserNewRating(current, new DbMatch(rating, deletion.Value));

                if (_ratings.HasRatingOf(victim))
                {
                    _ratings.Insert(victim, @new, _ratings.RatingOf(victim), work);
                }
                else
                {
                    _ratings.Insert(victim, @new, work);
                }
            }
        }

        private Match Match(KeyValuePair<string, uint> deletion)
        {
            return new DbMatch(RatingOf(Author(deletion.Key)), deletion.Value);
        }

        public Work Work(Diff diff)
        {
            return _works.Work(diff);
        }

        public bool WorkExist(Diff diff)
        {
            return _works.Exist(diff);
        }

        public IEnumerable<Author> TopAuthors()
        {
            return _authors.TopAuthors();
        }
    }
}