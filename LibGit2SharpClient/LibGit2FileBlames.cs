// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2FileBlames : AFileBlames
    {
        private readonly BlameHunkCollection _collection;
        private readonly Commit _since;

        public LibGit2FileBlames(BlameHunkCollection collection, Commit since)
        {
            _collection = collection;
            _since = since;
        }

        public Blame AtLine(uint line)
        {
            var blame = _collection.HunkForLine((int)line);

            return blame.FinalCommit.Equals(_since)
                ? (Blame)new IgnoredBlame(blame.FinalCommit.Author.Email, (uint)blame.FinalStartLineNumber, (uint)blame.LineCount)
                : (Blame)new CountedBlame(blame.FinalCommit.Author.Email, (uint)blame.FinalStartLineNumber, (uint)blame.LineCount);
        }
    }
}