using DevRating.Domain;

namespace DevRating.Database
{
    public interface Works
    {
        DbWork Insert(
            string repository,
            string start,
            string end,
            DbObject author,
            uint additions,
            DbObject rating);

        DbWork Insert(
            string repository,
            string start,
            string end,
            DbObject author,
            uint additions);

        DbWork Work(Diff diff);
        bool Exist(Diff diff);
    }
}