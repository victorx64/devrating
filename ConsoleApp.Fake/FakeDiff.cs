// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.ConsoleApp.Fake
{
    public sealed class FakeDiff : Diff
    {
        private readonly string _key;
        private readonly string _start;
        private readonly string _end;
        private readonly string? _since;
        private readonly string? _link;
        private readonly string _email;
        private readonly string _organization;
        private readonly uint _additions;
        private readonly IEnumerable<Deletion> _deletions;
        private readonly DateTimeOffset _createdAt;

        public FakeDiff(string? link)
            : this(
                "key",
                "start",
                "end",
                "since this commit",
                "author",
                "org",
                10u,
                new[]
                {
                    new DefaultDeletion("victim1", 7u),
                    new DefaultDeletion("victim2", 14u),
                },
                DateTimeOffset.UtcNow,
                link
            )
        {
        }

        public FakeDiff()
            : this(
                "key",
                "start",
                "end",
                "since this commit",
                "author",
                "org",
                10u,
                new[]
                {
                    new DefaultDeletion("victim1", 7u),
                    new DefaultDeletion("victim2", 14u),
                },
                DateTimeOffset.UtcNow
            )
        {
        }

        public FakeDiff(
            string key,
            string start,
            string end,
            string? since,
            string email,
            string organization,
            uint additions,
            IEnumerable<Deletion> deletions,
            DateTimeOffset createdAt)
            : this(
                key,
                start,
                end,
                since,
                email,
                organization,
                additions,
                deletions,
                createdAt,
                null
            )
        {
        }

        public FakeDiff(
            string key,
            string start,
            string end,
            string? since,
            string email,
            string organization,
            uint additions,
            IEnumerable<Deletion> deletions,
            DateTimeOffset createdAt,
            string? link
        )
        {
            _key = key;
            _start = start;
            _end = end;
            _since = since;
            _email = email;
            _organization = organization;
            _additions = additions;
            _deletions = deletions;
            _createdAt = createdAt;
            _link = link;
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_organization, _key, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_organization, _key, _start, _end);
        }

        public void AddTo(EntityFactory factory)
        {
            factory.InsertRatings(
                _organization,
                _key,
                _email,
                _deletions,
                factory.InsertedWork(
                    _organization,
                    _key,
                    _start,
                    _end,
                    _since,
                    _email,
                    _additions,
                    _link,
                    _createdAt
                )
                .Id(),
                _createdAt
            );
        }

        private class Dto
        {
            public string Email { get; set; } = string.Empty;
            public string Start { get; set; } = string.Empty;
            public string End { get; set; } = string.Empty;
            public string Organization { get; set; } = string.Empty;
            public string? Since { get; set; } = default;
            public string Key { get; set; } = string.Empty;
            public string? Link { get; set; } = default;
            public uint Additions { get; set; } = default;
            public IEnumerable<DeletionDto> Deletions { get; set; } = Array.Empty<DeletionDto>();
            public DateTimeOffset CreatedAt { get; set; } = default;

            internal class DeletionDto
            {
                public string Email { get; set; } = string.Empty;
                public uint Counted { get; set; } = default;
                public uint Ignored { get; set; } = default;
            }
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize<Dto>(
                new Dto
                {
                    Additions = _additions,
                    Deletions = _deletions.Select(deletion =>
                        new Dto.DeletionDto
                        {
                            Counted = deletion.Counted(),
                            Email = deletion.Email(),
                            Ignored = deletion.Ignored()
                        }
                    ),
                    End = _end,
                    Email = _email,
                    Key = _key,
                    Link = _link,
                    Organization = _organization,
                    Since = _since,
                    Start = _start,
                    CreatedAt = _createdAt
                }
            );
        }

        public DateTimeOffset CreatedAt()
        {
            return _createdAt;
        }
    }
}