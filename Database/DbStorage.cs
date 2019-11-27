using System.Collections.Generic;
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

            InsertAuthorNewRating(author, deletions, InsertedWork(diff, additions, author));
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

        private void InsertAuthorNewRating(DbObject author, IDictionary<string, uint> deletions,
            DbObject work)
        {
            var current = RatingOf(author);

            var @new = _formula.WinnerNewRating(current, InsertVictimsNewRatings(deletions, work, current));

            if (_ratings.HasRatingOf(author))
            {
                _ratings.Insert(author, @new, _ratings.RatingOf(author), work);
            }
            else
            {
                _ratings.Insert(author, @new, work);
            }
        }

        private IEnumerable<Match> InsertVictimsNewRatings(IDictionary<string, uint> deletions, DbObject work,
            double rating)
        {
            var matches = new List<Match>();

            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Key);

                var current = RatingOf(victim);

                matches.Add(new DbMatch(current, deletion.Value));

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

            return matches;
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