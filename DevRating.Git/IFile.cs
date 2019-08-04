using DevRating.Rating;

namespace DevRating.Git
{
    public interface IFile
    {
        IPlayers UpdatedDevelopers(IPlayers developers);
        IFile PatchedFile(bool binary, string author, string patch);
    }
}