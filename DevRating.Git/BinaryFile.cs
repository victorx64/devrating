using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class BinaryFile : IFile
    {
        public IEnumerable<AuthorChange> ChangedAuthors()
        {
            return new List<AuthorChange>();
        }

        public IFile PatchedFile(bool binary, string author, string patch)
        {
            return new BinaryFile();
        }
    }
}