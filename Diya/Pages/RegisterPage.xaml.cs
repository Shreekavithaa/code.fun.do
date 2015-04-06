using Diya.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Diya.Utils;
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
using Diya.Azure;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Diya.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public RegisterPage()
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

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            var res = validateFields();
            if (!res.Equals(""))
            {
                Debug.WriteLine("Validated");
                await registerAysncCall();
            }
        }
        private void showProcessRing()
        {
            Ring.Visibility = Visibility.Visible;
            ProcessingRing.IsActive = true;
        }

        private void hideProcessRing()
        {
            ProcessingRing.IsActive = false;
            Ring.Visibility = Visibility.Collapsed;
        }
        public async Task<string> validateFields()
        {
            string responseMessage = null;
            string _registername = nameregister.Text;
            string _registeremail = emailregister.Text;
            string _registerphone = phoneregister.Text;
            string _registerpass = passregister.Password;
            string _registerconfpass = confpassregister.Password;

            if (_registername.Equals("") || _registeremail.Equals("") || _registerphone.Equals("") || _registerpass.Equals("") || _registerconfpass.Equals(""))
            {
                responseMessage = "Please fill the fields";
            }
            else if (!Util.IsValidEmail(_registeremail))
            {
                responseMessage = "Please enter a valid email";
            }
            else if (!(_registerphone.Trim().Length > 8 && _registerphone.Trim().Length < 19))
            {
                responseMessage = "Please enter valid phonenumber";
            }
            else if (!_registerpass.Equals(_registerconfpass))
            {
                responseMessage = "Password and Confirm Password should be same";
            }
            if (responseMessage != null)
            {
                await Util.displayMessage("", responseMessage);
                return "";
            }
            return "true";
        }
        private async Task registerAysncCall()
        {
            showProcessRing();
            if (!Util.checkInternetConnection())
            {
                hideProcessRing();
                await Util.displayMessage("No Internet Connection", "Alert");

            }
            else
            {
                String registername = nameregister.Text;
                String registeremail = emailregister.Text;
                String registerphone = phoneregister.Text;
                String registerpass = passregister.Password;
                String registerconfpass = confpassregister.Password;

                registerTable item = new registerTable { name = registername, emailId = registeremail, mobileNumber = Convert.ToInt32(registerphone), password = Util.getMD5(registerconfpass) };
                try
                {
                    await App.MobileService.GetTable<registerTable>().InsertAsync(item);
                    hideProcessRing();
                    Frame.Navigate(typeof(LoginPage));
                }
                catch (Exception ex)
                {
                    Util.debugLog(ex.ToString());
                }

                
            }
        }

    }
}
