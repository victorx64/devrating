using DevRating.Rating;

namespace DevRating.Git
{
    public interface IFile
    {
        IFile SolidifiedFile(bool binary);
        IPlayers UpdatedPlayers(IPlayers players);
        void ApplyPatch(string author, string patch);
    }
}