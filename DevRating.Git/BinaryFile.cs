using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class BinaryFile : IFile
    {
        public IList<IPlayer> UpdatedDevelopers(IList<IPlayer> developers)
        {
            return developers;
        }

        public IFile PatchedFile(bool binary, string author, string patch)
        {
            return new BinaryFile();
        }
    }
}