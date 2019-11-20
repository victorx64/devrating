using System;
using System.Collections.Generic;
using DevRating.Database;
using DevRating.Domain;

namespace DevRating.ConsoleApp
{
    internal sealed class Application
    {
        private readonly Diff _diff;
        private readonly Instance _instance;
        private readonly IDictionary<string, Action> _actions;

        public Application(Diff diff, Instance instance)
        {
            _diff = diff;
            _instance = instance;
            _actions = new Dictionary<string, Action>
            {
                {"show", PrintToConsole},
                {"show-saved", PrintSavedToConsole},
                {"save", Save},
                {"db-exist", DbExist},
                {"db-create", _instance.Create},
                {"db-drop", _instance.Drop},
            };
        }

        public void Run(string command)
        {
            _actions[command].Invoke();
        }

        private void DbExist()
        {
            Console.WriteLine(_instance.Exist());
        }

        private void PrintToConsole()
        {
            var connection = _instance.Connection();

            using var transaction = connection.BeginTransaction();

            try
            {
                _diff.AddTo(_instance.Works());

                PrintSavedToConsole();
            }
            finally
            {
                transaction.Rollback();
                connection.Close();
            }
        }

        private void PrintSavedToConsole()
        {
            var work = _instance.Works().Work(_diff.Key());

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

        private void Save()
        {
            var connection = _instance.Connection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                _diff.AddTo(_instance.Works());

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
    }
}