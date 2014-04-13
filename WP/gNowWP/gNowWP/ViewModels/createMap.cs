using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;

namespace gNowWP.ViewModels
{
    class createMap
    {
        private Map mapLocation;
        private double Latitude;
        private double Longitude;
        private GeoCoordinateWatcher myCoordinateWatcher;

        public createMap(Map temp)
        {
            this.mapLocation = temp;
        }

        public void setCenter(double lat, double lon, double zoom)
        {
            this.Latitude = lat;
            this.Longitude = lon;

            this.mapLocation.Center = new GeoCoordinate(this.Latitude, this.Longitude);
            this.mapLocation.ZoomLevel = zoom;

            myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            myCoordinateWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(this.myCoordinateWatcher_PositionChanged);

        }

        private void myCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (!e.Position.Location.IsUnknown)
            {
                Latitude = e.Position.Location.Latitude;
                Longitude = e.Position.Location.Longitude;
                mapLocation.Center = new GeoCoordinate(Latitude, Longitude);
            }
        }

        public void addPushpins(List<Tuple<double, double>> Values)
        {
            foreach (Tuple<double, double> location in Values)
            {

                MapOverlay overlay = new MapOverlay();

                ImageBrush ib = new ImageBrush();

                ib.ImageSource =
                new BitmapImage(new Uri("/Assets/AppBar/marker.png", UriKind.Relative));

                // Create a map marker
                Rectangle rectangle = new Rectangle();

                rectangle.Fill = ib;

                rectangle.Height = 40;
                rectangle.Width = 40;

                overlay.Content = rectangle;

                overlay.GeoCoordinate = new GeoCoordinate(location.Item1, location.Item2);

                MapLayer layer = new MapLayer();
                layer.Add(overlay);

                mapLocation.Layers.Add(layer);

            }
        }
    }
}
