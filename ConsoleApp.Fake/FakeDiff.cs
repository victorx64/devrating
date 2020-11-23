// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.ConsoleApp.Fake
{
    public sealed class FakeDiff : Diff
    {
        private readonly string _key;
        private readonly string _start;
        private readonly string _end;
        private readonly Envelope _since;
        private readonly Envelope _link;
        private readonly string _email;
        private readonly string _organization;
        private readonly uint _additions;
        private readonly IEnumerable<Deletion> _deletions;
        private readonly DateTimeOffset _createdAt;

        public FakeDiff(Envelope link)
            : this(
                "key",
                "start",
                "end",
                new DefaultEnvelope("since this commit"),
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
            : this (
                "key",
                "start",
                "end",
                new DefaultEnvelope("since this commit"),
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
            Envelope since,
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
                new DefaultEnvelope()
            )
        {
        }

        public FakeDiff(
            string key,
            string start,
            string end,
            Envelope since,
            string email,
            string organization,
            uint additions,
            IEnumerable<Deletion> deletions,
            DateTimeOffset createdAt,
            Envelope link
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
            return works.GetOperation().Work(_key, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start, _end);
        }

        public void AddTo(EntityFactory factory, DateTimeOffset createdAt)
        {
            factory.InsertRatings(
                _organization,
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
    }
}