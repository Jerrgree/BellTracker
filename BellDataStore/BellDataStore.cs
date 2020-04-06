using Common;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data
{
    public class BellDataStore : IBellDataStore
    {
        private readonly BellContext _dbContext;

        public BellDataStore(BellContext context)
        {
            _dbContext = context;
        }

        public Week GetWeek(string year, int week)
        {
            try
            {
                return _dbContext
                        .Weeks
                        .Include(x => x.Prices)
                        .FirstOrDefault(x => x.Year == year && x.WeekNumber == week);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
