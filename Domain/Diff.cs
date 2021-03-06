// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.Domain
{
    public interface Diff : JsonObject
    {
        Work From(Works works);
        bool PresentIn(Works works);
        void AddTo(EntityFactory factory);
        DateTimeOffset CreatedAt();
    }
}