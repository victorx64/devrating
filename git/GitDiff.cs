using System.Text.Json;
using devrating.entity;
using devrating.factory;
using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitDiff : Diff
{
    private readonly string _commit;
    private readonly string? _since;
    private readonly Deletions _deletions;
    private readonly string _key;
    private readonly string _organization;
    private readonly string? _link;
    private readonly string _email;
    private readonly DateTimeOffset _createdAt;

    public GitDiff(
        ILoggerFactory log,
        string @base,
        string commit,
        string? since,
        string repository,
        string key,
        string? link,
        string organization,
        DateTimeOffset createdAt
    )
        : this(
            commit,
            since,
            new TotalDeletions(
                new CachedPatches(
                    new GitPatches(
                        log,
                        @base,
                        commit,
                        since,
                        repository,
                        new GitDiffSizes(
                            log,
                            repository
                        )
                    )
                )
            ),
            key,
            link,
            organization,
            createdAt,
            new GitProcess(
                log,
                "git",
                $"show --no-patch --format=%aE {commit}",
                repository
            ).Output().First()
        )
    {
    }

    private GitDiff(
        string commit,
        string? since,
        Deletions deletions,
        string key,
        string? link,
        string organization,
        DateTimeOffset createdAt,
        string email
    )
    {
        _commit = commit;
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
        return works.ContainsOperation().Contains(_organization, _key, _commit);
    }

    public class Dto
    {
        public string Email { get; set; } = string.Empty;
        public string Commit { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string? Since { get; set; } = default;
        public string Repository { get; set; } = string.Empty;
        public string? Link { get; set; } = default;
        public IEnumerable<ContemporaryLinesDto> Deletions { get; set; } = Array.Empty<ContemporaryLinesDto>();
        public DateTimeOffset CreatedAt { get; set; } = default;

        public class ContemporaryLinesDto
        {
            public string VictimEmail { get; set; } = string.Empty;
            public double Weight { get; set; } = default;
            public uint Size { get; set; } = default;
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(
            new Dto
            {
                Deletions = _deletions.Items().Select(
                    deletion => new Dto.ContemporaryLinesDto
                    {
                        Weight = deletion.Weight(),
                        Size = deletion.Size(),
                        VictimEmail = deletion.VictimEmail(),
                    }
                ),
                Commit = _commit,
                Email = _email,
                Repository = _key,
                Link = _link,
                Organization = _organization,
                Since = _since,
                CreatedAt = _createdAt
            }
        );
    }

    public Work RelatedWork(Works works)
    {
        return works.GetOperation().Work(_organization, _key, _commit);
    }

    public Work NewWork(Factories factories)
    {
        var work = factories.WorkFactory().NewWork(
            _organization,
            _key,
            _commit,
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
