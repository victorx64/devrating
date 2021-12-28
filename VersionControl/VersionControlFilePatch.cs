// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.VersionControl
{
    public sealed class VersionControlFilePatch : FilePatch
    {
        private readonly Deletions _deletions;
        private readonly Additions _additions;

        public VersionControlFilePatch(string patch, AFileBlames blames)
            : this(new VersionControlDeletions(patch, blames), new VersionControlAdditions(patch))
        {
        }

        public VersionControlFilePatch(IEnumerable<string> patch, AFileBlames blames)
            : this(new VersionControlDeletions(patch, blames), new VersionControlAdditions(patch))
        {
        }

        public VersionControlFilePatch(Deletions deletions, Additions additions)
        {
            _deletions = deletions;
            _additions = additions;
        }

        public Deletions Deletions()
        {
            return _deletions;
        }

        public Additions Additions()
        {
            return _additions;
        }
    }
}