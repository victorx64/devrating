using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface Repository
    {
        Task<Modifications> Modifications(ModificationsFactory factory, string sha);
    }
}