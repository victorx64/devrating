using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class BinaryFile : File
    {
        public IEnumerable<AuthorChange> ChangedAuthors()
        {
            return new List<AuthorChange>();
        }

        public File PatchedFile(bool binary, string author, string patch)
        {
            return new BinaryFile();
        }
    }
}