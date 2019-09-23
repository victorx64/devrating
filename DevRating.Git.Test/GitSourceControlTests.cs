using NUnit.Framework;

namespace DevRating.Git.Test
{
    public class GitSourceControlTests
    {
        [Test]
        public void LowPointsOnHigherWinner()
        {
            var commit = new Commit(new FakeRepository(), "commit");

            var modifications = commit.Modifications(new FakeModificationsFactory());

            // Assert...
        }
    }
}