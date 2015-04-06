using Diya.Azure;
using Diya.Common;
using Diya.ItemsList;
using Diya.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Diya.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDescriptionPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemDescriptionPage()
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
        /// 
        private string latitude;
        private string longitude;
        private string title1;

        private items item = new items();
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            List<string> list_string = new List<string>();
            list_string = (List<string>)e.NavigationParameter;
            Util.debugLog(list_string[0], list_string[1]);
            if (list_string[1].Equals("All"))
            {
                item = DataSource.GetBean_All(Convert.ToInt32(list_string[0]));
            }
            else
            {
                item = DataSource.GetBean_Location(Convert.ToInt32(list_string[0]));
            }
            title1 = item.title;
            Title.Text = item.title;
            Type.Text = item.type;
            Status.Text = item.status;
            Landmark.Text = item.landmark;
            Description.Text = item.description;
            Util.debugLog("Testing");
            Util.debugLog(item.image.Length.ToString());
            if (!item.image.Equals(""))
            {

                Util.debugLog("Testing into description !!!");
                Util.debugLog(item.image.Length.ToString());
                ///PostDetailsImage.Source = new BitmapImage(new Uri(item.image));
                byte[] byteBuffer = Convert.FromBase64String(item.image);
             //   PostDetailsImage.Source = byteArrayToImage(byteBuffer);
                byteArrayToImage(byteBuffer);
            }
            latitude = item.latitude;
            longitude = item.longitude;
        }

        

        public async void byteArrayToImage(byte[] byteArrayIn)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(byteArrayIn.AsBuffer(0, byteArrayIn.Length));
                stream.Seek(0);

                BitmapImage image = new BitmapImage();

                await image.SetSourceAsync(stream);

                PostDetailsImage.Source = image;
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

        private void GetDirections_Click(object sender, RoutedEventArgs e)
        {
            List<string> passing = new List<string>();
            passing.Add(latitude);
            passing.Add(longitude);
            Frame.Navigate(typeof(MapsPage), passing);
        }

        private async void StatusChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showProcessRing();
            string stat = "";
            if (StatusChange.SelectedIndex.Equals(0))
            {
                stat = "Closed";
            }
            else if (StatusChange.SelectedIndex.Equals(1))
            {
                stat = "Damaged";
            }

            Util.debugLog(item.id);
            JObject jo = new JObject();
            jo.Add("id", item.id);
            jo.Add("status", stat);

            try
            {
                await App.MobileService.GetTable<items>().UpdateAsync(jo);
                hideProcessRing();
                Frame.Navigate(typeof(ItemsListPage));
            }
            catch (Exception ex)
            {
                Util.debugLog(ex.ToString());
            }
            hideProcessRing();
        }
    }
}
