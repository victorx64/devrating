using System.Collections.Generic;

namespace DevRating.Vcs
{
    public sealed class DefaultModificationsCollection : ModificationsCollection
    {
        private readonly IList<Addition> _additions;
        private readonly IList<Deletion> _deletions;

        public DefaultModificationsCollection()
        {
            _additions = new List<Addition>();
            _deletions = new List<Deletion>();
        }

        public void Clear()
        {
            _additions.Clear();
            _deletions.Clear();
        }

        public void AddAddition(Addition addition)
        {
            _additions.Add(addition);
        }

        public void AddDeletion(Deletion deletion)
        {
            _deletions.Add(deletion);
        }

        public void PutTo(ModificationsStorage storage)
        {
            storage.Insert(NonDeletedAdditions(_additions, _deletions), NonSelfDeletions(_deletions));
        }

        private IEnumerable<Deletion> NonSelfDeletions(IList<Deletion> deletions)
        {
            foreach (var deletion in deletions)
            {
                if (!deletion.Commit().Author().Equals(deletion.PreviousCommit().Author()))
                {
                    yield return deletion;
                }
            }
        }

        private IEnumerable<Addition> NonDeletedAdditions(IList<Addition> additions, IList<Deletion> deletions)
        {
            foreach (var addition in additions)
            {
                yield return addition.NewAddition(
                    addition.Count() -
                    AddedThenDeletedLines(addition.Commit(), deletions));
            }
        }

        private uint AddedThenDeletedLines(Commit commit, IList<Deletion> deletions)
        {
            foreach (var deletion in deletions)
            {
                if (deletion.PreviousCommit().Equals(commit) &&
                    deletion.PreviousCommit().Author().Equals(deletion.Commit().Author()))
                {
                    return deletion.Count();
                }
            }

            return 0;
        }
    }
}