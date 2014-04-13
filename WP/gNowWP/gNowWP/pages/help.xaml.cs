using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Windows.Media;
using ImageTools.IO.Gif;
using Microsoft.Phone.Tasks;
using System.Windows.Documents;
using gNowWP.Resources; 

namespace gNowWP.pages
{
    public partial class help : PhoneApplicationPage
    {
        public help()
        {
            InitializeComponent();

            Sample smple = new Sample();
            smple.ImageSource = new Uri("/Assets/Images/GRACE_globe_animation.gif", UriKind.Relative);
            this.DataContext = smple;
            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();

        }

        public class Sample
        {

            private Uri _imageSource;

            public Uri ImageSource
            {
                get { return _imageSource; }
                set
                {
                    if (_imageSource != value)
                    {
                        _imageSource = value;
                    }
                }
            }
        }

        private void HyperlinkButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://en.wikipedia.org/wiki/International_Gravity_Formula", UriKind.Absolute);
            wbt.Show();
        }

        private void HyperlinkButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://en.wikipedia.org/wiki/Gravity_of_Earth#Free_air_correction", UriKind.Absolute);
            wbt.Show();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Paragraph newP = new Paragraph();
            Paragraph newP2 = new Paragraph();
            Paragraph newP3 = new Paragraph();
            Paragraph newP4 = new Paragraph();
            Paragraph newP5 = new Paragraph();
            Paragraph newP6 = new Paragraph();

            newP.Inlines.Add(AppResources.lblGravityDesc);

            rtxtDescGravity.Blocks.Add(newP);

            newP2.Inlines.Add(AppResources.lblGravityCDesc);

            rtxtGravityCDesc.Blocks.Add(newP2);

            newP3.TextAlignment = TextAlignment.Justify;
            newP3.Inlines.Add(AppResources.lblFormula1);
            newP3.Inlines.Add("\n\r");

            newP4.TextAlignment = TextAlignment.Center;
            newP4.Inlines.Add("IGF = 9.780327 (1 + 0.0053024sin²Φ - 0.0000058sin²2Φ)");
            newP4.Inlines.Add("\n\r");
            newP4.Inlines.Add("FAC = -3.086 x 10⁻⁶ x h");
            newP4.Inlines.Add("\n\r");
            newP4.Inlines.Add("g = IGF + FAC");
            newP4.Inlines.Add("\n\r");

            newP6.TextAlignment = TextAlignment.Left;
            newP6.Inlines.Add(AppResources.lblFormula2);
            newP6.Inlines.Add("\n");
            newP6.Inlines.Add(AppResources.lblFormula3);
            newP6.Inlines.Add("\n");
            newP6.Inlines.Add(AppResources.lblFormula4);
            newP6.Inlines.Add("\n");
            newP6.Inlines.Add(AppResources.lblFormula5);
            newP6.Inlines.Add("\n");
            newP6.Inlines.Add(AppResources.lblFormula6);
            newP6.Inlines.Add("\n");
            newP6.Inlines.Add(AppResources.lblFormula7);
            newP6.Inlines.Add("\n\r");

            newP5.TextAlignment = TextAlignment.Justify;
            newP5.Inlines.Add(AppResources.lblFormula8);
            newP5.Inlines.Add("\n");
            newP5.Inlines.Add(AppResources.lblFormula9);
            newP5.Inlines.Add("\n\r");
            newP5.Inlines.Add(AppResources.lblFormula10);
            newP5.Inlines.Add("\n");
            newP5.Inlines.Add(AppResources.lblFormula11);

            rtxtFormula.Blocks.Add(newP3);
            rtxtFormula.Blocks.Add(newP4);
            rtxtFormula.Blocks.Add(newP6);
            rtxtFormula.Blocks.Add(newP5);
        } 
    }
}