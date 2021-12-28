// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.Domain
{
    public interface Work : Entity
    {
        uint Additions();
        Author Author();
        Rating UsedRating();
        string Start();
        string End();
        string? Link();
        string? Since();
        DateTimeOffset CreatedAt();
    }
}