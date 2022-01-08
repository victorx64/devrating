namespace devrating.entity;

public interface Authors
{
    GetAuthorOperation GetOperation();
    InsertAuthorOperation InsertOperation();
    ContainsAuthorOperation ContainsOperation();
}
