using System;
using DevRating.DefaultObject;
using DevRating.Domain;
using Semver;

namespace DevRating.VersionControl
{
    public sealed class VersionControlTag : Tag
    {
        private readonly SemVersion? _version;
        private readonly Envelope _sha;

        public VersionControlTag(string sha, string name)
            : this(
                sha,
                name.StartsWith("v", StringComparison.OrdinalIgnoreCase)
                    ? name.Substring(1)
                    : name,
                false
            )
        {
        }

        private VersionControlTag(string sha, string name, bool unused)
            : this(
                sha,
                SemVersion.TryParse(name, out var semver)
                    ? semver
                    : null
            )
        {
        }

        public VersionControlTag(string sha, SemVersion? version)
            : this(new DefaultEnvelope(sha), version)
        {
        }

        public VersionControlTag(Envelope sha, SemVersion? version)
        {
            _sha = sha;
            _version = version;
        }

        public Envelope Sha()
        {
            return _sha;
        }

        public bool HasVersion()
        {
            return _version != null;
        }

        public SemVersion Version()
        {
            return _version!;
        }
    }
}