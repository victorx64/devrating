namespace devrating.entity;

public interface InsertAuthorOperation
{
    Author Insert(string organization, string repository, string email, DateTimeOffset createdAt);
}
