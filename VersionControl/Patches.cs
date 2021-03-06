// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.VersionControl
{
    public interface Patches
    {
        IEnumerable<FilePatch> Items();
    }
}