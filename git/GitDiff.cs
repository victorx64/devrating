using System.Text.Json;
using devrating.entity;
using devrating.factory;
using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitDiff : Diff
{
    private readonly string _base;
    private readonly string _commit;
    private readonly string? _since;
    private readonly Deletions _deletions;
    private readonly string _key;
    private readonly string _organization;
    private readonly string? _link;
    private readonly string _email;
    private readonly DateTimeOffset _createdAt;
    private readonly IEnumerable<string> _paths;
    private readonly ILoggerFactory _log;
    private readonly string _repository;

    public GitDiff(
        ILoggerFactory log,
        string @base,
        string commit,
        string? since,
        string repository,
        string key,
        string? link,
        string organization,
        DateTimeOffset createdAt,
        IEnumerable<string> paths
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
                            repository,
                            paths
                        ),
                        paths
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
                new[]
                {
                    "show",
                    "--no-patch",
                    "--format=%aE",
                    commit,
                },
                repository
            ).Output().First(),
            paths,
            @base,
            log,
            repository
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
        string email,
        IEnumerable<string> paths,
        string @base,
        ILoggerFactory log,
        string repository
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
        _paths = paths;
        _base = @base;
        _log = log;
        _repository = repository;
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
        public IEnumerable<string> Paths { get; set; } = Array.Empty<string>();
        public uint Additions { get; set; } = default;

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
                CreatedAt = _createdAt,
                Paths = _paths,
                Additions = Additions(),
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
            _createdAt,
            _paths
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

    public uint Additions()
    {
        if (_commit.StartsWith('^'))
        {
            var log = _log.CreateLogger(this.GetType());

            log.LogError(new EventId(1735322), $"We've encountered boundary commit `{_commit}`. " +
                "Try to clone the repository with deeper history " +
                "(do `git clone` with higher `--depth` argument).");

            throw new InvalidOperationException($"Encountered boundary commit {_commit}");
        }

        var args = new List<string>
        {
            "diff",
            $"{_base}..{_commit}",
            "-U0",
            Config.GitDiffWhitespace,
            Config.GitDiffRenames,
            "--shortstat",
            "--",
        };

        args.AddRange(_paths);

        var output = new GitProcess(
            _log,
            "git",
            args,
            _repository
        ).Output();

        if (!output.Any() || !output[0].Any())
        {
            return 0;
        }

        var stat = output[0];

        var start = stat.IndexOf("changed, ") + "changed, ".Length;
        var end = stat.IndexOf(" insert");

        if (end == -1) {
            return 0;
        }

        return uint.Parse(stat.Substring(start, end - start));
    }
}
