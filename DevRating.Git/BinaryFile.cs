using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class BinaryFile : IFile
    {
        public IFile SolidifiedFile(bool binary)
        {
            return new BinaryFile();
        }

        public IPlayers UpdatedPlayers(IPlayers players)
        {
            return players;
        }

        public void ApplyPatch(string author, string patch)
        {
        }
    }
}