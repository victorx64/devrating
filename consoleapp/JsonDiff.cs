using System.Text.Json;
using devrating.entity;
using devrating.factory;
using devrating.git;

namespace devrating.consoleapp;

public sealed class JsonDiff : Diff
{
    private class Dto
    {
        public string Email { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string? Since { get; set; } = default;
        public string Repository { get; set; } = string.Empty;
        public string? Link { get; set; } = default;
        public IEnumerable<ContemporaryLinesDto> Deletions { get; set; } = Array.Empty<ContemporaryLinesDto>();
        public DateTimeOffset CreatedAt { get; set; } = default;
        internal class ContemporaryLinesDto
        {
            public string VictimEmail { get; set; } = string.Empty;
            public uint DeletedLines { get; set; } = default;
            public uint AllLines { get; set; } = default;
            public bool DeletionAccountable { get; set; } = default;
        }
    }

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
