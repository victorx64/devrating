using GitHubJwt;

namespace DevRating.GitHubApp
{
    public sealed class JsonWebToken
    {
        private readonly int _id;
        private readonly string _path;

        public JsonWebToken(int id, string path)
        {
            _id = id;
            _path = path;
        }

        public string Value()
        {
            return new GitHubJwtFactory(
                    new FilePrivateKeySource(_path),
                    new GitHubJwtFactoryOptions {AppIntegrationId = _id, ExpirationSeconds = 570})
                .CreateEncodedJwtToken();
        }
    }
}