using devrating.entity;

namespace devrating.factory;

public interface AuthorFactory
{
    Id AuthorAtOrg(
        string organization,
        string repository,
        string email,
        DateTimeOffset createdAt
    );
}
