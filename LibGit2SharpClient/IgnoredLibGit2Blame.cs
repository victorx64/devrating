// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.DefaultObject;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class IgnoredLibGit2Blame : Blame
    {
        private readonly BlameHunk _hunk;

        public IgnoredLibGit2Blame(BlameHunk hunk)
        {
            _hunk = hunk;
        }

        public Deletion SubDeletion(uint from, uint to)
        {
            return new DefaultDeletion(
                _hunk.FinalCommit.Author.Email,
                0,
                Math.Min((uint) _hunk.FinalStartLineNumber + (uint) _hunk.LineCount, to) - from
            );
        }
    }
}