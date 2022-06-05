using Semver;

namespace devrating.git;

public sealed class GitTag : Tag
{
    private readonly SemVersion? _version;
    private readonly string? _sha;

    public GitTag(string? sha, string name)
        : this(
            sha,
            name.StartsWith("v", StringComparison.OrdinalIgnoreCase)
                ? name.Substring(1)
                : name,
            false
        )
    {
    }

    private GitTag(string? sha, string name, bool _)
        : this(
            sha,
            SemVersion.TryParse(name, out var semver)
                ? semver
                : null
        )
    {
    }

    public GitTag(string? sha, SemVersion? version)
    {
        _sha = sha;
        _version = version;
    }

    public string? Sha()
    {
        return _sha;
    }

    public SemVersion? Version()
    {
        return _version;
    }
}
