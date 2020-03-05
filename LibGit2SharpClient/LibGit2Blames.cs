// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Blames : Blames
    {
        private readonly BlameHunkCollection _collection;
        private readonly Commit _since;

        public LibGit2Blames(BlameHunkCollection collection, Commit since)
        {
            _collection = collection;
            _since = since;
        }

        public Blame HunkForLine(uint line)
        {
            var hunk = _collection.HunkForLine((int) line);

            return hunk.FinalCommit.Equals(_since)
                ? (Blame) new IgnoredLibGit2Blame(hunk)
                : (Blame) new CountedLibGit2Blame(hunk);
        }
    }
}