using System;
using System.Collections.Generic;
using DevRating.Domain;
using DevRating.LibGit2SharpClient;

namespace DevRating.ConsoleApp
{
    internal sealed class ConsoleArguments : Arguments
    {
        private readonly string[] _args;
        private readonly IDictionary<string, Action> _actions;

        public ConsoleArguments(string[] args, Application application)
        {
            _args = args;
            _actions = new Dictionary<string, Action>
            {
                {"show", delegate { application.PrintToConsole(Diff()); }},
                {"add", delegate { application.Save(Diff()); }},
                {"clear", application.Reset},
                {"top", application.Top}
            };
        }

        public void Run()
        {
            if (_args.Length > 1 && _actions.ContainsKey(_args[0]))
            {
                _actions[_args[0]].Invoke();
            }
            else
            {
                PrintUsage();
            }
        }

        private Diff Diff()
        {
            return new LibGit2Diff(_args[1], _args[2], _args[3]);
        }

        private void PrintUsage()
        {
            Console.WriteLine("DevRating calculates developer ratings and rewards based on git log.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  devrating top");
            Console.WriteLine("  devrating clear");
            Console.WriteLine("  devrating show <path-to-repo> <commit> <commit>");
            Console.WriteLine("  devrating add <path-to-repo> <commit> <commit>");
            Console.WriteLine();
            Console.WriteLine("Description:");
            Console.WriteLine("  top        Show the leaderboard");
            Console.WriteLine("  clear      Drop the leaderboard");
            Console.WriteLine("  show       Show the rating updates made by the difference of commits");
            Console.WriteLine("  add        Take into account the rating updates made by the difference of commits");
        }
    }
}