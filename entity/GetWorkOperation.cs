namespace devrating.entity;

public interface GetWorkOperation
{
    Work Work(string organization, string repository, string commit);
    Work Work(Id id);
    IEnumerable<Work> Last(string organization, string repository, DateTimeOffset after);
    IEnumerable<Work> Last(Id author, DateTimeOffset after);
}
