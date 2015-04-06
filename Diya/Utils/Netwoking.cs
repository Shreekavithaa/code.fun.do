using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Diya.Utils
{
    class Netwoking
    {
        public static bool checkInternetConnectivity()
        {
            string connectionProfileInfo = string.Empty;
            try
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (InternetConnectionProfile != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.debugLog(ex.ToString());
                return false;
            }
        }
      
    }
}
