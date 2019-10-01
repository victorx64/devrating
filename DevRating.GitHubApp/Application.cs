using System;
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

        public async Task HandlePushEvent(PushWebhookPayload payload)
        {
            /*
             * git clone bare repo
             * foreach payload.commit
             * try
                * analyze each commit
                * post report to comments 
             * catch
                * post error message
                * invite to retry or create issue ticket
             */

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

            var path = global::LibGit2Sharp.Repository.Clone(source, "repo.git", new CloneOptions
            {
                IsBare = true,
                BranchName = payload.Repository.DefaultBranch,
                RecurseSubmodules = false
            });

            var repository = new LibGit2Repository(path, clone);
            
            var formula = new EloFormula();

            foreach (var commit in payload.Commits)
            {
                var modifications = new AzureModifications("", "DevRating", "DevRating", formula);

                await repository.WriteInto(modifications, commit.Id);

                await modifications.Sync();

//                await installation.Repository.Comment.Create(payload.Repository.Id, commit.Id,
//                    new NewCommitComment(matches.Report(commit.Id)));
            }
        }
    }
}