// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.Domain
{
    public interface Id : IEquatable<Id>
    {
        object Value();
        bool Filled();
    }
}