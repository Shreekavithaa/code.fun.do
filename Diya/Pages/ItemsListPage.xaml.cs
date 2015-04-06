using Diya.Common;
using Diya.ItemsList;
using Diya.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
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
    public sealed partial class ItemsListPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemsListPage()
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
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            showProcessRing();
            //var sampleDataGroups = await DataSource.GetGroupsAsync();

            //foreach (SampleDataItem item in sampleDataGroups)
            //{
            //    Utils.Util.debugLog(item.Title, item.type, item.Index, item.image,"***1");
            //}
            ////this.DefaultViewModel["Groups"] = sampleDataGroups;
            var sampleDataGroups_Location = await DataSource.GetLocationAsync();
            foreach (SampleDataItem item in sampleDataGroups_Location)
            {
                Utils.Util.debugLog(item.Title, item.type, item.Index, "***1");
            }
            string latitude;
            string longitude;
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {

                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                latitude = geoposition.Coordinate.Point.Position.Latitude.ToString("0.00");
                longitude = geoposition.Coordinate.Point.Position.Longitude.ToString("0.00");
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    // Util.displayMessage("", "Location is disabled in phone settings");
                    //locationHeader.Text = "Location is disabled in phone settings";

                }
                else
                {
                    //  Util.displayMessage("", "Something else happened acquring the location");
                   // locationHeader.Text = "Couldn't get location ";
                }
            }

            this.DefaultViewModel["Groups1"] = sampleDataGroups_Location;
            var sampleDataGroups_Favourite = await DataSource.GetFavouriteAsync();
            foreach (SampleDataItem item in sampleDataGroups_Favourite)
            {
                Utils.Util.debugLog(item.Title, item.type, item.Index, "***1");
            }
            this.DefaultViewModel["Groups2"] = sampleDataGroups_Favourite;
            hideProcessRing();
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

            Constants.DISPLAY_NAME = "";
            Constants.EMAIL_ID = "";
            Constants.MOBILE_NUMBER = "";
            Frame.Navigate(typeof(LoginPage));
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

        private void HUB_Items_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {

        }

        private void ItemViewer_Tapped(object sender, TappedRoutedEventArgs e)
        {

            var thisElement = sender as Grid;
            var index = ((TextBlock)thisElement.FindName("Index")).Text;
            var group = ((TextBlock)thisElement.FindName("Group")).Text;

            List<string> passing = new List<string>();
            passing.Add(index);
            passing.Add(group);
            Frame.Navigate(typeof(ItemDescriptionPage), passing);
        }
    }
}
