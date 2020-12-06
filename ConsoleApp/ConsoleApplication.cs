// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    public sealed class ConsoleApplication : Application
    {
        private readonly Database _database;
        private readonly Formula _formula;

        public ConsoleApplication(Database database, Formula formula)
        {
            _database = database;
            _formula = formula;
        }

        public void Top(Output output, string organization)
        {
            _database.Instance().Connection().Open();

            using var transaction = _database.Instance().Connection().BeginTransaction();

            try
            {
                if (!_database.Instance().Present())
                {
                    _database.Instance().Create();
                }

                foreach (var author in _database.Entities().Authors().GetOperation().TopOfOrganization(organization))
                {
                    output.WriteLine(
                        $"<{author.Email()}> {_database.Entities().Ratings().GetOperation().RatingOf(author.Id()).Value():F2}"
                    );
                }
            }
            finally
            {
                transaction.Rollback();
                _database.Instance().Connection().Close();
            }
        }

        public void Save(Diff diff)
        {
            _database.Instance().Connection().Open();

            using var transaction = _database.Instance().Connection().BeginTransaction();

            try
            {
                if (!_database.Instance().Present())
                {
                    _database.Instance().Create();
                }

                if (diff.PresentIn(_database.Entities().Works()))
                {
                    throw new InvalidOperationException("The diff is already added.");
                }

                diff.AddTo(new DefaultEntityFactory(_database.Entities(), _formula), DateTimeOffset.UtcNow);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                _database.Instance().Connection().Close();
            }
        }

        public void PrintTo(Output output, Diff diff)
        {
            _database.Instance().Connection().Open();

            using var transaction = _database.Instance().Connection().BeginTransaction();

            try
            {
                if (!_database.Instance().Present())
                {
                    _database.Instance().Create();
                }

                if (!diff.PresentIn(_database.Entities().Works()))
                {
                    throw new InvalidOperationException("The diff is not present in the database. " +
                        "To insert, run `devrating add <path> (<base> <head> | <merge>) -l [<link>]`.");
                }

                PrintWorkToConsole(output, diff.From(_database.Entities().Works()));
            }
            finally
            {
                transaction.Rollback();
                _database.Instance().Connection().Close();
            }
        }

        private void PrintWorkToConsole(Output output, Work work)
        {
            if (work.Link() is object)
            {
                output.WriteLine($"Link: {work.Link()}");
            }

            if (work.Since() is object)
            {
                output.WriteLine($"Since: {work.Since()}");
            }

            var usedRating = work.UsedRating();

            var rating = usedRating.Id().Filled()
                ? usedRating.Value()
                : _formula.DefaultRating();

            var percentile = _formula.WinProbabilityOfA(rating, _formula.DefaultRating());

            var additions = Math.Min(work.Additions(), 250);

            output.WriteLine($"<{work.Author().Email()}> Added {work.Additions()} lines. " +
                $"Reward: {additions / (1d - percentile):F2}");

            PrintWorkRatingsToConsole(output, work);
        }

        private void PrintWorkRatingsToConsole(Output output, Work work)
        {
            foreach (var rating in _database.Entities().Ratings().GetOperation().RatingsOf(work.Id()).Reverse().ToList())
            {
                var previous = rating.PreviousRating();

                var before = previous.Id().Filled()
                    ? previous.Value()
                    : _formula.DefaultRating();

                var ignored = rating.IgnoredDeletions() is object
                    && rating.IgnoredDeletions() > 0
                    ? $" + {rating.IgnoredDeletions()}"
                    : "";

                var lost = rating.CountedDeletions() is object
                    ? $"Lost {rating.CountedDeletions()}{ignored} lines. "
                    : "";

                output.WriteLine($"<{rating.Author().Email()}> {lost}New rating: {before:F2} -> {rating.Value():F2}");
            }
        }
    }
}