using System;
using System.Data;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal sealed class Application
    {
        private readonly Diff _diff;
        private readonly IDbConnection _connection;
        private readonly WorksRepository _works;

        public Application(Diff diff, IDbConnection connection, WorksRepository works)
        {
            _diff = diff;
            _connection = connection;
            _works = works;
        }

        public void PrintToConsole()
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                _diff.AddTo(_works);

                PrintSavedToConsole();
            }
            finally
            {
                transaction.Rollback();
                _connection.Close();
            }
        }

        public void PrintSavedToConsole()
        {
            var work = _works.Work(_diff.Key());

            Console.WriteLine("Reward:");
            Console.WriteLine($"{work.Author().Email()} {work.Reward():F2}");
            Console.WriteLine("Rating updates:");

            foreach (var rating in work.Ratings())
            {
                Console.WriteLine(rating.HasPreviousRating()
                    ? $"{rating.Author().Email()} {rating.PreviousRating().Value():F2} -> {rating.Value():F2}"
                    : $"{rating.Author().Email()} {rating.Value():F2}");
            }
        }

        public void Save()
        {
            _connection.Open();

            var transaction = _connection.BeginTransaction();

            try
            {
                _diff.AddTo(_works);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}