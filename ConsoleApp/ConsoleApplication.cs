using System;
using DevRating.Database;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal sealed class ConsoleApplication : Application
    {
        private readonly Instance _instance;
        private readonly Formula _formula;

        public ConsoleApplication(Instance instance, Formula formula)
        {
            _instance = instance;
            _formula = formula;
        }

        public void Top()
        {
            var connection = _instance.Connection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                if (!_instance.Exist())
                {
                    _instance.Create();
                }

                foreach (var author in _instance.Storage().TopAuthors())
                {
                    var percentile = _formula.WinProbabilityOfA(author.Rating().Value(), _formula.DefaultRating());

                    Console.WriteLine($"{author.Email()} {author.Rating().Value():F2} ({percentile:P} percentile)");
                }
            }
            finally
            {
                transaction.Rollback();
                connection.Close();
            }
        }

        public void Save(Diff diff)
        {
            var connection = _instance.Connection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                if (!_instance.Exist())
                {
                    _instance.Create();
                }

                if (_instance.Storage().WorkExist(diff))
                {
                    throw new Exception("The diff is already added.");
                }

                diff.AddTo(_instance.Storage());

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public void PrintToConsole(Diff diff)
        {
            var connection = _instance.Connection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                if (!_instance.Exist())
                {
                    _instance.Create();
                }

                if (!_instance.Storage().WorkExist(diff))
                {
                    diff.AddTo(_instance.Storage());

                    Console.WriteLine("To add these updates run `devrating add <path> <before> <after>`.");
                    Console.WriteLine();
                }

                PrintWorkToConsole(_instance.Storage().Work(diff));
            }
            finally
            {
                transaction.Rollback();
                connection.Close();
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

            foreach (var rating in work.Ratings())
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