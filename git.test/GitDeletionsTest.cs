using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public sealed class GitDeletionsTest
{
    [Fact]
    public void ParsesZeroDeletion()
    {
        Assert.Empty(
            new GitDeletions(@"
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
                new FakeFileBlames(
                    new[]
                    {
                            new GitBlame("Victim", 0u, 100u, true, new FakeDiffSizes(1000u), string.Empty),
                    }
                )
            ).Items());
    }

    [Fact]
    public void ParsesSingleDeletion()
    {
        Assert.Single(
            new GitDeletions(@"
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
                new FakeFileBlames(
                    new[]
                    {
                            new GitBlame("Victim", 0u, 100u, true, new FakeDiffSizes(1000u), string.Empty),
                    }
                )
            ).Items());
    }

    [Fact]
    public void CreatesTwoDeletionsOnTwoDifferentVictims()
    {
        Assert.Equal(2,
            new GitDeletions(@"
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
                    new FakeFileBlames(
                        new[]
                        {
                                new GitBlame("Victim1", 2u, 1u, true, new FakeDiffSizes(1000u), string.Empty),
                                new GitBlame("Victim2", 4u, 1u, true, new FakeDiffSizes(1000u), string.Empty),
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
            new GitDeletions(@"
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
                    new FakeFileBlames(
                        new[]
                        {
                                new GitBlame("Victim1", 2u, 3u, true, new FakeDiffSizes(1000u), string.Empty),
                        }
                    )
                )
                .Items()
                .Count()
        );
    }
}
