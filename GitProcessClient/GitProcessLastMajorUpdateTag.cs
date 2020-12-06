// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using DevRating.VersionControl;
using Semver;

namespace DevRating.GitProcessClient
{
    public sealed class GitProcessLastMajorUpdateTag : Tag
    {
        private readonly Tag _release;

        public GitProcessLastMajorUpdateTag(string repository, string before)
            : this(
                new LastMajorUpdateTag(
                    new VersionControlProcess("git", $"tag -l --format='%(objectname) %(refname:short)' --merged {before}", repository)
                        .Output()
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(t => new VersionControlTag(t.Split(' ')[0], t.Split(' ')[1]))
                )
            )
        {
        }

        private GitProcessLastMajorUpdateTag(Tag release)
        {
            _release = release;
        }

        public string? Sha()
        {
            return _release.Sha();
        }

        public bool HasVersion()
        {
            return _release.HasVersion();
        }

        public SemVersion Version()
        {
            return _release.Version();
        }
    }
}