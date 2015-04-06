using Diya.Azure;
using Diya.Common;
using Diya.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Diya.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DonatePage : Page, IFileOpenPickerContinuable
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public DonatePage()
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Constants.DISPLAY_NAME = "";
            Constants.EMAIL_ID = "";
            Constants.MOBILE_NUMBER = "";
            Frame.Navigate(typeof(LoginPage));
        }

        private void textAddImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Util.debugLog("image button clicked ......... ");
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Clear();
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.PickSingleFileAndContinue();
            }
            catch
            {
                Util.debugLog("s1");
            }
        }

        string str = "";
        

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            try
            {
                Util.debugLog("Hello");
                if (args.Files.Count > 0)
                {
                    Util.debugLog("Hello");
                    textAddImage.Label = "Replace Image";
                    var stream = await args.Files[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
                    var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    CameraPreview.Source = bitmapImage;
                    RandomAccessStreamReference rasr = RandomAccessStreamReference.CreateFromStream(stream);
                    var streamWithContent = await rasr.OpenReadAsync();
                    byte[] buffer = new byte[streamWithContent.Size];
                    //showProcessRing();
                    await streamWithContent.ReadAsync(buffer.AsBuffer(), (uint)streamWithContent.Size, InputStreamOptions.None);
                    //hideProcessRing();
                    str = Convert.ToBase64String(buffer);
                    int S1Length = str.Length;
                    Util.debugLog(str);
                }
            }
            catch
            {
                Util.debugLog("s2");
            }
        }
        private string areaLatitude = "";
        private string areaLongitude = "";

        private string title1 = "";
        private string message = "";
        private string landMark = "";
        private string type1 = "";

        public async Task alias()
        {
            showProcessRing();
            if (Type.SelectedIndex.Equals(0))
            {
                type1 = "Food";
            }
            else if (Type.SelectedIndex.Equals(1))
            {
                type1 = "Clothing";
            }
            if (AreaName.IsEnabled == true)
            {
                string page = "http://maps.google.com/maps/api/geocode/xml?address=" + AreaName.Text + "&sensor=false";
                // ... Use HttpClient.
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(page))
                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    string result = await content.ReadAsStringAsync();

                    // ... Display the result.
                    if (result != null)
                    {
                        Util.debugLog(result + "...");
                        Util.debugLog(result.IndexOf("location").ToString());
                        int start = result.IndexOf("location") + 19;
                        int end = result.IndexOf("location") + 28;
                        string x = "";
                        for (int s = start; s <= end; s++)
                        {
                            x = x + result[s];
                        }
                        areaLatitude = x;
                        start = result.IndexOf("location") + 45;
                        end = result.IndexOf("location") + 54;
                        x = "";
                        for (int s = start; s <= end; s++)
                        {
                            x = x + result[s];
                        }
                        areaLongitude = x;
                    }
                }
            }
            else
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracyInMeters = 50;
                try
                {

                    Geoposition geoposition = await geolocator.GetGeopositionAsync(
                        maximumAge: TimeSpan.FromMinutes(5),
                        timeout: TimeSpan.FromSeconds(10)
                        );

                    areaLatitude = geoposition.Coordinate.Point.Position.Latitude.ToString("0.00");
                    areaLongitude = geoposition.Coordinate.Point.Position.Longitude.ToString("0.00");
                }
                catch (Exception ex)
                {
                    if ((uint)ex.HResult == 0x80004004)
                    {
                        // the application does not have the right capability or the location master switch is off
                        // Util.displayMessage("", "Location is disabled in phone settings");
                        Util.debugLog("Location is disabled in phone settings");

                    }
                    else
                    {
                        //  Util.displayMessage("", "Something else happened acquring the location");
                        Util.debugLog("Couldn't get location");
                    }
                }
            }
            landMark = LandMarkName.Text.ToString();
            title1 = TitleTextBox.Text.ToString();
            message = MsgTextBox.Text.ToString();
           // Util.debugLog(str, "Maddy");

            items item = new items { description = message, emailid = Constants.EMAIL_ID, image = str , latitude = areaLatitude, longitude = areaLongitude, landmark = landMark, type = type1, title = title1 };
            try
            {
                await App.MobileService.GetTable<items>().InsertAsync(item);
                Frame.Navigate(typeof(HomePage));
                hideProcessRing();
            }
            catch (Exception ex)
            {
                Util.debugLog(ex.ToString());
                hideProcessRing();
            }
        }
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            await alias(); 
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

        private void Location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

    
            if (Location.SelectedIndex.ToString().Equals("0"))
            {
                AreaName.IsEnabled = false;
            }
            else
            {
                AreaName.IsEnabled = true;
            }
           
        }
        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
