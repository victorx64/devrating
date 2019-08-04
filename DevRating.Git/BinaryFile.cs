using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class BinaryFile : IFile
    {
        public IPlayers UpdatedDevelopers(IPlayers developers)
        {
            return developers;
        }

        public IFile PatchedFile(bool binary, string author, string patch)
        {
            return new BinaryFile();
        }
    }
}