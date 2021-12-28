// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class ThinDiff : Diff
    {
        private readonly string _start;
        private readonly string _end;
        private readonly string _organization;
        private readonly string _repository;

        public ThinDiff(string organization, string repository, string start, string end)
        {
            _organization = organization;
            _repository = repository;
            _start = start;
            _end = end;
        }

        public void AddTo(EntityFactory factory)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset CreatedAt()
        {
            throw new NotImplementedException();
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_organization, _repository, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_organization, _repository, _start, _end);
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}