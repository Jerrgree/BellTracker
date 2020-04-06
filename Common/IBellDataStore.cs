using Domain;
using System.Threading.Tasks;

namespace Common
{
    public interface IBellDataStore
    {
        public Task<Week> GetWeek(string year, int week);

        public Task<Week> GetOrCreateWeek(string year, int week);

        public Task<bool> AddPriceInstance(Price price);
    }
}
