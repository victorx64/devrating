using devrating.entity;

namespace devrating.factory;

public interface WorkFactory
{
    Work NewWork(
        string organization,
        string repository,
        string commit,
        string? since,
        string email,
        string? link,
        DateTimeOffset createdAt,
        IEnumerable<string> paths
    );
}
