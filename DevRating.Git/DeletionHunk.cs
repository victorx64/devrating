using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class DeletionHunk : Hunk, IDeletionHunk
    {
        public DeletionHunk(string author, string header) : base(author, header)
        {
        }

        public IList<string> DeleteFrom(IEnumerable<string> authors)
        {
            var result = new List<string>(authors);

            if (Count > 0)
            {
                result.RemoveRange(Index, Count);
            }

            return result;
        }

        public IEnumerable<AuthorChange> ChangedAuthors(IList<string> authors)
        {
            var result = new List<AuthorChange>();

            for (var i = Index; i < Index + Count; i++)
            {
                if (!authors[i].Equals(Author))
                {
                    result.Add(new AuthorChange(authors[i], Author));
                }
            }

            return result;
        }

        public int CompareTo(IDeletionHunk other)
        {
            return base.CompareTo((Hunk)other);
        }
    }
}