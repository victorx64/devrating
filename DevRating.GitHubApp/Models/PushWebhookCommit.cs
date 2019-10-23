using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace DevRating.GitHubApp.Models
{
    /// <summary>
    /// Delete this class when https://github.com/octokit/octokit.net/pull/1844 is merged
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class PushWebhookCommit
    {
        public string Id { get; protected set; } = string.Empty;

        public string TreeId { get; protected set; } = string.Empty;

        public bool Distinct { get; protected set; } = default;

        public string Message { get; protected set; } = string.Empty;

        public DateTimeOffset Timestamp { get; protected set; } = default;

        public Uri Url { get; protected set; } = new Uri("http://devrating.net/");

        public PushWebhookCommitter Author { get; protected set; } = new PushWebhookCommitter();

        public PushWebhookCommitter Committer { get; protected set; } = new PushWebhookCommitter();

        public IReadOnlyList<string> Added { get; protected set; } = new string[] { };

        public IReadOnlyList<string> Removed { get; protected set; } = new string[] { };

        public IReadOnlyList<string> Modified { get; protected set; } = new string[] { };

        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Sha: {0}", Id); }
        }
    }
}