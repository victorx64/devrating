using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevRating.Git.Test
{
    public class FakeModifications : Modifications
    {
        public void AddDeletion(string victim)
        {
            throw new NotImplementedException();
        }

        public void AddAdditions(int count)
        {
            throw new NotImplementedException();
        }
    }
}