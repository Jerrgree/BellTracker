using Common;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class BellDataStore : IBellDataStore
    {
        private readonly BellContext _dbContext;

        public BellDataStore(BellContext context)
        {
            _dbContext = context;
        }

        public async Task<Week> GetWeek(string year, int week)
        {
            return await _dbContext
                    .Weeks
                    .Include(x => x.Prices.OrderBy(p => p.DayOfWeek).ThenBy(p => p.IsMorning))
                    .FirstOrDefaultAsync(x => x.Year == year && x.WeekNumber == week);
        }

        public async Task<Week> GetOrCreateWeek(string year, int week)
        {
            var value = await GetWeek(year, week);

            if (value == null)
            {
                value = new Week()
                {
                    Year = year,
                    WeekNumber = week
                };
                await _dbContext.Weeks.AddAsync(value);
                await _dbContext.SaveChangesAsync();
            }

            return value;
        }

        public async Task<bool> AddPriceInstance(Price price)
        {
            try
            {
                await _dbContext.Prices.AddAsync(price);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) // better error handling in the future
            {
                return false;
            }
        }
    }
}