using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using gNowWP.ViewModels;

namespace Wp7ChartsSamples.Amcharts.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var items = new List<ChartItemViewModel>();

            sqliteDB cn = new sqliteDB();
            cn.open();

            string query = "SELECT idgravity, round(latitude,6) latitude, round(longitude,6) longitude, altitude, round(gravity,3) gravity FROM gravityData ORDER BY registered DESC";
            List<gravityInfo> placeInfo = cn.db.Query<gravityInfo>(query);
            for (int i = 0; i < placeInfo.Count - 1; i++)
                items.Add(new ChartItemViewModel { Value = placeInfo[i].gravity, Altitude = placeInfo[i].altitude });

            var chart = new ChartViewModel { Items = items };

            this.Charts = Enumerable.Repeat(0, 5).Select(_ => chart).ToList();

            cn.close();
        }

        public List<ChartViewModel> Charts { get; set; }
    }
}
