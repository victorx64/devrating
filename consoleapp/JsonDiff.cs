using System.Text.Json;
using devrating.entity;
using devrating.factory;
using devrating.git;
using static devrating.git.GitProcessDiff;

namespace devrating.consoleapp;

public sealed class JsonDiff : Diff
{
    private readonly Dto _state;

    public JsonDiff(string json) : this(JsonSerializer.Deserialize<Dto>(json)!)
    {
    }

    private JsonDiff(Dto state)
    {
        _state = state;
    }

    public Work NewWork(Factories factories)
    {
        var work = factories.WorkFactory().NewWork(
            _state.Organization,
            _state.Repository,
            _state.Start,
            _state.End,
            _state.Since,
            _state.Email,
            _state.Link,
            _state.CreatedAt
        );

        factories.RatingFactory().NewRatings(
            _state.Organization,
            _state.Repository,
            _state.Email,
            _state.Deletions.Select(d => new GitContemporaryLines(
                d.AllLines, d.DeletedLines, d.DeletionAccountable, d.VictimEmail)),
            work.Id(),
            _state.CreatedAt
        );

        return work;
    }

    public Work RelatedWork(Works works)
    {
        return works.GetOperation().Work(_state.Organization, _state.Repository, _state.Start, _state.End);
    }

    public bool PresentIn(Works works)
    {
        return works.ContainsOperation().Contains(_state.Organization, _state.Repository, _state.Start, _state.End);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(_state);
    }
}
