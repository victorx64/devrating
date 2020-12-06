// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        Rating PreviousRating();
        uint? CountedDeletions();
        uint? IgnoredDeletions();
        Work Work();
        Author Author();
        DateTimeOffset CreatedAt();
    }
}