using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class Report
    {
        private readonly AuthorsCollection _authors;
        private readonly Output _channel;

        public Report(AuthorsCollection authors, Output channel)
        {
            _authors = authors;
            _channel = channel;
        }

        public void Print()
        {
            var authors = _authors
                .Authors()
                .Result
                .ToList();

            authors.Sort(Comparison);

            authors.Reverse();

            _channel.WriteLine("Author, Wins, Defeats, Points");
            
            foreach (var author in authors)
            {
                _channel.Write($"{author.Key}, ");

                author.Value.Print(_channel);
            }
        }

        private int Comparison(KeyValuePair<string, Player> a, KeyValuePair<string, Player> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}