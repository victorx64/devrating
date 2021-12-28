// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Semver;

namespace DevRating.VersionControl
{
    public interface Tag
    {
        string? Sha();
        SemVersion? Version();
    }
}