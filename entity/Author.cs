namespace devrating.entity;

public interface Author : Entity
{
    string Email();
    string Repository();
    string Organization();
    DateTimeOffset CreatedAt();
}
