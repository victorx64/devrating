using System.Collections.Generic;

namespace DevRating.Git
{
    public interface File
    {
        IEnumerable<AuthorChange> ChangedAuthors();
        File PatchedFile(bool binary, string author, string patch);
    }
}