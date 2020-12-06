// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class JsonDiff : Diff
    {
        private class Dto
        {
            public string Email { get; set; } = string.Empty;
            public string Start { get; set; } = string.Empty;
            public string End { get; set; } = string.Empty;
            public string Organization { get; set; } = string.Empty;
            public string? Since { get; set; }
            public string Key { get; set; } = string.Empty;
            public string? Link { get; set; }
            public uint Additions { get; set; }
            public IEnumerable<DeletionDto> Deletions { get; set; } = new DeletionDto[0];

            internal class DeletionDto
            {
                public string Email { get; set; } = string.Empty;
                public uint Counted { get; set; }
                public uint Ignored { get; set; }
            }
        }

        private readonly Dto _state;

        public JsonDiff(string json) : this(JsonSerializer.Deserialize<Dto>(json)!)
        {
        }

        private JsonDiff(Dto state)
        {
            _state = state;
        }

        public void AddTo(EntityFactory factory, DateTimeOffset createdAt)
        {
            factory.InsertRatings(
                _state.Organization,
                _state.Email,
                _state.Deletions.Select(d => new DefaultDeletion(d.Email, d.Counted, d.Ignored)),
                factory.InsertedWork(
                    _state.Organization,
                    _state.Key,
                    _state.Start,
                    _state.End,
                    _state.Since,
                    _state.Email,
                    _state.Additions,
                    _state.Link,
                    createdAt
                )
                .Id(),
                createdAt
            );
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_state.Key, _state.Start, _state.End);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_state.Key, _state.Start, _state.End);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize<Dto>(_state);
        }
    }
}