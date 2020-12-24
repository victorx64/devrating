// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class ThinDiff : Diff
    {
        private readonly string _start;
        private readonly string _end;
        private readonly string _repository;

        public ThinDiff(string start, string end, string repository)
        {
            _start = start;
            _end = end;
            _repository = repository;
        }

        public void AddTo(EntityFactory factory, DateTimeOffset createdAt)
        {
            throw new NotImplementedException();
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_repository, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_repository, _start, _end);
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}