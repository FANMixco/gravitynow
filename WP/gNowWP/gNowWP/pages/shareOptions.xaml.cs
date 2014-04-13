using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using gNowWP.Resources;

namespace La_Ruta_Maya.pages
{
    public partial class shareOptions : PhoneApplicationPage
    {
        public shareOptions()
        {
            InitializeComponent();
        }

        private void ListBoxItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string gravity = this.NavigationContext.QueryString["gravity"];

            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.Body = AppResources.lblBody1 + gravity + AppResources.lblBody2;

            smsComposeTask.Show();
        }

        private void ListBoxItem_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string gravity = this.NavigationContext.QueryString["gravity"];

            EmailComposeTask task = new EmailComposeTask();

            task.Subject = AppResources.lblSubject;

            task.Body = AppResources.lblBody1 + gravity + AppResources.lblBody2;

            task.Show();
        }

        private void ListBoxItem_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string gravity = this.NavigationContext.QueryString["gravity"];

            ShareStatusTask shareStatusTask = new ShareStatusTask();

            shareStatusTask.Status = AppResources.lblBody1 + gravity + AppResources.lblBody2;

            shareStatusTask.Show();
        }
    }
}