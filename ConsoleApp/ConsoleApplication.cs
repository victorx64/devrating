using System;
using DevRating.Domain;
using DevRating.LibGit2SharpClient;

namespace DevRating.ConsoleApp
{
    internal sealed class ConsoleApplication : Application
    {
        private readonly Diffs _diffs;

        public ConsoleApplication(Diffs diffs)
        {
            _diffs = diffs;
        }

        public void Top()
        {
            _diffs.Database().Connection().Open();

            using var transaction = _diffs.Database().Connection().BeginTransaction();

            try
            {
                if (!_diffs.Database().Exist())
                {
                    _diffs.Database().Create();
                }

                foreach (var author in _diffs.Database().Authors().TopAuthors())
                {
                    var percentile = _diffs.Formula()
                        .WinProbabilityOfA(author.Rating().Value(), _diffs.Formula().DefaultRating());

                    Console.WriteLine($"{author.Email()} {author.Rating().Value():F2} ({percentile:P} percentile)");
                }
            }
            finally
            {
                transaction.Rollback();
                _diffs.Database().Connection().Close();
            }
        }

        public void Save(string path, string start, string end)
        {
            var diff = new LibGit2Diff(path, start, end);

            _diffs.Database().Connection().Open();

            using var transaction = _diffs.Database().Connection().BeginTransaction();

            try
            {
                if (!_diffs.Database().Exist())
                {
                    _diffs.Database().Create();
                }

                if (_diffs.Database().Works().Contains(diff.RepositoryName(), start, end))
                {
                    throw new Exception("The diff is already added.");
                }

                diff.AddTo(_diffs);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                _diffs.Database().Connection().Close();
            }
        }

        public void PrintToConsole(string path, string start, string end)
        {
            var diff = new LibGit2Diff(path, start, end);

            _diffs.Database().Connection().Open();

            using var transaction = _diffs.Database().Connection().BeginTransaction();

            try
            {
                if (!_diffs.Database().Exist())
                {
                    _diffs.Database().Create();
                }

                if (!_diffs.Database().Works().Contains(diff.RepositoryName(), start, end))
                {
                    diff.AddTo(_diffs);

                    Console.WriteLine("To add these updates run `devrating add <path> <before> <after>`.");
                    Console.WriteLine();
                }

                PrintWorkToConsole(_diffs.Database().Works().Work(diff.RepositoryName(), start, end));
            }
            finally
            {
                transaction.Rollback();
                _diffs.Database().Connection().Close();
            }
        }

        private void PrintWorkToConsole(Work work)
        {
            var rating = work.HasUsedRating()
                ? work.UsedRating().Value()
                : _diffs.Formula().DefaultRating();

            var percentile = _diffs.Formula().WinProbabilityOfA(rating, _diffs.Formula().DefaultRating());

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
                var percentile = _diffs.Formula().WinProbabilityOfA(rating.Value(), _diffs.Formula().DefaultRating());

                var previous = rating.HasPreviousRating()
                    ? rating.PreviousRating().Value()
                    : _diffs.Formula().DefaultRating();

                Console.WriteLine(
                    $"{rating.Author().Email()} {previous:F2} -> {rating.Value():F2} ({percentile:P} percentile)");
            }
        }
    }
}