using DevRating.Rating;

namespace DevRating.VersionControlSystem
{
    public interface IVersionControlSystem
    {
        IRating Rating();
    }
}