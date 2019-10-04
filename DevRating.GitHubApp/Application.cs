using System;
using System.IO;
using System.Threading.Tasks;
using DevRating.AzureTable;
using DevRating.LibGit2Sharp;
using DevRating.Rating;
using LibGit2Sharp;
using Octokit;

namespace DevRating.GitHubApp
{
    public sealed class Application
    {
        private readonly JsonWebToken _token;
        private readonly string _name;

        public Application(JsonWebToken token, string name)
        {
            _token = token;
            _name = name;
        }

        public async Task HandlePushEvent(PushWebhookPayload payload, string directory)
        {
            if (!payload.Ref.Equals($"refs/heads/{payload.Repository.DefaultBranch}"))
            {
                return;
            }

            var app = new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Octokit.Credentials(_token.Value(), AuthenticationType.Bearer)
            };

            var response = await app.GitHubApps.CreateInstallationToken(payload.Installation.Id);

            var installation = new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Octokit.Credentials(response.Token, AuthenticationType.Oauth)
            };

            var clone = payload.Repository.CloneUrl;

            var source = clone.Insert(clone.IndexOf("://", StringComparison.Ordinal) + "://".Length,
                $"x-access-token:{response.Token}@");

            var path = Path.Combine(directory, "repository");

            if (Directory.Exists(path))
            {
                DirectoryHelper.DeleteDirectory(path);
            }

            global::LibGit2Sharp.Repository.Clone(source, path,
                new CloneOptions
                {
                    IsBare = true,
                    BranchName = payload.Repository.DefaultBranch,
                    RecurseSubmodules = false
                });

            var repository = new LibGit2Repository(path, clone);

            var formula = new EloFormula();

            var modifications = new AzureModifications(
                Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"),
                "devrating",
                formula);

            foreach (var commit in payload.Commits)
            {
                string report;

                try
                {
                    modifications.Clear();

                    await repository.WriteInto(modifications, commit.Id);

                    await modifications.Upload();

                    report = modifications.Report();
                }
                catch (Exception e)
                {
                    report = e.ToString();
                }

                await installation.Repository.Comment.Create(payload.Repository.Id, commit.Id,
                    new NewCommitComment(report));
            }

            repository.Dispose();

            DirectoryHelper.DeleteDirectory(path);
        }
    }
}