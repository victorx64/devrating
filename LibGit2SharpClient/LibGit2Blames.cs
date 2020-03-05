// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Blames : Blames
    {
        private readonly BlameHunkCollection _collection;

        public LibGit2Blames(BlameHunkCollection collection)
        {
            _collection = collection;
        }

        public Blame HunkForLine(uint line)
        {
            return new LibGit2Blame(_collection.HunkForLine((int) line));
        }
    }
}