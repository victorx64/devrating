using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DevRating.Git.Test
{
    public class CommitTests
    {
        [Test]
        public async Task ParseSingleHunk()
        {
            var patch = @"
diff --git a/Xamarin.Forms.Platform.iOS/Renderers/PickerRenderer.cs b/Xamarin.Forms.Platform.iOS/Renderers/PickerRenderer.cs
index 4cd45fb5..073cfce7 100644
--- a/Xamarin.Forms.Platform.iOS/Renderers/PickerRenderer.cs
+++ b/Xamarin.Forms.Platform.iOS/Renderers/PickerRenderer.cs
@@ -84,3 +84,6 @@ namespace Xamarin.Forms.Platform.iOS
-
-                                       entry.InputAssistantItem.LeadingBarButtonGroups = null;
-                                       entry.InputAssistantItem.TrailingBarButtonGroups = null;
+
+                                       if (Forms.IsiOS9OrNewer)
+                                       {
+                                               entry.InputAssistantItem.LeadingBarButtonGroups = null;
+                                               entry.InputAssistantItem.TrailingBarButtonGroups = null;
+                                       }";

            var modifications = await FakeModifications(patch, "lineAuthor", "commitAuthor", "commitSha");

            Assert.AreEqual(6, modifications.Additions());
            Assert.AreEqual(3, modifications.Deletions().Count());
            Assert.IsTrue(modifications.Deletions().All(m => m.Equals("lineAuthor")));
        }
        
        [Test]
        public async Task ParseMultipleHunks()
        {
            var patch = @"
diff --git a/Xamarin.Forms.ControlGallery.iOS/Xamarin.Forms.ControlGallery.iOS.csproj b/Xamarin.Forms.ControlGallery.iOS/Xamarin.Forms.ControlGallery.iOS.csproj
index 90984f7a..23825bac 100644
--- a/Xamarin.Forms.ControlGallery.iOS/Xamarin.Forms.ControlGallery.iOS.csproj
+++ b/Xamarin.Forms.ControlGallery.iOS/Xamarin.Forms.ControlGallery.iOS.csproj
@@ -352 +352 @@
-    <PackageReference Include=''Xamarin.TestCloud.Agent'' Version=''0.21.7'' />
+    <PackageReference Include=''Xamarin.TestCloud.Agent'' Version=''0.21.8'' />
diff --git a/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/Bugzilla35736.cs b/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/Bugzilla35736.cs
index 1581e8c5..190286dc 100644
--- a/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/Bugzilla35736.cs
+++ b/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/Bugzilla35736.cs
@@ -48 +48 @@ namespace Xamarin.Forms.Controls.Issues
-               [Ignore]
+               [Ignore(''Fails sometimes'')]
diff --git a/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/TestPages/ScreenshotConditionalApp.cs b/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/TestPages/ScreenshotConditionalApp.cs
index 6e458d2b..b3344fb0 100644
--- a/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/TestPages/ScreenshotConditionalApp.cs
+++ b/Xamarin.Forms.Controls.Issues/Xamarin.Forms.Controls.Issues.Shared/TestPages/ScreenshotConditionalApp.cs
@@ -239,3 +239 @@ namespace Xamarin.Forms.Controls
-#pragma warning disable 618
-                       _app.SwipeRight();
-#pragma warning restore 618
+                       SwipeLeftToRight();
@@ -256,3 +254 @@ namespace Xamarin.Forms.Controls
-#pragma warning disable 618
-                       _app.SwipeLeft();
-#pragma warning restore 618
+                       SwipeRightToLeft();";

            var modifications = await FakeModifications(patch, "lineAuthor", "commitAuthor", "commitSha");

            Assert.AreEqual(4, modifications.Additions());
            Assert.AreEqual(8, modifications.Deletions().Count());
            Assert.IsTrue(modifications.Deletions().All(m => m.Equals("lineAuthor")));
        }

        private async Task<FakeModifications> FakeModifications(string patch, string line, string commit, string sha)
        {
            return (FakeModifications) await new Commit(
                    new FakeRepository(
                        new List<Watchdog>
                        {
                            new FilePatch(patch,
                                new FakeBlame(line))
                        }, commit), sha)
                .Modifications(new FakeModificationsFactory());
        }
    }
}