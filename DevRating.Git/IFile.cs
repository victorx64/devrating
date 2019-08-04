using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface IFile
    {
        IList<IPlayer> UpdatedDevelopers(IList<IPlayer> developers);
        IFile PatchedFile(bool binary, IPlayer author, string patch);
    }
}