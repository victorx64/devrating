using System;

namespace DevRating.Domain
{
    public interface Id : IEquatable<Id>
    {
        object Value();
        bool Filled();
    }
}