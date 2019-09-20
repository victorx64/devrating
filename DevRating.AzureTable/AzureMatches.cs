using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Game;
using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    public class AzureMatches : Matches
    {
        private readonly CloudTable _table;

        public AzureMatches()
        {
        }

        public AzureMatches(CloudTable table)
        {
            _table = table;
        }
        
        public Task<double> Points(string player)
        {
            throw new System.NotImplementedException();
        }

        public Task Add(string player, string contender, string commit, double points, double reward, int rounds)
        {
            throw new System.NotImplementedException();
        }

        public Task Add(string player, string commit, double points, double reward, int rounds)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Match>> Matches(string player)
        {
            throw new System.NotImplementedException();
        }

        public Task Lock(string player)
        {
            throw new System.NotImplementedException();
        }

        public Task Unlock(string player)
        {
            throw new System.NotImplementedException();
        }

        public Task Sync()
        {
            throw new System.NotImplementedException();
        }
    }
}