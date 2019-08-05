using System.Collections.Generic;

namespace DevRating.Git
{
    public interface IFile
    {
        IEnumerable<AuthorChange> ChangedAuthors();
        IFile PatchedFile(bool binary, string author, string patch);
    }
}