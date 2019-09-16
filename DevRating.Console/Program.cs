using System.Collections.Generic;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var git = new Git.Git(
                ".",
                "",
                "");

            var authors = new Players();

            git.ExtendAuthorChanges(authors);
        }
    }
    
    internal class Players : AuthorChanges
    {
        private readonly IDictionary<string, Player> _players;

        public Players() : this(new Dictionary<string, Player>())
        {
        }
        
        public Players(IDictionary<string, Player> players)
        {
            _players = players;
        }
        
        public void AddChange(string previous, string next, string commit)
        {
            throw new System.NotImplementedException();
        }

        public void AddChange(string next, string commit)
        {
            throw new System.NotImplementedException();
        }
    }
}