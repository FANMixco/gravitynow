using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using gNowWP.ViewModels;
using Microsoft.Phone.Maps.Toolkit;
using System.Device.Location;
using gNowWP.Resources;

namespace gNowWP.pages
{
    public partial class map : PhoneApplicationPage
    {
        public map()
        {
            InitializeComponent();
        }

        private void createAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            ApplicationBarIconButton button2 = new ApplicationBarIconButton();

            button1.IconUri = new Uri("/Assets/AppBar/road.png", UriKind.Relative);
            button1.Text = AppResources.btnRoad;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(road_Click);

            button2.IconUri = new Uri("/Assets/AppBar/eye.png", UriKind.Relative);
            button2.Text = AppResources.btnAerial;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(aerial_Click);

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            createAppBar();

            createMap cm = new createMap(gravityLocation);

            sqliteDB cn = new sqliteDB();
            cn.open();

            string query = "SELECT idgravity, latitude, longitude, altitude, gravity FROM gravityData WHERE idgravity=" + this.NavigationContext.QueryString["id"];
            
            List<gravityInfo> placeInfo = cn.db.Query<gravityInfo>(query);

            var values = placeInfo[0];

            lblAltitude.Text = "Altitude:\n" + values.altitude.ToString();
            lblLatitude.Text = "Latitude:\n" + Math.Round(values.latitude, 6).ToString();
            lblLongitude.Text = "Longitude:\n" + Math.Round(values.longitude, 6).ToString();
            lblGravity.Text = "Gravity:\n" + Math.Round(values.gravity, 6).ToString();

            cm.setCenter(values.latitude, values.longitude, 13);

            List<Tuple<double, double>> locations = new List<Tuple<double, double>>();
            locations.Add(new Tuple<double, double>(values.latitude, values.longitude));

            cm.addPushpins(locations);

            cn.close();

        }

        private void road_Click(object sender, EventArgs e)
        {
           gravityLocation.CartographicMode = MapCartographicMode.Road;
        }

        private void aerial_Click(object sender, EventArgs e)
        {
            gravityLocation.CartographicMode = MapCartographicMode.Aerial;
        }
    }
}