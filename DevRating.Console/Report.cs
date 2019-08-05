using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class Report
    {
        private readonly IGit _git;
        private readonly OutputChannels _channels;

        public Report(IGit git, OutputChannels channels)
        {
            _git = git;
            _channels = channels;
        }

        public void Print()
        {
            var authors = _git
                .Authors()
                .ToList();
            
            authors.Sort(Comparison);

            foreach (var author in authors)
            {
                var channel = _channels.Channel();
                
                channel.Write($"{author.Key}, ");
                
                author.Value.Print(channel);
            }
        }

        private int Comparison(KeyValuePair<string, IPlayer> a, KeyValuePair<string, IPlayer> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}