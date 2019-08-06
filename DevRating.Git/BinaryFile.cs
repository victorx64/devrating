using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class BinaryFile : File
    {
        public IEnumerable<DefaultAuthorChange> ChangedAuthors()
        {
            return new List<DefaultAuthorChange>();
        }

        public File PatchedFile(bool binary, string author, string patch)
        {
            return new BinaryFile();
        }
    }
}