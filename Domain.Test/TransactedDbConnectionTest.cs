using System.Data;
using DevRating.Domain.Fake;
using Xunit;

namespace DevRating.Domain.Test
{
    public sealed class TransactedDbConnectionTest
    {
        [Fact]
        public void DisposesConnection()
        {
            var connection = new TransactedDbConnection(new FakeDbConnection(new FakeDbCommand()));

            connection.Open();

            connection.Dispose();

            Assert.Equal(ConnectionState.Closed, connection.State);
        }

        [Fact]
        public void BeginsTransaction()
        {
            Assert.Equal(IsolationLevel.Unspecified,
                new TransactedDbConnection(
                        new FakeDbConnection(
                            new FakeDbCommand()))
                    .BeginTransaction()
                    .IsolationLevel);
        }

        [Fact]
        public void BeginsTransactionWithIsolationLevel()
        {
            Assert.Equal(IsolationLevel.Chaos,
                new TransactedDbConnection(
                        new FakeDbConnection(
                            new FakeDbCommand()))
                    .BeginTransaction(IsolationLevel.Chaos)
                    .IsolationLevel);
        }

        [Fact]
        public void ChangesDatabaseName()
        {
            var connection = new TransactedDbConnection(
                new FakeDbConnection(
                    new FakeDbCommand()));

            var name = "new database name";

            connection.ChangeDatabase(name);

            Assert.Equal(name, connection.Database);
        }

        [Fact]
        public void ClosesConnection()
        {
            var connection = new TransactedDbConnection(
                new FakeDbConnection(
                    new FakeDbCommand()));

            connection.Open();

            connection.Close();

            Assert.Equal(ConnectionState.Closed, connection.State);
        }

        [Fact]
        public void OpensConnection()
        {
            var connection = new TransactedDbConnection(
                new FakeDbConnection(
                    new FakeDbCommand()));

            connection.Close();

            connection.Open();

            Assert.Equal(ConnectionState.Open, connection.State);
        }

        [Fact]
        public void CreatesCommand()
        {
            var command = new FakeDbCommand();

            Assert.Equal(command, new TransactedDbConnection(new FakeDbConnection(command)).CreateCommand());
        }

        [Fact]
        public void CreatesTransactedCommand()
        {
            var connection = new TransactedDbConnection(new FakeDbConnection(new FakeDbCommand()));

            Assert.Equal(connection.BeginTransaction(), connection.CreateCommand().Transaction);
        }

        [Fact]
        public void ReturnsOriginConnectionString()
        {
            var origin = new FakeDbConnection(new FakeDbCommand()) {ConnectionString = "test"};

            Assert.Equal(origin.ConnectionString, new TransactedDbConnection(origin).ConnectionString);
        }

        [Fact]
        public void SetsOriginConnectionString()
        {
            var origin = new FakeDbConnection(new FakeDbCommand()) {ConnectionString = "test"};

            new TransactedDbConnection(origin).ConnectionString = "not test";

            Assert.Equal("not test", origin.ConnectionString);
        }

        [Fact]
        public void ReturnsOriginConnectionTimeout()
        {
            var origin = new FakeDbConnection(new FakeDbCommand());

            Assert.Equal(origin.ConnectionTimeout, new TransactedDbConnection(origin).ConnectionTimeout);
        }
    }
}