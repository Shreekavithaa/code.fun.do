using Diya.Azure;
using Diya.Common;
using Diya.Utils;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Diya.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public LoginPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Util.EmptyBackStack(this.Frame);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void showProcessRing()
        {
            ProcessingRing.IsActive = true;
            Register.IsEnabled = false;
            Ring.Visibility = Visibility.Visible;
        }

        private void hideProcessRing()
        {
            ProcessingRing.IsActive = false;
            Register.IsEnabled = true;
            Ring.Visibility = Visibility.Collapsed;
        }
        private async Task<string> validateCreds()
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Password;
            string validCreds = "false";
            if (username.Trim().Equals(""))
            {
                await Util.displayMessage("Please Enter Email");

            }
            else if (password.Trim().Equals(""))
            {
                await Util.displayMessage("Please Enter Password");
            }
            else
            {
                validCreds = "";
            }
            return validCreds;
        }

        private void saveCredentials()
        {
            if (RememberMeCheckBox.IsChecked == true)
            {
                string username = UserNameTextBox.Text;
                string password = PasswordTextBox.Password;
                String[] creds = new[] { username, password };
                SharedPreferences.set("creds", creds);
            }
            else
            {
                SharedPreferences.remove("creds");
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String username = UserNameTextBox.Text;
            String password = PasswordTextBox.Password;
            var res = await validateCreds();
            if (res.Equals(""))
            {
                showProcessRing();
                if (!Netwoking.checkInternetConnectivity())
                {
                    hideProcessRing();
                    await Util.displayMessage("Check your internet connection");
                    return;
                }
                saveCredentials();

                //registerTable item = new registerTable {emailId = username, password = Util.getMD5(password) };
               
                try
                {
                    IMobileServiceTable<registerTable> todoTable = App.MobileService.GetTable<registerTable>();
                   // var x = await App.MobileService.GetTable<registerTable>().ToListAsync();
                    List<registerTable> items = await todoTable.Where(todoItem => todoItem.emailId == username).ToListAsync();
                    foreach (registerTable item1 in items)
                    {
                        Util.debugLog(item1.emailId.ToString());
                        if (item1.password.Equals(Util.getMD5(password)))
                        {
                            Constants.DISPLAY_NAME = item1.name;
                            Constants.EMAIL_ID = item1.emailId;
                            Constants.MOBILE_NUMBER = item1.mobileNumber.ToString();
                            hideProcessRing();
                            Frame.Navigate(typeof(HomePage));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.debugLog(ex.ToString());
                }
                hideProcessRing();
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }
    }
}
