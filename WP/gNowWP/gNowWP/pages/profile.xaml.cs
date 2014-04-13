using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Live.Controls;
using Microsoft.Live;
using System.Windows.Media.Imaging;
using gNowWP.ViewModels;
using Microsoft.Phone.Reactive;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using gNowWP.Resources;

namespace gNowWP.pages
{
    public partial class settings : PhoneApplicationPage
    {
        private class ItemData
        {
            public string axis { get; set; }
            public double value { get; set; }
        }

        private List<UserInfo> infoUser;

        private LiveAuthClient auth;

        private void getUser()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("user.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<UserInfo>));
                    infoUser = (List<UserInfo>)serializer.Deserialize(stream);
                    stream.Close();
                }
            }
        }

        public settings()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            login(false);
            getUser();

            btnSync.IsChecked = infoUser[0].Autosync;

            if (infoUser[0].Metres == true)
                cmbUnits.SelectedIndex = 0;
            else
                cmbUnits.SelectedIndex = 1;

            sldGravity.Value = infoUser[0].Ndecimal;

            sldCalculator.Value = infoUser[0].Cdecimal;

        }

        private List<UserInfo> GeneratePersonData(string uid)
        {
            List<UserInfo> data = new List<UserInfo>();
            UserInfo ui = new UserInfo();

            //asign basic values
            ui.Iduser = uid;

            if (cmbUnits.SelectedIndex == 0)
                ui.Metres = true;
            else
                ui.Metres = false;

            ui.Autosync = (bool)btnSync.IsChecked;

            ui.Ndecimal = (int)sldGravity.Value;

            ui.Cdecimal = (int)sldCalculator.Value;

            data.Add(ui);
            return data;
        }

        private async void login(bool previous)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                lblErr.Text = "";
                try
                {
                    auth = new LiveAuthClient("000000004011A732");
                    LiveLoginResult result = await auth.InitializeAsync(new string[] { "wl.basic", "wl.offline_access", "wl.signin", "wl.emails" });

                    if (previous == true)
                        if (result.Status != LiveConnectSessionStatus.Connected)
                            result = await auth.LoginAsync(new string[] { "wl.basic", "wl.offline_access", "wl.signin", "wl.emails" });

                    if (result.Status == LiveConnectSessionStatus.Connected)
                    {
                        var connect = new LiveConnectClient(auth.Session);
                        var opResult = await connect.GetAsync("me");
                        dynamic nameResult = opResult.Result;
                        var email = nameResult.emails;

                        this.infoText.Text = nameResult.name;
                        this.emailText.Text = email.preferred;

                        var photoResult = await connect.GetAsync("me/picture");
                        dynamic photoResultdyn = photoResult.Result;
                        var image = new BitmapImage(new Uri(photoResultdyn.location, UriKind.Absolute));
                        this.profileImage.Source = image;

                        stpInfo.Visibility = Visibility.Visible;
                        btnSignIn.Content = AppResources.btnSignOut;

                    }
                    else
                        this.lblErr.Text = AppResources.errNoSignIn;

                }
                catch (LiveAuthException exc)
                {
                    this.lblErr.Text = "Error: " + exc.Message;
                }
                catch (LiveConnectException connExc)
                {
                    this.lblErr.Text = "Error making request: " + connExc.Message;
                }
                finally
                {
                    bar.Visibility = Visibility.Collapsed;
                    btnSignIn.IsEnabled = true;
                }
            }
            else
                MessageBox.Show(AppResources.errInternet, "Error", MessageBoxButton.OK);
        }

        private void updateInfo()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
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
                            Console.Write(AppResources.errReg);
                            break;
                        case "2":
                            MessageBox.Show(AppResources.errTry, "Error", MessageBoxButton.OK);
                            break;
                        default:
                            editStorage(deserialized[0].total);
                            break;
                    }
                });
                w.DownloadStringAsync(
                new Uri("http://gnow.hostingsiteforfree.com/webServices/register.php?fullname=" + infoText.Text + "&email=" + emailText.Text));
            }
            else
                MessageBox.Show(AppResources.errLogin, "Error", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btnSignIn.Content.ToString() == AppResources.btnSignIn)
            {
                login(true);

                updateInfo();

            }
            else
                logout();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?r=" + Guid.NewGuid().ToString(), UriKind.Relative));
        }

        private void logout()
        {
            try
            {
                this.auth.Logout();
                stpInfo.Visibility = Visibility.Collapsed;
                profileImage.Source = null;
                btnSignIn.Content = AppResources.btnSignIn;
                editStorage("");
            }
            catch
            {
                lblErr.Text = AppResources.errTry;
            }
        }

        private void editStorage(string uid)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //Open existing file
                if (myIsolatedStorage.FileExists("user.xml"))
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("user.xml", FileMode.Create))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<UserInfo>));
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            //set values from User
                            serializer.Serialize(xmlWriter, GeneratePersonData(uid));
                            //getUser();
                        }
                    }
                }
            }
        }

        private void cmbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(pvtOptions == null))
                editStorage(infoUser[0].Iduser);
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            if (!(pvtOptions == null))
                editStorage(infoUser[0].Iduser);
        }

        private void sldGravity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!(pvtOptions == null))
            {
                sldGravity.Value = Math.Round(e.NewValue);
                editStorage(infoUser[0].Iduser);
            }
        }

        private void sldCalculator_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!(pvtOptions == null))
            {
                sldCalculator.Value = Math.Round(e.NewValue);
                editStorage(infoUser[0].Iduser);
            }
        }

        private void myLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(pvtOptions == null))
            {
                LongListSelector item = (LongListSelector)sender;
                if (item.SelectedItem != null)
                {
                    gravityInfo p = item.SelectedItem as gravityInfo;

                    string url = "/pages/map.xaml?id=" + p.idgravity.ToString();
                    NavigationService.Navigate(new Uri(url, UriKind.Relative));
                }

            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            sqliteDB cn = new sqliteDB();
            cn.open();

            string query = "SELECT idgravity, round(latitude,6) latitude, round(longitude,6) longitude, altitude, round(gravity,6) gravity FROM gravityData ORDER BY registered DESC";
            List<gravityInfo> placeInfo = cn.db.Query<gravityInfo>(query);
            myLocations.ItemsSource = placeInfo;

            cn.close();
        }

        private void pvtOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            createAppBar(((Pivot)sender).SelectedIndex);
        }

        private void createAppBar(int p)
        {
            ApplicationBar = new ApplicationBar();

            if (p == 2)
            {

                ApplicationBarIconButton button2 = new ApplicationBarIconButton();

                button2.IconUri = new Uri("/Assets/AppBar/sync.png", UriKind.Relative);
                button2.Text = AppResources.btnSync;
                ApplicationBar.Buttons.Add(button2);
                button2.Click += new EventHandler(syncButton_Click);
            }
            else
                ApplicationBar.IsVisible = false;
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            getLocation();
        }

        private void getLocation()
        {
            sqliteDB cn = new sqliteDB();
            cn.open();

            string query = "SELECT idgravity, latitude, longitude, altitude, gravity, status FROM gravityData WHERE status='false' LIMIT 1";
            List<gravityInfo> placeInfo = cn.db.Query<gravityInfo>(query);

            if (placeInfo.Count > 0)
            {
                setGravity("true", placeInfo[0].latitude, placeInfo[0].longitude, placeInfo[0].altitude);
                setLocation(placeInfo[0].latitude, placeInfo[0].longitude, placeInfo[0].altitude, placeInfo[0].gravity);
            }
            else if (placeInfo.Count == 0)
                MessageBox.Show(AppResources.successFull, AppResources.titleThanks, MessageBoxButton.OK);
        }

        private void setLocation(double latitude, double longitude, double altitude, double gravity)
        {
            if (!String.IsNullOrEmpty(infoUser[0].Iduser))
            {

                string URL = "http://gnow.hostingsiteforfree.com/webServices/setLocation.php?uid=" + infoUser[0].Iduser + "&altitude=" + altitude.ToString() + "&longitude=" + longitude.ToString() + "&latitude=" + latitude.ToString() + "&gravity=" + gravity.ToString();

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
                            Console.Write(AppResources.errReg, "Error", MessageBoxButton.OK);
                            break;
                        case "2":
                            MessageBox.Show(AppResources.errTry, "Error", MessageBoxButton.OK);
                            break;
                        default:
                            getLocation();
                            break;
                    }
                });
                w.DownloadStringAsync(
                new Uri(URL));
            }
            else
                MessageBox.Show(AppResources.errLogin, "Error", MessageBoxButton.OK);
        }

        private void setGravity(string cStatus, double latitude, double longitude, double altitude)
        {
            gravity G = new gravity();
            sqliteDB cn = new sqliteDB();
            cn.open();

            string query = "SELECT idgravity, latitude, longitude, altitude, gravity FROM gravityData WHERE latitude=" + latitude.ToString() + " AND longitude=" + longitude.ToString() + " AND altitude=" + altitude.ToString();

            var existing = cn.db.Query<gravityData>(query).FirstOrDefault();
            if (existing != null)
            {
                existing.status = cStatus;

                cn.db.RunInTransaction(() =>
                {
                    cn.db.Update(existing);
                });
            }
            cn.close();
        }
    }
}