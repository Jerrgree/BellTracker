using Domain;
using System.Threading.Tasks;

namespace Common
{
    public interface IBellDataStore
    {
        Task<Week> GetWeek(string year, int week);

        Task<Week> GetOrCreateWeek(string year, int week);

        Task<bool> AddPriceInstance(Price price);
    }
}