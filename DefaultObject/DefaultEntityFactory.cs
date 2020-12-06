// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultEntityFactory : EntityFactory
    {
        private readonly Entities _entities;
        private readonly Formula _formula;

        public DefaultEntityFactory(Entities entities, Formula formula)
        {
            _entities = entities;
            _formula = formula;
        }

        public Work InsertedWork(
            string organization,
            string repository,
            string start,
            string end,
            string? since,
            string email,
            uint additions,
            string? link,
            DateTimeOffset createdAt
        )
        {
            var author = AuthorInOrg(organization, email, createdAt);

            return _entities.Works().InsertOperation().Insert(
                repository,
                start,
                end,
                since,
                author,
                additions,
                _entities.Ratings().GetOperation().RatingOf(author).Id(),
                link,
                createdAt
            );
        }

        public void InsertRatings(
            string organization,
            string email,
            IEnumerable<Deletion> deletions,
            Id work,
            DateTimeOffset createdAt
        )
        {
            var items = NonSelfDeletions(email, deletions);

            if (!items.Any())
            {
                return;
            }

            InsertRatings(organization, email, work, items, createdAt);
        }

        private void InsertRatings(
            string organization,
            string email,
            Id work,
            IEnumerable<Deletion> deletions,
            DateTimeOffset createdAt
        )
        {
            var author = AuthorInOrg(organization, email, createdAt);

            var winner = _entities.Ratings().GetOperation().RatingOf(author);

            var value = winner.Id().Filled() ? winner.Value() : _formula.DefaultRating();

            _entities.Ratings().InsertOperation().Insert(
                _formula.WinnerNewRating(
                    value,
                    MatchesWithInsertedLosers(organization, deletions, work, value, createdAt)
                ),
                null,
                null,
                winner.Id(),
                work,
                author,
                createdAt
            );
        }

        private IList<Deletion> NonSelfDeletions(string email, IEnumerable<Deletion> deletions)
        {
            bool NonSelfDeletion(Deletion d)
            {
                return !d.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
            }

            return deletions.Where(NonSelfDeletion).ToList();
        }

        private IEnumerable<Match> MatchesWithInsertedLosers(
            string organization,
            IEnumerable<Deletion> deletions,
            Id work,
            double winner,
            DateTimeOffset createdAt
        )
        {
            var matches = new List<Match>();

            foreach (var deletion in deletions)
            {
                var victim = AuthorInOrg(organization, deletion.Email(), createdAt);

                var current = _entities.Ratings().GetOperation().RatingOf(victim);

                var value = current.Id().Filled() ? current.Value() : _formula.DefaultRating();

                _entities.Ratings().InsertOperation().Insert(
                    _formula.LoserNewRating(value, new DefaultMatch(winner, deletion.Counted())),
                    deletion.Counted(),
                    deletion.Ignored(),
                    current.Id(),
                    work,
                    victim,
                    createdAt
                );

                matches.Add(new DefaultMatch(value, deletion.Counted()));
            }

            return matches;
        }

        private Id AuthorInOrg(string organization, string email, DateTimeOffset createdAt)
        {
            return _entities.Authors().ContainsOperation().Contains(organization, email)
                ? _entities.Authors().GetOperation().Author(organization, email).Id()
                : _entities.Authors().InsertOperation().Insert(organization, email, createdAt).Id();
        }
    }
}