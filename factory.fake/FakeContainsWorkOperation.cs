using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeContainsWorkOperation : ContainsWorkOperation
{
    private readonly IList<Work> _works;

    public FakeContainsWorkOperation(IList<Work> works)
    {
        _works = works;
    }

    public bool Contains(string organization, string repository, string commit)
    {
        bool Predicate(Work work)
        {
            return work.Author().Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                   work.Author().Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                   work.Commit().Equals(commit, StringComparison.OrdinalIgnoreCase);
        }

        return _works.Any(Predicate);
    }

    public bool Contains(Id id)
    {
        bool Predicate(Entity a)
        {
            return a.Id().Value().Equals(id.Value());
        }

        return _works.Any(Predicate);
    }

    public bool Contains(string organization, string repository, DateTimeOffset after)
    {
        bool Predicate(Work work)
        {
            return work.Author().Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                   work.Author().Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                   work.CreatedAt() >= after;
        }

        return _works.Any(Predicate);
    }
}
