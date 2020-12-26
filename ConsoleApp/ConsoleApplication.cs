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

                output.WriteLine("Author | Rating");
                output.WriteLine("------ | ------");

                foreach (var author in _database.Entities().Authors().GetOperation()
                .TopOfOrganization(organization, DateTimeOffset.UtcNow - TimeSpan.FromDays(90)))
                {
                    output.WriteLine(
                        $"<{author.Email()}> | {_database.Entities().Ratings().GetOperation().RatingOf(author.Id()).Value():F2}"
                    );
                }
            }
            finally
            {
                transaction.Rollback();
                _database.Instance().Connection().Close();
            }
        }

        public void Total(Output output, string repository, DateTimeOffset after)
        {
            _database.Instance().Connection().Open();

            using var transaction = _database.Instance().Connection().BeginTransaction();

            try
            {
                if (!_database.Instance().Present())
                {
                    _database.Instance().Create();
                }

                output.WriteLine("Author | Total reward");
                output.WriteLine("------ | ------------");

                foreach (var item in _database
                    .Entities()
                    .Works()
                    .GetOperation()
                    .Last(repository, after)
                    .GroupBy(w => w.Author().Email(), Reward)
                    .Select(g => new { Key = g.Key, Sum = g.Sum() })
                    .OrderByDescending(s => s.Sum))
                {
                    output.WriteLine($"<{item.Key}> | {item.Sum:F2}");
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
                        "To insert, run `devrating add`.");
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
            output.WriteLine($"Author: <{work.Author().Email()}>");
            output.WriteLine($"base: {work.Start()}");
            output.WriteLine($"head: {work.End()}");

            if (work.Since() is object)
            {
                output.WriteLine($"Since: {work.Since()}");
            }

            if (work.Link() is object)
            {
                output.WriteLine($"Link: {work.Link()}");
            }

            output.WriteLine($"Additions: {work.Additions()}");
            output.WriteLine($"Reward: {Reward(work):F2}");

            PrintWorkRatingsToConsole(output, work);
        }

        private void PrintWorkRatingsToConsole(Output output, Work work)
        {
            output.WriteLine("Author | Lines lost | Prev rating | New rating");
            output.WriteLine("------ | ---------- | ----------- | ----------");

            foreach (var rating in _database.Entities().Ratings().GetOperation().RatingsOf(work.Id()).Reverse().ToList())
            {
                var previous = rating.PreviousRating();

                var before = previous.Id().Filled()
                    ? previous.Value()
                    : _formula.DefaultRating();

                var lost = rating.CountedDeletions() is object
                    ? rating.CountedDeletions().ToString()
                    : "0";

                output.WriteLine($"<{rating.Author().Email()}> | {lost} | {before:F2} | {rating.Value():F2}");
            }
        }

        private double Reward(Work work)
        {
            var usedRating = work.UsedRating();

            var rating = usedRating.Id().Filled()
                ? usedRating.Value()
                : _formula.DefaultRating();

            var percentile = _formula.WinProbabilityOfA(rating, _formula.DefaultRating());

            var additions = Math.Min(work.Additions(), 250);

            return additions / (1d - percentile);
        }
    }
}