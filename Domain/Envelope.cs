using System;

namespace DevRating.Domain
{
    public interface Envelope
    {
        IConvertible Value();
        bool Filled();
    }
}