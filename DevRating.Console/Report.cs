using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class Report
    {
        private readonly AuthorsCollection _authors;
        private readonly Output _channels;

        public Report(AuthorsCollection authors, Output channels)
        {
            _authors = authors;
            _channels = channels;
        }

        public void Print()
        {
            var authors = _authors
                .Authors()
                .Result
                .ToList();

            authors.Sort(Comparison);

            authors.Reverse();

            _channels.WriteLine("Author, Wins, Defeats, Points");
            
            foreach (var author in authors)
            {
                _channels.Write($"{author.Key}, ");

                author.Value.Print(_channels);
            }
        }

        private int Comparison(KeyValuePair<string, Player> a, KeyValuePair<string, Player> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}