// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Semver;

namespace DevRating.VersionControl
{
    public sealed class LastMajorUpdateTag : Tag
    {
        private readonly Tag? _release;

        public LastMajorUpdateTag(IEnumerable<Tag> releases)
            : this(
                releases.Where(delegate(Tag r) { return r.Version() is object; })
                    .OrderByDescending(delegate(Tag r) { return r.Version(); })
                    .ToList()
            )
        {
        }

        private LastMajorUpdateTag(IList<Tag> releases)
            : this(
                releases.Any() && releases.First().Version()!.Major != releases.Last().Version()!.Major
                    ? releases.Last(delegate(Tag r) { return r.Version()!.Major == releases.First().Version()!.Major; })
                    : null
            )
        {
        }

        private LastMajorUpdateTag(Tag? release)
        {
            _release = release;
        }

        public string? Sha()
        {
            return _release?.Sha();
        }

        public SemVersion? Version()
        {
            return _release?.Version();
        }
    }
}