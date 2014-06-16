using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using gNowWP.Resources;
using gNowWP.ViewModels;
using System.Xml;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Newtonsoft.Json;
using Microsoft.Phone.Reactive;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Tasks;
using System.Net.NetworkInformation;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;
using System.Windows.Input;
using System.Windows.Documents;

namespace gNowWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<UserInfo> data;
        gravity G = new gravity();

        public class resultLocation
        {
            public string place_id { get; set; }
            public string licence { get; set; }
            public string osm_type { get; set; }
            public string osm_id { get; set; }
            public List<string> boundingbox { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
            public string @class { get; set; }
            public string type { get; set; }
            public double importance { get; set; }
            public string icon { get; set; }
        }

        public class resultData
        {
            public int alt { get; set; }
            public double lng { get; set; }
            public double lat { get; set; }
        }

        private void clean()
        {
            bar.Visibility = Visibility.Visible;
            lblGravity.FontSize = 36;
            lblGravity.Text = "";
            lblUnitsG.Text = "";
            locationData.Visibility = Visibility.Collapsed;
        }

        public async void GetSinglePositionAsync()
        {
            clean();

            Windows.Devices.Geolocation.Geolocator geolocator = null;
            Windows.Devices.Geolocation.Geoposition geoposition = null;

            try
            {
                geolocator = new Windows.Devices.Geolocation.Geolocator();

                geolocator.DesiredAccuracy = PositionAccuracy.High;

                geoposition = await geolocator.GetGeopositionAsync();

                double latitude = geoposition.Coordinate.Latitude;
                double longitude = geoposition.Coordinate.Longitude;
                int altitude = (int)geoposition.Coordinate.Altitude;
                double gravityR = 0;

                lblAltitude.Text = altitude.ToString();
                lblLatitude.Text = latitude.ToString();
                lblLongitude.Text = longitude.ToString();

                lblAltitudeS.Text = AppResources.lblAltitude + ":\n" + altitude.ToString();
                lblLatitudeS.Text = AppResources.lblLatitude + ":\n" + latitude.ToString().Substring(0, 11);
                lblLongitudeS.Text = AppResources.lblLongitude + ":\n" + longitude.ToString().Substring(0, 11);

                if (data[0].Metres == true)
                {
                    gravityR = Double.Parse(getGResult(latitude, Convert.ToDouble(altitude), true));
                    lblUnitsG.Text = getUResult(true);
                }
                else
                {
                    gravityR = Double.Parse(getGResult(latitude, Convert.ToDouble(altitude), false));
                    lblUnitsG.Text = getUResult(false);
                }

                locationData.Visibility = Visibility.Visible;

                lblGravityTotal.Text = gravityR.ToString();

                gravityR = Math.Round(gravityR, data[0].Ndecimal);

                lblGravity.Text = gravityR.ToString();

                lblGravity.FontSize = 120;

                if (data[0].Autosync == true)
                {
                    syncData();
                }
                else
                    setGravity("false");

            }
            catch (Exception ex)
            {
                // Something else happened while acquiring the location.
                Console.Write(ex.ToString());
                lblGravity.Text = AppResources.errData;
                var result = MessageBox.Show(AppResources.errGPS, "Error", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                    lunchLocation();
            }
            bar.Visibility = Visibility.Collapsed;

        }

        private async void lunchLocation()
        {
            var op = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            if (getUserData() == false)
            {
                createProfile();
                getUserData();
            }
        }

        private bool getUserData()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("user.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<UserInfo>));
                        data = (List<UserInfo>)serializer.Deserialize(stream);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void setGravity(string cStatus)
        {
            sqliteDB cn = new sqliteDB();
            double gravityR = double.Parse(lblGravityTotal.Text);

            if (lblGravity.Text == " ft/s²")
                gravityR = G.changeToMetres(gravityR);

            cn.open();

            int exist = existGravity();

            if (exist == 0)
            {
                using (cn.db)
                {
                    cn.db.RunInTransaction(() =>
                    {
                        cn.db.Insert(new gravityData() { altitude = int.Parse(lblAltitude.Text), gravity = gravityR, latitude = Double.Parse(lblLatitude.Text), longitude = Double.Parse(lblLongitude.Text), status = cStatus, registered = DateTime.Now });
                    });
                }
            }

            if (cStatus == "true" && exist > 0)
            {
                string query = "SELECT * FROM gravityData WHERE latitude=" + lblLatitude.Text + " AND longitude=" + lblLongitude.Text + " AND altitude=" + lblAltitude.Text;

                var existing = cn.db.Query<gravityData>(query).FirstOrDefault();
                if (existing != null)
                {
                    existing.status = cStatus;

                    cn.db.RunInTransaction(() =>
                    {
                        cn.db.Update(existing);
                    });
                }
            }
            cn.close();
        }

        private int existGravity()
        {
            sqliteDB cn = new sqliteDB();
            cn.open();

            string query = "SELECT COUNT(idgravity) FROM gravityData WHERE latitude=" + lblLatitude.Text + " AND longitude=" + lblLongitude.Text + " AND altitude=" + lblAltitude.Text;
            List<resultG> total = cn.db.Query<resultG>(query);

            cn.close();
            return total[0].total;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GetSinglePositionAsync();
        }

        public string getGResult(double latitude, double altitude, bool metres)
        {
            lblResult.FontSize = 36;

            gravity G = new gravity(latitude, 0, altitude);
            double result = -1;

            if (metres == false)
                altitude = G.changeToMetres(altitude);

            result = G.getGravity();

            if (result == 0)
                return AppResources.errLatitude;
            else if (result == 1)
                return AppResources.errAltitudeAbove;
            else if (result == 2)
                return AppResources.errAltitudeBelow;
            else
            {
                lblResult.FontSize = 48;

                if (metres == false)
                    result = G.changeToFeet(result);
                return result.ToString();
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }

        public string getUResult(bool metres)
        {
            if (metres == false)
                return " ft/s²";
            else
                return " m/s²";
        }

        private void createProfile()
        {
            sqliteDB cn = new sqliteDB();
            cn.createDB();
            cn.close();

            //Initialize the session here
            // Write to the Isolated Storage
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("user.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<UserInfo>));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        //set values from User
                        serializer.Serialize(xmlWriter, GeneratePersonData());
                    }
                }
            }
        }

        private List<UserInfo> GeneratePersonData()
        {
            List<UserInfo> data = new List<UserInfo>();
            UserInfo ui = new UserInfo();

            //asign basic values
            ui.Iduser = "";
            ui.Metres = true;
            ui.Autosync = false;
            ui.Ndecimal = 3;
            ui.Cdecimal = 8;
            data.Add(ui);
            return data;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //set step
            createAppBar(((Pivot)sender).SelectedIndex);

            if (((Pivot)sender).SelectedIndex == 2 && String.IsNullOrEmpty(lblGravity1.Text))
            {
                gravityPlaces dataR = new gravityPlaces();

                imgPlace1.Source = new BitmapImage(new Uri("/Assets/Planets/" + dataR.getName(3).ToString().ToLower() + ".jpg", UriKind.Relative));
                lblDescription1.Text = dataR.getDescription(3);
                if (!((String.Equals(lblGravity.Text, AppResources.errData)) || String.IsNullOrEmpty(lblGravity.Text)))
                {
                    if (lblUnitsG.Text == " ft/s²")
                        lblGravity1.Text = Math.Round(G.changeToMetres(Convert.ToDouble(lblGravity.Text)), data[0].Ndecimal).ToString();
                    else
                        lblGravity1.Text = lblGravity.Text;
                }
                else
                    lblGravity1.Text = dataR.getGravity(3).ToString();
            }
        }

        private void createAppBar(int step)
        {
            ApplicationBar = new ApplicationBar();

            if (step == 0)
            {

                ApplicationBarIconButton button1 = new ApplicationBarIconButton();
                ApplicationBarIconButton button2 = new ApplicationBarIconButton();
                ApplicationBarIconButton button3 = new ApplicationBarIconButton();
                ApplicationBarIconButton button4 = new ApplicationBarIconButton();

                button1.IconUri = new Uri("/Assets/AppBar/interchange.png", UriKind.Relative);
                button1.Text = AppResources.btnChangeU;
                ApplicationBar.Buttons.Add(button1);
                button1.Click += new EventHandler(changeUButton_Click);

                button3.IconUri = new Uri("/Assets/AppBar/share.png", UriKind.Relative);
                button3.Text = AppResources.btnShare;
                ApplicationBar.Buttons.Add(button3);
                button3.Click += new EventHandler(shareButton_Click);

                button4.IconUri = new Uri("/Assets/AppBar/refresh.png", UriKind.Relative);
                button4.Text = AppResources.btnRefresh;
                ApplicationBar.Buttons.Add(button4);
                button4.Click += new EventHandler(refreshButton_Click);

                if (data[0].Autosync == false)
                {
                    button2.IconUri = new Uri("/Assets/AppBar/sync.png", UriKind.Relative);
                    button2.Text = AppResources.btnSync;
                    ApplicationBar.Buttons.Add(button2);
                    button2.Click += new EventHandler(syncButton_Click);
                }
            }
            else
                ApplicationBar.Mode = ApplicationBarMode.Minimized;

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = AppResources.menuProfile;
            ApplicationBar.MenuItems.Add(menuItem1);
            menuItem1.Click += new EventHandler(profileButton_Click);

            ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem();
            menuItem2.Text = AppResources.menuHelp;
            ApplicationBar.MenuItems.Add(menuItem2);
            menuItem2.Click += new EventHandler(helpButton_Click);

            ApplicationBarMenuItem menuItem3 = new ApplicationBarMenuItem();
            menuItem3.Text = AppResources.menuAbout;
            ApplicationBar.MenuItems.Add(menuItem3);
            menuItem3.Click += new EventHandler(aboutusButton_Click);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            GetSinglePositionAsync();
        }

        private void shareButton_Click(object sender, EventArgs e)
        {
            if ((String.Equals(lblGravity.Text, AppResources.errData)) || String.IsNullOrEmpty(lblGravity.Text))
                return;

            string gravity = lblGravity.Text;

            if (data[0].Metres == true)
                gravity += " m/s²";
            else
                gravity += " ft/s²";

            NavigationService.Navigate(new Uri("/pages/shareOptions.xaml?gravity=" + gravity, UriKind.Relative));
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/pages/help.xaml", UriKind.Relative));
        }

        private void aboutusButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/pages/aboutus.xaml", UriKind.Relative));
        }

        private void profileButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/pages/profile.xaml", UriKind.Relative));
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            syncData();
        }

        private async void syncData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                double gValue = 0;

                if ((String.Equals(lblGravity.Text, AppResources.errData)) || String.IsNullOrEmpty(lblGravity.Text))
                {
                    MessageBox.Show(AppResources.errGPS2, "Error", MessageBoxButton.OK);
                    return;
                }

                if (lblUnitsG.Text == " ft/s²")
                    gValue = G.changeToMetres(Convert.ToDouble(lblGravityTotal.Text));
                else
                    gValue = Convert.ToDouble(lblGravityTotal.Text);

                string URL = "http://gnow.hostingsiteforfree.com/webServices/setLocation.php?uid=" + data[0].Iduser + "&altitude=" + lblAltitude.Text + "&longitude=" + lblLongitude.Text + "&latitude=" + lblLatitude.Text + "&gravity=" + gValue.ToString();

                if (!String.IsNullOrEmpty(data[0].Iduser))
                {
                    cleanString cs = new cleanString();

                    WebClient w = new WebClient();

                    Observable
                    .FromEvent<DownloadStringCompletedEventArgs>(w, "DownloadStringCompleted")
                    .Subscribe(r =>
                    {
                        var deserialized = JsonConvert.DeserializeObject<List<result>>(cs.clean(r.EventArgs.Result));

                        switch (deserialized[0].total)
                        {
                            case "0":
                                if (data[0].Autosync == false)
                                    setGravity("true");
                                MessageBox.Show(AppResources.errReg, "Error", MessageBoxButton.OK);
                                break;
                            case "2":
                                MessageBox.Show(AppResources.errTry, "Error", MessageBoxButton.OK);
                                break;
                            default:
                                setGravity("true");
                                MessageBox.Show(AppResources.success, AppResources.titleThanks, MessageBoxButton.OK);
                                break;
                        }
                    });
                    w.DownloadStringAsync(
                    new Uri(URL));
                }
                else
                    MessageBox.Show(AppResources.errLogin, "Error", MessageBoxButton.OK);
            }
            else
                MessageBox.Show(AppResources.errInternet, "Error", MessageBoxButton.OK);

        }

        private void changeUButton_Click(object sender, EventArgs e)
        {
            if ((String.Equals(lblGravity.Text, AppResources.errData)) || String.IsNullOrEmpty(lblGravity.Text))
                return;

            //double altitude;
            double gravity;
            string units;
            int pivotS = pvtOptions.SelectedIndex;
            double altitude = Convert.ToDouble(lblAltitude.Text);

            if (pivotS == 0)
            {

                if (lblUnitsG.Text == " m/s²")
                {
                    gravity = Math.Round(G.changeToFeet(Convert.ToDouble(lblGravity.Text)), data[0].Ndecimal);
                    units = getUResult(false);

                    altitude = Math.Round(G.changeToFeet(altitude), 0);
                }
                else
                {
                    gravity = Math.Round(G.changeToMetres(Convert.ToDouble(lblGravity.Text)), data[0].Ndecimal);
                    units = getUResult(true);

                    altitude = Math.Round(G.changeToMetres(altitude), 0);
                }

                lblAltitudeS.Text = AppResources.lblAltitude + ":\n" + altitude.ToString();
                lblUnitsG.Text = units;
                lblGravity.Text = gravity.ToString();
            }
        }

        private void cmbPlaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (pvtOptions == null)
                return;

            if (cmbUnitsComp.SelectedIndex == -1)
                return;

            gravityPlaces data = new gravityPlaces();

            ListPicker cmbValue = (ListPicker)sender;

            double gValue = 0;

            if (!((String.Equals(lblGravity.Text, AppResources.errData)) || String.IsNullOrEmpty(lblGravity.Text)) && cmbValue.SelectedIndex == 3)
                gValue = Convert.ToDouble(lblGravity.Text);
            else
                gValue = data.getGravity(cmbValue.SelectedIndex);

            if (cmbUnitsComp.SelectedIndex == 1)
                gValue = G.changeToFeet(gValue);

            switch (cmbValue.Name)
            {
                case "cmbPlaces1":
                    imgPlace1.Source = new BitmapImage(new Uri("/Assets/Planets/" + data.getName(cmbValue.SelectedIndex).ToString().ToLower() + ".jpg", UriKind.Relative));
                    lblDescription1.Text = data.getDescription(cmbValue.SelectedIndex);
                    lblGravity1.Text = gValue.ToString();
                    break;
                case "cmbPlaces2":
                    imgPlace2.Source = new BitmapImage(new Uri("/Assets/Planets/" + data.getName(cmbValue.SelectedIndex).ToString().ToLower() + ".jpg", UriKind.Relative));
                    lblDescription2.Text = data.getDescription(cmbValue.SelectedIndex);
                    lblGravity2.Text = gValue.ToString();
                    break;
            }

            if (cmbPlaces1.SelectedIndex > -1 && cmbPlaces2.SelectedIndex > -1)
            {

                int gCompared = data.comparedGravity(cmbPlaces1.SelectedIndex, cmbPlaces2.SelectedIndex);
                double gPercentage = 0;

                gPercentage = data.percentageGravity(cmbPlaces1.SelectedIndex, cmbPlaces2.SelectedIndex);

                switch (gCompared)
                {
                    case 0:
                        lblCompared1.Foreground = new SolidColorBrush(Colors.Green);
                        lblCompared1.Text = AppResources.gravBigger + Math.Round(gPercentage, 3).ToString() + "%";
                        break;
                    case 1:
                        lblCompared1.Foreground = new SolidColorBrush(Colors.Red);
                        lblCompared1.Text = AppResources.gravSmaller + Math.Round(gPercentage, 3).ToString() + "%";
                        break;
                    case 2:
                        lblCompared1.Foreground = new SolidColorBrush(Colors.Blue);
                        lblCompared1.Text = AppResources.gravSame;
                        break;
                }
            }
        }

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculate();
        }

        private void calculate()
        {
            if (!(String.IsNullOrEmpty(txtLatitude.Text) || String.IsNullOrEmpty(txtAltitude.Text)) && cmbUnits.SelectedIndex > -1)
            {
                lblResult.Visibility = Visibility.Visible;
                lblUnitsC.Visibility = Visibility.Visible;

                double gravityR = 0;
                string tempGravity = "";

                if (cmbUnits.SelectedIndex == 0)
                {
                    tempGravity = getGResult(Convert.ToDouble(txtLatitude.Text), Convert.ToDouble(txtAltitude.Text), true);
                    lblUnitsC.Text = getUResult(true);
                }
                else
                {
                    tempGravity = getGResult(Convert.ToDouble(txtLatitude.Text), G.changeToMetres(Convert.ToDouble(txtAltitude.Text)), false);
                    lblUnitsC.Text = getUResult(false);
                }

                string pattern = "^[-+]?[0-9]*\\.?[0-9]*$";

                if (Regex.IsMatch(tempGravity, pattern) == false)
                {
                    lblUnitsC.Visibility = Visibility.Collapsed;
                    lblUnitsC.Text = "";
                }
                else
                {
                    gravityR = Math.Round(Convert.ToDouble(tempGravity), data[0].Cdecimal);
                    tempGravity = gravityR.ToString();
                }

                lblResult.Text = tempGravity;
            }
        }

        private void cmbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculate();
        }

        private void cmbUnitsComp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pvtOptions == null)
                return;

            if (cmbUnitsComp.SelectedIndex == 0)
            {
                lblGravity1.Text = G.changeToMetres(Convert.ToDouble(lblGravity1.Text)).ToString();

                if (!String.IsNullOrEmpty(lblGravity2.Text))
                    lblGravity2.Text = G.changeToMetres(Convert.ToDouble(lblGravity2.Text)).ToString();
            }
            else
            {
                lblGravity1.Text = G.changeToFeet(Convert.ToDouble(lblGravity1.Text)).ToString();

                if (!String.IsNullOrEmpty(lblGravity2.Text))
                    lblGravity2.Text = G.changeToFeet(Convert.ToDouble(lblGravity2.Text)).ToString();
            }

        }

        private void txtLocation_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (e.Key == Key.Enter)
                {
                    string address = txtLocation.Text;

                    if (String.IsNullOrEmpty(address))
                        return;

                    rtxtResult.Blocks.Clear();

                    WebClient w = new WebClient();

                    Observable
                    .FromEvent<DownloadStringCompletedEventArgs>(w, "DownloadStringCompleted")
                    .Subscribe(r =>
                    {
                        try
                        {
                            var deserialized = JsonConvert.DeserializeObject<List<resultLocation>>(r.EventArgs.Result);

                            for (int i = 0; i < deserialized.Count; i++)
                                setDataLocation(deserialized[i]);
                        }
                        catch
                        {
                            MessageBox.Show(AppResources.lblTryAgain, "Error", MessageBoxButton.OK);
                        }
                    });
                    w.DownloadStringAsync(
                    new Uri("http://nominatim.openstreetmap.org/search?q=" + address + "&format=json"));
                    searchPivot.Focus();

                }
            }
            else
                MessageBox.Show(AppResources.errInternet, "Error", MessageBoxButton.OK);
        }

        private void setDataLocation(resultLocation data)
        {
            WebClient w = new WebClient();

            Observable
            .FromEvent<DownloadStringCompletedEventArgs>(w, "DownloadStringCompleted")
            .Subscribe(r =>
            {
                var deserialized = JsonConvert.DeserializeObject<resultData>(r.EventArgs.Result);

                gravity G = new gravity(deserialized.lat, 0, deserialized.alt);

                Paragraph newP = new Paragraph();

                Hyperlink link = new Hyperlink();
                link.Inlines.Add(data.display_name);
                link.NavigateUri = new Uri("http://bing.com/maps/?cp=" + data.lat + "~" + data.lon + "&lvl=16&sp=point." + data.lat + "_" + data.lon + "_");
                link.TargetName = "_blank";
                link.Foreground = (Brush)Application.Current.Resources["PhoneAccentBrush"];

                newP.Inlines.Add(AppResources.lblLocation + ": ");
                newP.Inlines.Add(link);
                newP.Inlines.Add("\n");
                newP.Inlines.Add(AppResources.lblLatitude + ": " + data.lat + "°");
                newP.Inlines.Add("\n");
                newP.Inlines.Add(AppResources.lblLongitude + ": " + data.lon + "°");
                newP.Inlines.Add("\n");
                newP.Inlines.Add(AppResources.lblGravity2 + " " + Math.Round(G.getGravity(), 6).ToString() + " m/s²");
                newP.Inlines.Add("\n\r");
                rtxtResult.Blocks.Add(newP);

            });
            w.DownloadStringAsync(
            new Uri("http://api.geonames.org/srtm3JSON?lat=" + data.lat + "&lng=" + data.lon + "&username=fanmixco"));
        }

    }
}