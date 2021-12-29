// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class VersionControlBlame : Blame
    {
        private readonly string _email;
        private readonly uint _count;
        private readonly uint _start;
        private readonly bool _accountable;
        private readonly uint _totalAdditions;

        public VersionControlBlame(string email, uint start, uint count, bool accountable, uint totalAdditions)
        {
            _email = email;
            _start = start;
            _count = count;
            _accountable = accountable;
            _totalAdditions = totalAdditions;
        }

        public bool ContainsLine(uint line)
        {
            return _start <= line && line < _start + _count;
        }

        public Deletion SubDeletion(uint from, uint to)
        {
            from = Math.Min(Math.Max(_start, from), _start + _count);
            to = Math.Max(Math.Min(_start + _count, to), _start);
            return new DefaultDeletion(_email, to - from, _totalAdditions, _accountable);
        }
    }
}