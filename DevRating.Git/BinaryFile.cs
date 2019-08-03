using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class BinaryFile : IFile
    {
        public IPlayers UpdatedPlayers(IPlayers players)
        {
            return players;
        }

        public IFile PatchedFile(bool binary, string author, string patch)
        {
            return new BinaryFile();
        }
    }
}