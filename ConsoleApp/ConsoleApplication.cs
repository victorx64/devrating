using System;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal sealed class ConsoleApplication : Application
    {
        private readonly Database _database;
        private readonly Formula _formula;

        public ConsoleApplication(Database database, Formula formula)
        {
            _database = database;
            _formula = formula;
        }

        public void Top()
        {
            _database.Instance().Connection().Open();

            using var transaction = _database.Instance().Connection().BeginTransaction();

            try
            {
                if (!_database.Instance().Present())
                {
                    _database.Instance().Create();
                }

                foreach (var author in _database.Entities().Authors().GetOperation().Top())
                {
                    var percentile = _formula
                        .WinProbabilityOfA(
                            _database.Entities().Ratings().GetOperation().RatingOf(author).Value(),
                            _formula.DefaultRating());

                    Console.WriteLine(
                        $"{author.Email()} {_database.Entities().Ratings().GetOperation().RatingOf(author).Value():F2} ({percentile:P} percentile)");
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

                if (diff.Fingerprint().PresentIn(_database.Entities().Works()))
                {
                    throw new Exception("The diff is already added.");
                }

                diff.Fingerprint().AddTo(_database.Entities(), _formula);

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

        public void PrintToConsole(Diff diff)
        {
            _database.Instance().Connection().Open();

            using var transaction = _database.Instance().Connection().BeginTransaction();

            try
            {
                if (!_database.Instance().Present())
                {
                    _database.Instance().Create();
                }

                if (!diff.Fingerprint().PresentIn(_database.Entities().Works()))
                {
                    diff.Fingerprint().AddTo(_database.Entities(), _formula);

                    Console.WriteLine("To add these updates run `devrating add <path> <before> <after>`.");
                    Console.WriteLine();
                }

                PrintWorkToConsole(diff.Fingerprint().WorkFrom(_database.Entities().Works()));
            }
            finally
            {
                transaction.Rollback();
                _database.Instance().Connection().Close();
            }
        }

        private void PrintWorkToConsole(Work work)
        {
            var rating = work.HasUsedRating()
                ? work.UsedRating().Value()
                : _formula.DefaultRating();

            var percentile = _formula.WinProbabilityOfA(rating, _formula.DefaultRating());

            Console.WriteLine(work.Author().Email());
            Console.WriteLine($"Added {work.Additions()} lines with {rating} rating ({percentile:P} percentile)");
            Console.WriteLine(
                $"Reward = {work.Additions()} / (1 - {percentile:F2}) = {work.Additions() / (1d - percentile):F2}");
            Console.WriteLine();

            PrintWorkRatingsToConsole(work);
        }

        private void PrintWorkRatingsToConsole(Work work)
        {
            Console.WriteLine("Rating updates");

            foreach (var rating in _database.Entities().Ratings().GetOperation().RatingsOf(work))
            {
                var percentile = _formula.WinProbabilityOfA(rating.Value(), _formula.DefaultRating());

                var previous = rating.HasPreviousRating()
                    ? rating.PreviousRating().Value()
                    : _formula.DefaultRating();

                Console.WriteLine(
                    $"{rating.Author().Email()} {previous:F2} -> {rating.Value():F2} ({percentile:P} percentile)");
            }
        }
    }
}