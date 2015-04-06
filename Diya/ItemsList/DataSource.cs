using Diya.Azure;
using Diya.Utils;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Diya.Azure;
using Diya.Common;
using Diya.ItemsList;
using Diya.Utils;
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

namespace Diya.ItemsList
{
    public class SampleDataItem
    {
        public SampleDataItem(String name, String Type, String Status, String index, string group1, string group)
        {
            this.Title = name;
            this.type = Type;
            this.status = Status;
            this.Index = index;
            this.image = group1;
            this.grouping = group;
        }

        public string Title { get; private set; }
        public string type { get; private set; }
        public string status { get; private set; }
        public string image { get; private set; }
        public string Index { get; private set; }

        public string grouping { get; private set; }


    }
     public sealed class DataSource
    {
        private static DataSource _sampleDataSource = new DataSource();

        private ObservableCollection<SampleDataItem> _groups = new ObservableCollection<SampleDataItem>();
        private ObservableCollection<SampleDataItem> _groups1 = new ObservableCollection<SampleDataItem>();
        private ObservableCollection<SampleDataItem> _groups2 = new ObservableCollection<SampleDataItem>();

        private ObservableCollection<SampleDataItem> _searchgroups = new ObservableCollection<SampleDataItem>();
        private ObservableCollection<SampleDataItem> _searchgroups1 = new ObservableCollection<SampleDataItem>();
        private ObservableCollection<SampleDataItem> _searchgroups2 = new ObservableCollection<SampleDataItem>();

        private static List<items> List_FacilityBeans_All = new List<items>();
        private static List<items> List_FacilityBean_Location = new List<items>();

        private static List<items> List_FacilityBean_Favourite = new List<items>();

        public static items GetBean_All(int index)
        {
            return List_FacilityBeans_All[index];
        }
        public static items GetBean_Location(int index)
        {
            return List_FacilityBean_Location[index];
        }
        public static items GetBean_Favourite(int index)
        {
            return List_FacilityBean_Favourite[index];
        }
        public ObservableCollection<SampleDataItem> Groups
        {
            get { return this._groups; }
        }
        public ObservableCollection<SampleDataItem> Groups1
        {
            get { return this._groups1; }
        }
        public ObservableCollection<SampleDataItem> Groups2
        {
            get { return this._groups2; }
        }

