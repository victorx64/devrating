using System;

namespace DevRating.Domain
{
    public interface InsertWorkOperation
    {
        Work Insert(string repository, string start, string end, Id author, uint additions, Id rating,
            Envelope<IConvertible> link);
    }
}