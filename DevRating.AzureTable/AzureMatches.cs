using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Game;
using Microsoft.Azure.Cosmos.Table;

namespace DevRating.AzureTable
{
    public sealed class AzureMatches : Matches
    {
        private readonly double _points;
        private readonly string _container;
        private readonly string _connection;
        private readonly CloudTable _table;
        private readonly IDictionary<string, Player> _players;

        public AzureMatches(string connection, string container, string table, double points)
            : this(connection, container, Table(connection, table), points)
        {
        }

        public AzureMatches(string connection, string container, CloudTable table, double points)
        {
            _connection = connection;
            _table = table;
            _container = container;
            _points = points;
            _players = new Dictionary<string, Player>();
        }

        public Task<double> Points(string player)
        {
            return _players[player].Points();
        }

        public Task Add(string player, string contender, string commit, double points, double reward, int rounds)
        {
            var type = player.Equals(contender) ? (byte) 1 : (byte) 0;

            return Add(player, contender, commit, points, reward, rounds, type);
        }

        public Task Add(string player, string commit, double points, double reward, int rounds)
        {
            return Add(player, string.Empty, commit, points, reward, rounds, 2);
        }

        private Task Add(string player, string contender, string commit, double points, double reward, int rounds,
            byte type)
        {
            return _players[player].AddMatch(contender, commit, points, reward, rounds, type);
        }

        public Task<IEnumerable<Match>> Matches(string player)
        {
            return _players[player].Matches();
        }

        public async Task Lock(string player)
        {
            _players.Add(player, new Player(player, _container, _connection, _table, _points));

            await _players[player].Lock();
        }

        public async Task Unlock(string player)
        {
            await _players[player].Unlock();

            _players.Remove(player);
        }

        public async Task Sync()
        {
            foreach (var player in _players.Values)
            {
                player.ThrowIfNotLocked();
            }

            try
            {
                foreach (var player in _players.Values)
                {
                    await player.Sync();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error has occured while syncing", exception);
            }
        }

        private static CloudTable Table(string connection, string table)
        {
            var storageAccount = CloudStorageAccount.Parse(connection);

            var client = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            var reference = client.GetTableReference(table);

            reference.CreateIfNotExists();

            return reference;
        }
    }
}