using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain
{
    public sealed class DefaultRatingFactory : RatingFactory
    {
        private readonly Deletions _deletions;
        private readonly string _email;
        private readonly AuthorFactory _authors;

        public DefaultRatingFactory(Deletions deletions, string email, AuthorFactory authors)
        {
            _deletions = deletions;
            _email = email;
            _authors = authors;
        }

        public void InsertNewRatings(Entities entities, Formula formula, Entity author, Entity work)
        {
            var victims = Victims();

            if (victims.Any())
            {
                InsertAuthorNewRating(entities, formula, author, victims, work);

                InsertVictimsNewRatings(entities, formula, victims, work, RatingOf(entities, formula, author));
            }
        }

        private void InsertAuthorNewRating(Entities entities, Formula formula, Entity author,
            IEnumerable<Deletion> victims, Entity work)
        {
            DefaultMatch Selector(Deletion deletion)
            {
                return new DefaultMatch(RatingOf(entities, formula, _authors.Author(entities, deletion.Email())),
                    deletion.Count());
            }

            var @new = formula.WinnerNewRating(RatingOf(entities, formula, author), victims.Select(Selector));

            entities.Ratings().InsertOperation().Insert(@new, new NullDbParameter(),
                entities.Ratings().GetOperation().RatingOf(author), work, author);
        }

        private void InsertVictimsNewRatings(Entities entities, Formula formula, IEnumerable<Deletion> victims,
            Entity work, double rating)
        {
            foreach (var deletion in victims)
            {
                var victim = _authors.Author(entities, deletion.Email());

                var current = RatingOf(entities, formula, victim);

                var @new = formula.LoserNewRating(current, new DefaultMatch(rating, deletion.Count()));

                entities.Ratings().InsertOperation()
                    .Insert(@new, new NullDbParameter(), entities.Ratings().GetOperation().RatingOf(victim), work,
                        victim);
            }
        }

        private IList<Deletion> Victims()
        {
            bool NotSelfDeletion(Deletion deletion)
            {
                return !deletion.Email().Equals(_email, StringComparison.OrdinalIgnoreCase);
            }

            return _deletions.Items().Where(NotSelfDeletion).ToList();
        }

        private double RatingOf(Entities entities, Formula formula, Entity author)
        {
            return entities.Ratings().ContainsOperation().ContainsRatingOf(author)
                ? entities.Ratings().GetOperation().RatingOf(author).Value()
                : formula.DefaultRating();
        }
    }
}