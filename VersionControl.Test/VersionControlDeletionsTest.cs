// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlDeletionsTest
    {
        [Fact]
        public void ParsesZeroDeletion()
        {
            Assert.Empty(
                new VersionControlDeletions(@"
commit 20faa6bd73e1147f18c666dfa3f822a6d5d93681
Author: Erik Baauw <hide@email.com>
Date:   Fri Apr 5 18:12:34 2019 +0200

    Update Accessory.js (#658)

    Throw an error when trying to add more than 99 bridged accessories to a bridge.

diff --git a/lib/Accessory.js b/lib/Accessory.js
index 5633fb0..fa22e17 100644
--- a/lib/Accessory.js
+++ b/lib/Accessory.js
@@ -18,0 +19,2 @@ var bufferShim = require('buffer-shims');
+const MAX_ACCESSORIES = 99; // Maximum number of bridged accessories per bridge.
+",
                    new FakeBlames(
                        new[]
                        {
                            new FakeBlame("Victim", 0, 100),
                        }
                    )
                ).Items());
        }

        [Fact]
        public void ParsesSingleDeletion()
        {
            Assert.Single(
                new VersionControlDeletions(@"
commit 20faa6bd73e1147f18c666dfa3f822a6d5d93681
Author: Erik Baauw <hide@email.com>
Date:   Fri Apr 5 18:12:34 2019 +0200

    Update Accessory.js (#658)

    Throw an error when trying to add more than 99 bridged accessories to a bridge.

diff --git a/lib/Accessory.js b/lib/Accessory.js
index 5633fb0..fa22e17 100644
--- a/lib/Accessory.js
+++ b/lib/Accessory.js
@@ -19 +19 @@ var bufferShim = require('buffer-shims');
-const MAX_ACCESSORIES = 99; // Maximum number of bridged accessories per bridge.
+const MAX_ACCESSORIES = 149; // Maximum number of bridged accessories per bridge.",
                    new FakeBlames(
                        new[]
                        {
                            new FakeBlame("Victim", 0, 100),
                        }
                    )
                ).Items());
        }

        [Fact]
        public void CreatesTwoDeletionsOnTwoDifferentVictims()
        {
            Assert.Equal(2,
                new VersionControlDeletions(@"
commit 1f6c2728e1ad51e2711abba6779a72b522f84b34 (tag: v0.4.39)
Author: Tian Zhang <khaos.tian@gmail.com>
Date:   Wed Jan 10 21:15:25 2018 -0800

    0.4.39

diff --git a/package-lock.json b/package-lock.json
index 59bc854..c709613 100644
--- a/package-lock.json
+++ b/package-lock.json
@@ -3 +3 @@
-  'version': '0.4.38',
+  'version': '0.4.39',
@@ -5 +5 @@
-  'version': '0.4.38',
+  'version': '0.4.39',",
                        new FakeBlames(
                            new[]
                            {
                                new FakeBlame("Victim1", 2, 1),
                                new FakeBlame("Victim2", 4, 1),
                            }
                        )
                    )
                    .Items()
                    .Count()
            );
        }

        [Fact]
        public void CreatesTwoDeletionsOnSingleVictim()
        {
            Assert.Equal(2,
                new VersionControlDeletions(@"
commit 1f6c2728e1ad51e2711abba6779a72b522f84b34 (tag: v0.4.39)
Author: Tian Zhang <khaos.tian@gmail.com>
Date:   Wed Jan 10 21:15:25 2018 -0800

    0.4.39

diff --git a/package-lock.json b/package-lock.json
index 59bc854..c709613 100644
--- a/package-lock.json
+++ b/package-lock.json
@@ -3 +3 @@
-  'version': '0.4.38',
+  'version': '0.4.39',
@@ -5 +5 @@
-  'version': '0.4.38',
+  'version': '0.4.39',",
                        new FakeBlames(
                            new[]
                            {
                                new FakeBlame("Victim1", 2, 3),
                            }
                        )
                    )
                    .Items()
                    .Count()
            );
        }
    }
}