using Diya.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Diya.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MapsPage()
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
        public string longitude;
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            List<string> list_string = new List<string>();
            list_string = (List<string>)e.NavigationParameter;
            latitude = list_string[0];
            longitude = list_string[1];
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
        private void buttonRoad_Click(object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.Road;
        }

        private void buttonAerial_Click(object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.Aerial;
        }

        private void buttonHybrid_Click(object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.AerialWithRoads;
        }

        private void buttonTerrain_Click(object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.Terrain;
        }

        private async void map1_Loaded(object sender, RoutedEventArgs e)
        {
           ZoomToMalmoe();
            //// Start at Grand Central Station in New York City.
            //BasicGeoposition startLocation = new BasicGeoposition();
            //startLocation.Latitude = 17.45;
            //startLocation.Longitude = 78.35;
            //Geopoint startPoint = new Geopoint(startLocation);

            //// End at Central Park in New York City.
            //BasicGeoposition endLocation = new BasicGeoposition();
            //endLocation.Latitude = 17.45;
            //endLocation.Longitude = 84.45;
            //Geopoint endPoint = new Geopoint(endLocation);

            //// Get the route between the points.
            //MapRouteFinderResult routeResult =
            //    await MapRouteFinder.GetDrivingRouteAsync(
            //    startPoint,
            //    endPoint,
            //    MapRouteOptimization.Time,
            //    MapRouteRestrictions.None,
            //    290
            //    );

            //if (routeResult.Status == MapRouteFinderStatus.Success)
            //{
            //    // Use the route to initialize a MapRouteView.
            //    MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
            //    viewOfRoute.RouteColor = Colors.Blue;
            //    viewOfRoute.OutlineColor = Colors.Blue;

            //    // Add the new MapRouteView to the Routes collection
            //    // of the MapControl.
            //    map1.Routes.Add(viewOfRoute);

            //    // Fit the MapControl to the route.
            //    await map1.TrySetViewBoundsAsync(
            //        routeResult.Route.BoundingBox,
            //        null,
            //        Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
            //}
           

          
        }

        private void buttonMalmoe_Click(object sender, RoutedEventArgs e)
        {
            ZoomToMalmoe();
        }

        private void ZoomToMalmoe()
        {

            var jayway = new Geopoint(new BasicGeoposition() { Latitude = Convert.ToDouble(latitude), Longitude = Convert.ToDouble(longitude) });
            var youPin = CreatePin();
            map1.Children.Add(youPin);
            MapControl.SetLocation(youPin, jayway);
            MapControl.SetNormalizedAnchorPoint(youPin, new Point(0.0, 1.0));
            map1.TrySetViewAsync(jayway, 15, 0, 0, MapAnimationKind.Bow);
        }

        private async void buttonYou_Click(object sender, RoutedEventArgs e)
        {
            var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
            var location = await gl.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));

            var pin = new MapIcon()
            {
                Location = location.Coordinate.Point,
                Title = "You are here!",
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Images/pin.png")),
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            };
            map1.MapElements.Add(pin);
            map1.TrySetViewAsync(location.Coordinate.Point, 16, 0, 0, MapAnimationKind.Bow);
        }

        private void buttonJayway_Click(object sender, RoutedEventArgs e)
        {
            var jayway = new Geopoint(new BasicGeoposition() { Latitude = Convert.ToDouble(latitude), Longitude = Convert.ToDouble(longitude) });
            var youPin = CreatePin();
            map1.Children.Add(youPin);
            MapControl.SetLocation(youPin, jayway);
            MapControl.SetNormalizedAnchorPoint(youPin, new Point(0.0, 1.0));
            map1.TrySetViewAsync(jayway, 15, 0, 0, MapAnimationKind.Bow);
        }


        private void buttonLondon_Click(object sender, RoutedEventArgs e)
        {
            var london = new Geopoint(new BasicGeoposition() { Latitude = 51.5067275986075, Longitude = -0.0759853888303041 });
            map1.TrySetViewAsync(london, 16.5, 0, 0, MapAnimationKind.Bow);
        }

        private void Landmarks_Checked(object sender, RoutedEventArgs e)
        {
            map1.TrySetViewAsync(map1.Center, map1.ZoomLevel, 340, 35, MapAnimationKind.Bow);
            map1.LandmarksVisible = true;
        }

        private void Landmarks_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.LandmarksVisible = false;
            map1.TrySetViewAsync(map1.Center, map1.ZoomLevel, 0, 0, MapAnimationKind.Bow);
        }

        private void Pedestrian_Checked(object sender, RoutedEventArgs e)
        {
            map1.PedestrianFeaturesVisible = true;
        }

        private void Pedestrian_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.PedestrianFeaturesVisible = false;
        }

        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            map1.ColorScheme = MapColorScheme.Dark;
        }

        private void Dark_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.ColorScheme = MapColorScheme.Light;
        }

        private void Traffic_Checked(object sender, RoutedEventArgs e)
        {
            map1.TrafficFlowVisible = true;
        }

        private void Traffic_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TrafficFlowVisible = false;
        }

        private void TileSource_Checked(object sender, RoutedEventArgs e)
        {
            var httpsource = new HttpMapTileDataSource("http://a.tile.openstreetmap.org/{zoomlevel}/{x}/{y}.png");
            var ts = new MapTileSource(httpsource)
            {
                Layer = MapTileLayer.BackgroundReplacement
            };
            map1.Style = MapStyle.None;
            map1.TileSources.Add(ts);
        }

        private void TileSource_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TileSources.Clear();
            map1.Style = MapStyle.Road;
        }

        private void CustTileSource_Checked(object sender, RoutedEventArgs e)
        {
            var customSource = new CustomMapTileDataSource();
            customSource.BitmapRequested += async (o, args) =>
            {
                var deferral = args.Request.GetDeferral();
                args.Request.PixelData = await CreateBitmapAsStreamAsync();
                deferral.Complete();
            };
            var ts = new MapTileSource(customSource)
            {
                Layer = MapTileLayer.BackgroundOverlay,
                IsTransparencyEnabled = true,
            };
            map1.TileSources.Add(ts);
        }


        private void CustTileSource_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TileSources.Clear();
            map1.Style = MapStyle.Road;
        }

        private async System.Threading.Tasks.Task<IRandomAccessStreamReference> CreateBitmapAsStreamAsync()
        {
            const int pixelHeight = 256;
            const int pixelWidth = 256;
            const int bpp = 4;

            var bytes = new byte[pixelHeight * pixelWidth * bpp];

            for (var y = 0; y < pixelHeight; y++)
            {
                for (var x = 0; x < pixelWidth; x++)
                {
                    var pixelIndex = y * pixelWidth + x;
                    var byteIndex = pixelIndex * bpp;

                    if (x % 64 > 16)
                    {
                        bytes[byteIndex] = 0x20;        // Red
                        bytes[byteIndex + 1] = 0x20;    // Green
                        bytes[byteIndex + 2] = 0x80;    // Blue
                        bytes[byteIndex + 3] = 0x00;    // Alpha (0xff = fully opaque)
                    }
                    else
                    {
                        bytes[byteIndex + 3] = 0xff;    // Alpha (0xff = fully opaque)
                    }
                }
            }

            // Create RandomAccessStream from byte array
            var randomAccessStream =
                new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var writer = new DataWriter(outputStream);
            writer.WriteBytes(bytes);
            await writer.StoreAsync();
            await writer.FlushAsync();
            return RandomAccessStreamReference.CreateFromStream(randomAccessStream);
        }


        private DependencyObject CreatePin()
        {
            //Creating a Grid element.
            var myGrid = new Grid();
            myGrid.RowDefinitions.Add(new RowDefinition());
            myGrid.RowDefinitions.Add(new RowDefinition());
            myGrid.Background = new SolidColorBrush(Colors.Transparent);

            //Creating a Rectangle
            var myRectangle = new Rectangle { Fill = new SolidColorBrush(Colors.Black), Height = 20, Width = 20 };
            myRectangle.SetValue(Grid.RowProperty, 0);
            myRectangle.SetValue(Grid.ColumnProperty, 0);

            //Adding the Rectangle to the Grid
            myGrid.Children.Add(myRectangle);

            //Creating a Polygon
            var myPolygon = new Polygon()
            {
                Points = new PointCollection() { new Point(2, 0), new Point(22, 0), new Point(2, 40) },
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.Black)
            };
            myPolygon.SetValue(Grid.RowProperty, 1);
            myPolygon.SetValue(Grid.ColumnProperty, 0);

            //Adding the Polygon to the Grid
            myGrid.Children.Add(myPolygon);
            return myGrid;
        }

        private async void Directions_Click(object sender, RoutedEventArgs e)
        {
            // Start at Grand Central Station.
            BasicGeoposition startLocation = new BasicGeoposition();
            startLocation.Latitude = 40.7517;
            startLocation.Longitude = -073.9766;
            Geopoint startPoint = new Geopoint(startLocation);
            // End at Central Park.
            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = 40.7669;
            endLocation.Longitude = -073.9790;
            Geopoint endPoint = new Geopoint(endLocation);
            // Get the route between the points.
            MapRouteFinderResult routeResult =
            await MapRouteFinder.GetDrivingRouteAsync(
              startPoint,
              endPoint,
              MapRouteOptimization.Time,
              MapRouteRestrictions.None,
              290);
           
        }

       

    
    
    }
}
