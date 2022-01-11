namespace devrating.entity;

public interface Id : IEquatable<Id>
{
    object Value();
    bool Filled();
}
