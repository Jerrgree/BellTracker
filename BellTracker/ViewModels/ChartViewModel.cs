using Common;
using Common.Models;
using Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BellTracker.ViewModels
{
    public class ChartViewModel : ComponentBase
    {
        [Inject]
        public IBellDataStore BellDataStore { get; set; }

        protected Week CurrentWeek { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var currentDate = new ParsedDate(DateTime.Now);
            CurrentWeek = await BellDataStore.GetOrCreateWeek(currentDate.Year, currentDate.Week);
            await base.OnInitializedAsync();
        }
    }
}