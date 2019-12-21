using System;
using System.Collections.Generic;
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
                {"show", delegate { application.PrintToConsole(new LibGit2Diff(_args[1], _args[2], _args[3])); }},
                {"add", delegate { application.Save(new LibGit2Diff(_args[1], _args[2], _args[3])); }},
                {"top", application.Top}
            };
        }

        public void Run()
        {
            if (_args.Length > 0 && _actions.ContainsKey(_args[0]))
            {
                _actions[_args[0]].Invoke();
            }
            else
            {
                PrintUsage();
            }
        }

        private void PrintUsage()
        {
            Console.WriteLine("Dev Rating evaluates developers rating and rewards based on git diff.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  devrating top");
            Console.WriteLine("  devrating show <path> <before> <after>");
            Console.WriteLine("  devrating add <path> <before> <after>");
            Console.WriteLine();
            Console.WriteLine("Description:");
            Console.WriteLine("  top        Print the rating");
            Console.WriteLine("  show       Print a reward for the work between commits");
            Console.WriteLine("  add        Update the rating by committing the work between commits");
            Console.WriteLine("  <path>     Path to a local repository. E.g. '~/repos/devrating'");
            Console.WriteLine("  <before>   Sha of the parent commit of the first commit of the work");
            Console.WriteLine("  <after>    Sha of the last commit of the work");
        }
    }
}