// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeBlame : Blame
    {
        private readonly string _email;
        private readonly uint _start;
        private readonly uint _count;

        public FakeBlame(string email, uint start, uint count)
        {
            _email = email;
            _start = start;
            _count = count;
        }

        public Deletion SubDeletion(uint from, uint limit)
        {
            return new DefaultDeletion(
                _email,
                Math.Min(_start + _count, limit) - from,
                0
            );
        }
    }
}