using System;
using System.Collections.Generic;
using DevRating.Vcs;

namespace DevRating.SqlClient
{
    public sealed class InMemoryModifications : Modifications
    {
        private readonly IList<Addition> _additions;
        private readonly IList<Deletion> _deletions;

        public InMemoryModifications()
        {
            _additions = new List<Addition>();
            _deletions = new List<Deletion>();
        }

        public void AddAddition(Addition addition)
        {
            _additions.Add(addition);
        }

        public void AddDeletion(Deletion deletion)
        {
            _deletions.Add(deletion);
        }

        public IList<Addition> Additions()
        {
            return AdditionsMinusDeletions(_additions, _deletions);
        }

        public IList<Deletion> Deletions()
        {
//            return NonSelfDeletions(DeletionsMinusAdditions(_deletions, _additions));
            return DeletionsMinusAdditions(_deletions, _additions);
        }

        private IList<Addition> AdditionsMinusDeletions(
            IList<Addition> additions,
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
                            result.Add(addition.UpdatedAddition(x - y));
                        }
                        else if (x - y < 0)
                        {
                            throw new Exception();
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

        private IList<Deletion> DeletionsMinusAdditions(
            IList<Deletion> deletions,
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
                            result.Add(deletion.UpdatedDeletion(y - x));
                        }
                        else if (y - x < 0)
                        {
                            throw new Exception();
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
                if (!deletion.Commit().Author()
                    .Equals(deletion.PreviousCommit().Author()))
                {
                    yield return deletion;
                }
            }
        }
    }
}