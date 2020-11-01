// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class CountedBlame : Blame
    {
        private readonly string _email;
        private readonly uint _count;
        private readonly uint _start;

        public CountedBlame(string email, uint start, uint count)
        {
            _email = email;
            _start = start;
            _count = count;
        }

        public bool ContainsLine(uint line)
        {
            return _start <= line && line < _start + _count;
        }

        public Deletion SubDeletion(uint from, uint to)
        {
            from = Math.Min(Math.Max(_start, from), _start + _count);
            to = Math.Max(Math.Min(_start + _count, to), _start);
            return new DefaultDeletion(_email, to - from, 0);
        }
    }
}