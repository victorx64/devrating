// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;

namespace DevRating.Domain
{
    public interface DbInstance
    {
        void Create();
        bool Present();
        IDbConnection Connection();
    }
}