using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeGetWorkOperation : GetWorkOperation
{
    private readonly IList<Work> _works;

    public FakeGetWorkOperation(IList<Work> works)
    {
        _works = works;
    }

    public Work Work(string organization, string repository, string commit)
    {
        bool Predicate(Work work)
        {
            return work.Author().Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                work.Author().Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                work.Commit().Equals(commit, StringComparison.OrdinalIgnoreCase);
        }

        return _works.Single(Predicate);
    }

    public Work Work(Id id)
    {
        bool Predicate(Entity a)
        {
            return a.Id().Value().Equals(id.Value());
        }

        return _works.Single(Predicate);
    }

    public IEnumerable<Work> Last(string organization, string repository, DateTimeOffset after)
    {
        return _works;
    }
}
