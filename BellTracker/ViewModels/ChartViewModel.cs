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

        protected LineDataset<Point> data { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            // Initialize this or ChartJS blow up
            LineConfig = new LineConfig();
            var currentDate = new ParsedDate(DateTime.Now);
            CurrentWeek = await BellDataStore.GetOrCreateWeek(currentDate.Year, currentDate.Week);
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
                        Text = "Stonk"
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
                                    LabelString = "Cost"
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
                                    LabelString = "Time"
                                },
                                Ticks = new CategoryTicks()
                                {
                                    Labels = AxisLabels,
                                    Min = "Sun",
                                    Max = "Sat PM"
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

            data = new LineDataset<Point>()
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

            data.AddRange(CurrentWeek.Prices.Select(x => new Point()
            {
                X = GetPlotPoint(x),
                Y = x.Amount
            }));

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