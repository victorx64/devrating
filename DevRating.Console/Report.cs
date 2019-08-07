using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class Report
    {
        private readonly AuthorsCollection _authors;
        private readonly OutputChannels _channels;

        public Report(AuthorsCollection authors, OutputChannels channels)
        {
            _authors = authors;
            _channels = channels;
        }

        public void Print()
        {
            var authors = _authors
                .Authors()
                .ToList();

            authors.Sort(Comparison);

            authors.Reverse();

            var channel = _channels.Channel();

            channel.WriteLine("Author, Wins, Defeats, Points");
            
            foreach (var author in authors)
            {
                channel.Write($"{author.Key}, ");

                author.Value.Print(channel);
            }
        }

        private int Comparison(KeyValuePair<string, Player> a, KeyValuePair<string, Player> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}