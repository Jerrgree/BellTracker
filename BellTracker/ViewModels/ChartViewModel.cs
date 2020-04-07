using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes.Ticks;
using ChartJs.Blazor.ChartJS.Common.Enums;
using ChartJs.Blazor.ChartJS.Common.Handlers;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.Common.Wrappers;
using ChartJs.Blazor.ChartJS.LineChart;
using ChartJs.Blazor.Charts;
using ChartJs.Blazor.Util;
using Common;
using Common.Models;
using Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BellTracker.ViewModels
{
    public class ChartViewModel : ComponentBase
    {
        [Inject]
        public IBellDataStore BellDataStore { get; set; }

        protected Week CurrentWeek { get; set; }

        protected LineConfig LineConfig { get; set; }

        protected ChartJsLineChart Chart { get; set; }

        protected LineDataset<Int32Wrapper> data { get; set; }

        private List<string> AxisLabels => new List<string>()
        {
            "Sun",
            "Mon AM",
            "Mon PM",
            "Tue AM",
            "Tue PM",
            "Wed AM",
            "Wed PM",
            "Thu AM",
            "Thu PM",
            "Fri AM",
            "Fri PM",
            "Sat AM",
            "Sat PM",
        };

        #region Page State

        public DayOfWeek InputDayOfWeek { get; set; }

        public int InputAmount { get; set; }

        public bool InputIsMorning { get; set; }

        public bool CanInput =>
            CurrentWeek != null && (CurrentWeek.Prices.Any(x => x.DayOfWeek == InputDayOfWeek && x.IsMorning == InputIsMorning));

        #endregion Page State

        protected override async Task OnInitializedAsync()
        {
            // Initialize this or ChartJS blow up
            LineConfig = new LineConfig();
            var currentDate = new ParsedDate(DateTime.Now);
            CurrentWeek = await BellDataStore.GetOrCreateWeek(currentDate.Year, currentDate.Week);

            InputDayOfWeek = currentDate.DayOfWeek;
            InputAmount = 0;
            InputIsMorning = currentDate.IsMorning;

            SetupChart();

            await base.OnInitializedAsync();
        }

        protected void SetupChart()
        {
            LineConfig = new LineConfig()
            {
                Options = new LineOptions()
                {
                    Responsive = true,
                    Title = new OptionsTitle()
                    {
                        Display = true,
                        Text = "Stonk",
                        FontSize = 20
                    },
                    Tooltips = new Tooltips()
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = false
                    },
                    Scales = new Scales()
                    {
                        yAxes = new List<CartesianAxis>()
                        {
                            new LinearCartesianAxis()
                            {
                                ScaleLabel = new ScaleLabel()
                                {
                                    LabelString = "Cost",
                                    FontSize = 20
                                },
                                Ticks = new LinearCartesianTicks()
                                {
                                       SuggestedMax  = 200,
                                       Min = 0,
                                }
                            }
                        },
                        xAxes = new List<CartesianAxis>()
                        {
                            new CategoryAxis()
                            {
                                ScaleLabel = new ScaleLabel()
                                {
                                    LabelString = "Time",
                                    FontSize = 20
                                },
                                Ticks = new CategoryTicks()
                                {
                                    Min = "Sun",
                                    Max = "Sat PM",
                                }
                            }
                        }
                    },
                    Hover = new LineOptionsHover()
                    {
                        Intersect = true,
                        Mode = InteractionMode.Y
                    },
                }
            };

            LineConfig.Data.Labels = AxisLabels;

            data = new LineDataset<Int32Wrapper>()
            {
                BackgroundColor = ColorUtil.ColorString(0, 255, 0, 1.0),
                BorderColor = ColorUtil.ColorString(0, 0, 255, 1.0),
                Label = "Price",
                Fill = false,
                PointBackgroundColor = ColorUtil.RandomColorString(),
                BorderWidth = 1,
                PointRadius = 3,
                PointBorderWidth = 1,
                SteppedLine = SteppedLine.False,
            };

            data.AddRange(CurrentWeek.Prices.Select(x => x.Amount).Wrap());

            LineConfig.Data.Datasets.Add(data);

            StateHasChanged();
        }

        protected double GetPlotPoint(Price price)
        {
            double value = (int)price.DayOfWeek;

            if (!price.IsMorning)
            {
                value += 0.5d;
            }

            return value;
        }

        protected async Task UpdateChart()
        {
            await Chart.Update();
        }
    }
}