using Domain;

namespace Common
{
    public interface IBellDataStore
    {
        public Week GetWeek(string year, int week);
    }
}
