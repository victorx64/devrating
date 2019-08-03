using DevRating.Rating;

namespace DevRating.Git
{
    public interface IFile
    {
        IPlayers UpdatedPlayers(IPlayers players);
        IFile PatchedFile(bool binary, string author, string patch);
    }
}