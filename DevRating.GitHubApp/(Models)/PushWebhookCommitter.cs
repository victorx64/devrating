using System.Globalization;

namespace DevRating.GitHubApp
{
    public class PushWebhookCommitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushWebhookCommitter"/> class.
        /// </summary>
        public PushWebhookCommitter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PushWebhookCommitter"/> class.
        /// </summary>
        /// <param name="nodeId">The GraphQL Node Id</param>
        /// <param name="name">The full name of the author or committer.</param>
        /// <param name="email">The email.</param>
        /// <param name="username">The committer username.</param>
        public PushWebhookCommitter(string name, string email, string username)
        {
            Name = name;
            Email = email;
            Username = username;
        }

        /// <summary>Gets the name of the author or committer.</summary>
        /// <value>The name.</value>
        public string Name { get; protected set; }

        /// <summary>Gets the email of the author or committer.</summary>
        /// <value>The email.</value>
        public string Email { get; protected set; }

        /// <summary>
        /// Gets the username of the author or committer.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; protected set; }

        internal string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "Name: {0} Email: {1} Username: {2}", Name, Email,
                    Username);
            }
        }
    }
}