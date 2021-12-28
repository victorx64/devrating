// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlAdditionsTest
    {
        [Fact]
        public void CountsOneHunkAdditions()
        {
            Assert.Equal(2u, new VersionControlAdditions(@"
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
+"
            ).Count());
        }

        [Fact]
        public void SummarizesTwoHunkAdditions()
        {
            Assert.Equal(7u, new VersionControlAdditions(@"
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
+
@@ -268,0 +271,5 @@ Accessory.prototype.addBridgedAccessory = function(accessory, deferUpdate) {
+  // A bridge too far...
+  if (this.bridgedAccessories.length >= MAX_ACCESSORIES) {
+    throw new Error('Cannot Bridge more than 99 Accessories', MAX_ACCESSORIES);
+  }
+"
            ).Count());
        }

        [Fact]
        public void ParsesSingleLineAddition()
        {
            Assert.Equal(1u, new VersionControlAdditions(@"
commit f7baa1ac03b27ff719fcdfd5ae32ef6c6f377053
Author: Tian Zhang <hide@email.com>
Date:   Fri Apr 5 17:50:30 2019 -0700

    Bump max accessories limit

diff --git a/lib/Accessory.js b/lib/Accessory.js
index fa22e17..1a7c229 100644
--- a/lib/Accessory.js
+++ b/lib/Accessory.js
@@ -19 +19 @@ var bufferShim = require('buffer-shims');
-const MAX_ACCESSORIES = 99; // Maximum number of bridged accessories per bridge.
+const MAX_ACCESSORIES = 149; // Maximum number of bridged accessories per bridge."
            ).Count());
        }

        [Fact]
        public void ParsesZeroAdditions()
        {
            Assert.Equal(0u, new VersionControlAdditions(@"
commit 20faa6bd73e1147f18c666dfa3f822a6d5d93681
Author: Erik Baauw <hide@email.com>
Date:   Fri Apr 5 18:12:34 2019 +0200

    Update Accessory.js (#658)

    Throw an error when trying to add more than 99 bridged accessories to a bridge.

diff --git a/lib/Accessory.js b/lib/Accessory.js
index 5633fb0..fa22e17 100644
--- a/lib/Accessory.js
+++ b/lib/Accessory.js
@@ -18,2 +19,0 @@ var bufferShim = require('buffer-shims');
-const MAX_ACCESSORIES = 99; // Maximum number of bridged accessories per bridge.
-"
            ).Count());
        }

        [Fact]
        public void ThrowsExceptionOnContextualLine()
        {
            Assert.Throws<ContextLineEncounteredException>(
                delegate
                {
                    return new VersionControlAdditions(@"
commit f7baa1ac03b27ff719fcdfd5ae32ef6c6f377053
Author: Tian Zhang <hide@email.com>
Date:   Fri Apr 5 17:50:30 2019 -0700

    Bump max accessories limit

diff --git a/lib/Accessory.js b/lib/Accessory.js
index fa22e17..1a7c229 100644
--- a/lib/Accessory.js
+++ b/lib/Accessory.js
@@ -16,7 +16,7 @@ var IdentifierCache = require('./model/IdentifierCache').IdentifierCache;
 var bufferShim = require('buffer-shims');
 // var RelayServer = require('./util/relayserver').RelayServer;

-const MAX_ACCESSORIES = 99; // Maximum number of bridged accessories per bridge.
+const MAX_ACCESSORIES = 149; // Maximum number of bridged accessories per bridge.

 module.exports = {
   Accessory: Accessory"
                    ).Count();
                }
            );
        }
    }
}