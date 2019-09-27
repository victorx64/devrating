namespace DevRating.Git.Test
{
    public class FakeModificationsFactory : ModificationsFactory
    {
        public Modifications Modifications(string sha, string author)
        {
            return new FakeModifications();
        }
    }
}