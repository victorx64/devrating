using System.Text.Json;
using devrating.entity;
using devrating.factory;

namespace devrating.git;

public sealed class GitProcessDiff : Diff
{
    private readonly string _start;
    private readonly string _end;
    private readonly string? _since;
    private readonly Deletions _deletions;
    private readonly string _key;
    private readonly string _organization;
    private readonly string? _link;
    private readonly string _email;
    private readonly DateTimeOffset _createdAt;

    public GitProcessDiff(
        string start,
        string end,
        string? since,
        string repository,
        string branch,
        string key,
        string? link,
        string organization,
        DateTimeOffset createdAt,
        string? email = null
    )
        : this(
            start,
            end,
            since,
            repository,
            new CachedPatches(new GitProcessPatches(start, end, since, repository, branch)),
            key,
            link,
            organization,
            createdAt,
            email
        )
    {
    }

    public GitProcessDiff(
        string start,
        string end,
        string? since,
        string repository,
        Patches patches,
        string key,
        string? link,
        string organization,
        DateTimeOffset createdAt,
        string? email = null
    )
        : this(
            new GitProcess("git", $"rev-parse {start}", repository).Output()[0],
            new GitProcess("git", $"rev-parse {end}", repository).Output()[0],
            since,
            new TotalDeletions(patches),
            key,
            link,
            organization,
            createdAt,
            email ?? new GitProcess("git", $"show -s --format=%aE {end}", repository).Output()[0]
        )
    {
    }

    private GitProcessDiff(
        string start,
        string end,
        string? since,
        Deletions deletions,
        string key,
        string? link,
        string organization,
        DateTimeOffset createdAt,
        string email
    )
    {
        _start = start;
        _end = end;
        _since = since;
        _deletions = deletions;
        _key = key;
        _link = link;
        _organization = organization;
        _createdAt = createdAt;
        _email = email;
    }

    public bool PresentIn(Works works)
    {
        return works.ContainsOperation().Contains(_organization, _key, _start, _end);
    }

    private class Dto
    {
        public string Email { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string? Since { get; set; } = default;
        public string Repository { get; set; } = string.Empty;
        public string? Link { get; set; } = default;
        public IEnumerable<DeletionDto> Deletions { get; set; } = Array.Empty<DeletionDto>();
        public DateTimeOffset CreatedAt { get; set; } = default;

        internal class DeletionDto
        {
            public string Email { get; set; } = string.Empty;
            public uint Lines { get; set; } = default;
            public bool Accountable { get; set; } = default;
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(
            new Dto
            {
                Deletions = _deletions.Items().Select(
                    deletion => new Dto.DeletionDto
                    {
                        Lines = deletion.DeletedLines(),
                        Email = deletion.VictimEmail(),
                        Accountable = deletion.DeletionAccountable()
                    }
                ),
                End = _end,
                Email = _email,
                Repository = _key,
                Link = _link,
                Organization = _organization,
                Since = _since,
                Start = _start,
                CreatedAt = _createdAt
            }
        );
    }

    public Work RelatedWork(Works works)
    {
        return works.GetOperation().Work(_organization, _key, _start, _end);
    }

    public Work NewWork(Factories factories)
    {
        var work = factories.WorkFactory().NewWork(
            _organization,
            _key,
            _start,
            _end,
            _since,
            _email,
            _link,
            _createdAt
        );

        factories.RatingFactory().NewRatings(
            _organization,
            _key,
            _email,
            _deletions.Items(),
            work.Id(),
            _createdAt
        );

        return work;
    }
}
