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
            var additions = AdditionsMinusDeletions(_additions, _deletions);
            var deletions = NonSelfDeletions(DeletionsMinusAdditions(_deletions, _additions));

            storage.Insert(additions, deletions);
        }

        private IEnumerable<Addition> AdditionsMinusDeletions(
            IEnumerable<Addition> additions,
            IList<Deletion> deletions)
        {
            var result = new List<Addition>();

            foreach (var addition in additions)
            {
                var found = false;

                foreach (var deletion in deletions)
                {
                    if (addition.Commit().Equals(deletion.PreviousCommit()))
                    {
                        var x = addition.Count();
                        var y = deletion.Count();

                        if (x - y > 0)
                        {
                            result.Add(addition.NewAddition(x - y));
                        }

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    result.Add(addition);
                }
            }

            return result;
        }

        private IEnumerable<Deletion> DeletionsMinusAdditions(
            IEnumerable<Deletion> deletions,
            IList<Addition> additions)
        {
            var result = new List<Deletion>();

            foreach (var deletion in deletions)
            {
                var found = false;

                foreach (var addition in additions)
                {
                    if (addition.Commit().Equals(deletion.PreviousCommit()))
                    {
                        var x = addition.Count();
                        var y = deletion.Count();

                        if (y - x > 0)
                        {
                            result.Add(deletion.NewDeletion(y - x));
                        }

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    result.Add(deletion);
                }
            }

            return result;
        }

        private IEnumerable<Deletion> NonSelfDeletions(IEnumerable<Deletion> deletions)
        {
            foreach (var deletion in deletions)
            {
                if (!deletion.Commit().Author().Equals(deletion.PreviousCommit().Author()))
                {
                    yield return deletion;
                }
            }
        }
    }
}