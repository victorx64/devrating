using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Git
{
    public sealed class Commit
    {
        private readonly Repository _repository;
        private readonly string _sha;

        public Commit(Repository repository, string sha)
        {
            _repository = repository;
            _sha = sha;
        }

        public async Task<Modifications> Modifications(ModificationsFactory factory)
        {
            var modifications = factory.Modifications(_sha, _repository.Author(_sha));

            var tasks = new List<Task>();

            foreach (var patch in _repository.FilePatches(_sha))
            {
                tasks.Add(patch.WriteInto(modifications));
            }

            await Task.WhenAll(tasks);

            return modifications;
        }
    }
}