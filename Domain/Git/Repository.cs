using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Domain.Git
{
    public interface Repository
    {
        Task WriteInto(ModificationsCollection modifications, string commit);
        IEnumerable<string> Commits(string a, string b);
    }
}