using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface AuthorsCollection
    {
        Task<IDictionary<string, Player>>  Authors();
    }
}