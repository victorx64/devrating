using System;
using Semver;

namespace DevRating.VersionControl
{
    public sealed class VersionControlTag : Tag
    {
        private readonly SemVersion? _version;
        private readonly string? _sha;

        public VersionControlTag(string? sha, string name)
            : this(
                sha,
                name.StartsWith("v", StringComparison.OrdinalIgnoreCase)
                    ? name.Substring(1)
                    : name,
                false
            )
        {
        }

        private VersionControlTag(string? sha, string name, bool unused)
            : this(
                sha,
                SemVersion.TryParse(name, out var semver)
                    ? semver
                    : null
            )
        {
        }

        public VersionControlTag(string? sha, SemVersion? version)
        {
            _sha = sha;
            _version = version;
        }

        public string? Sha()
        {
            return _sha;
        }

        public bool HasVersion()
        {
            return _version is object;
        }

        public SemVersion Version()
        {
            return _version!;
        }
    }
}