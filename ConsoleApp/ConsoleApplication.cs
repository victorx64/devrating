using System;
using DevRating.Database;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal sealed class ConsoleApplication : Application
    {
        private readonly Instance _instance;

        public ConsoleApplication(Instance instance)
        {
            _instance = instance;
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
                    Console.WriteLine($"{author.Email()} {author.Rating().Value():F2}");
                }
            }
            finally
            {
                transaction.Rollback();
                connection.Close();
            }
        }

        public void Reset()
        {
            var connection = _instance.Connection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                if (_instance.Exist())
                {
                    _instance.Drop();
                }

                _instance.Create();

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

                    Console.WriteLine("To add these updates run `devrating add <path-to-repo> <commit> <commit>`.");
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
            Console.WriteLine("Reward");
            Console.WriteLine($"{work.Author().Email()} {work.Reward():F2}");
            Console.WriteLine();
            Console.WriteLine("Rating updates");

            foreach (var rating in work.Ratings())
            {
                Console.WriteLine(rating.HasPreviousRating()
                    ? $"{rating.Author().Email()} {rating.PreviousRating().Value():F2} -> {rating.Value():F2}"
                    : $"{rating.Author().Email()} {rating.Value():F2}");
            }
        }
    }
}