        public ObservableCollection<SampleDataItem> SearchGroups
        {
            get { return this._searchgroups; }
        }
        public ObservableCollection<SampleDataItem> SearchGroups1
        {
            get { return this._searchgroups1; }
        }
        public ObservableCollection<SampleDataItem> SearchGroups2
        {
            get { return this._searchgroups2; }
        }
        public static async Task<IEnumerable<SampleDataItem>> GetGroupsAsync()
        {

            await _sampleDataSource.GetAllAsync();
            return _sampleDataSource.Groups;
        }
        public static async Task<IEnumerable<SampleDataItem>> GetLocationAsync()
        {
            await _sampleDataSource.GetDataAsync_Location();
            return _sampleDataSource.Groups1;
        }
        public static async Task<IEnumerable<SampleDataItem>> GetFavouriteAsync()
        {
            await _sampleDataSource.GetDataAsync();
            return _sampleDataSource.Groups2;
        }
        public static async Task<IEnumerable<SampleDataItem>> GetSearchGroupsAsync(string text)
        {

            await _sampleDataSource.GetSearchResults(text, "Groups");
            return _sampleDataSource.SearchGroups;
        }
        public static async Task<IEnumerable<SampleDataItem>> GetSearchLocationAsync(string text)
        {
            await _sampleDataSource.GetSearchResults(text, "Groups1");
            return _sampleDataSource.SearchGroups1;
        }
        public static async Task<IEnumerable<SampleDataItem>> GetSearchFavouriteAsync(string text)
        {
            await _sampleDataSource.GetSearchResults(text, "Groups2");
            return _sampleDataSource.SearchGroups2;
        }
        private async Task GetSearchResults(string text, string section)
        {
            if (section.Equals("Groups"))
            {
                _searchgroups = new ObservableCollection<SampleDataItem>();
                foreach (var x in _groups)
                {
                    Debug.WriteLine(x.Title);
                    String title = x.Title.ToUpper();
                    if (title.Contains(text.ToUpper()))
                    {
                        Debug.WriteLine("Entered Search2");
                        SampleDataItem item = new SampleDataItem(x.Title, x.type, x.status, x.Index, x.image, x.grouping);
                        Util.debugLog(item.Title, item.type, item.status, "****000****");
                        this.SearchGroups.Add(item);
                        Debug.WriteLine("1:" + x.Title);
                    }
                }
            }
            else if (section.Equals("Groups1"))
            {
                _searchgroups1 = new ObservableCollection<SampleDataItem>();
                foreach (var x in _groups1)
                {
                    Debug.WriteLine(x.Title);
                    String title = x.Title.ToUpper();
                    if (title.Contains(text.ToUpper()))
                    {
                        Debug.WriteLine("Entered Search2");
                        SampleDataItem item = new SampleDataItem(x.Title, x.type, x.status, x.Index, x.image, x.grouping);
                        Util.debugLog(item.Title, item.type, item.status, "****000****");
                        this.SearchGroups1.Add(item);
                        Debug.WriteLine("1:" + x.Title);
                    }
                }
            }
            else if (section.Equals("Groups2"))
            {
                _searchgroups2 = new ObservableCollection<SampleDataItem>();
                foreach (var x in _groups2)
                {
                    Debug.WriteLine(x.Title);
                    String title = x.Title.ToUpper();
                    if (title.Contains(text.ToUpper()))
                    {
                        Debug.WriteLine("Entered Search2");
                        SampleDataItem item = new SampleDataItem(x.Title, x.type, x.status, x.Index, x.image, x.grouping);
                        Util.debugLog(item.Title, item.type, item.status, "****000****");
                        this.SearchGroups2.Add(item);
                        Debug.WriteLine("1:" + x.Title);
                    }
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(0));
        }
        private async Task GetDataAsync()
        {
            List_FacilityBeans_All = new List<items>();
            _groups2 = new ObservableCollection<SampleDataItem>();
            if (Util.checkInternetConnection())
            {
                var response = await App.MobileService.GetTable<items>().ToListAsync();
                for (int i = 0; i < response.Count(); i++)
                {
                    SampleDataItem item_1 = new SampleDataItem(response[i].title, response[i].type, response[i].status, i.ToString(), response[i].image, "All");
                     List_FacilityBeans_All.Add(response[i]);
                    this.Groups2.Add(item_1);
                    Util.debugLog(response[i].image, response[i].id, item_1.grouping, i.ToString());
                }
                

            }
            else
            {
                await Util.displayMessage("No Internet Connection", "Caution");
            }
        }
        private async Task GetDataAsync_Location()
        {
            _groups1 = new ObservableCollection<SampleDataItem>();
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
                await sendinglocationrequest(latitude, longitude);
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    //  Util.displayMessage("", "Location is disabled in phone settings");

                }
                else
                {
                    // Util.displayMessage("", "Something else happened acquring the location");
                }
            }
        }
        public async Task sendinglocationrequest(string latitude, string longitude)
        {
            List_FacilityBean_Location = new List<items>();
            var response = await App.MobileService.GetTable<items>().ToListAsync();

            IMobileServiceTable<items> todoTable = App.MobileService.GetTable<items>();
            // var x = await App.MobileService.GetTable<registerTable>().ToListAsync();
            List<items> items = await todoTable.Where(todoItem => (todoItem.latitude == latitude && todoItem.longitude == longitude)).ToListAsync();
            for (int i = 0; i < items.Count(); i++)
            {
                SampleDataItem item_1 = new SampleDataItem(items[i].title, items[i].type, items[i].status, i.ToString(), items[i].image, "Location");
                List_FacilityBean_Location.Add(items[i]);

               
                this.Groups1.Add(item_1);
            }      
        }
        private async Task GetAllAsync()
        {
            List_FacilityBean_Favourite = new List<items>();
            _groups = new ObservableCollection<SampleDataItem>();
            if (Util.checkInternetConnection())
            {
                var response = await App.MobileService.GetTable<items>().ToListAsync();
                for (int i = 0; i < response.Count(); i++)
                {
                    SampleDataItem item_1 = new SampleDataItem(response[i].title, response[i].type, response[i].status, i.ToString(), response[i].image , "All");
                    List_FacilityBean_Favourite.Add(response[i]);
                    this.Groups.Add(item_1);
                    Util.debugLog(response[i].image, response[i].id, item_1.grouping, i.ToString());
                }
            }
            else
            {
                await Util.displayMessage("No Internet Connection", "Caution");
            }
        }

    }
}
