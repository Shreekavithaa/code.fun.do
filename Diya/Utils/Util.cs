using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Diya.Utils
{
    class Util
    {        
        public async static Task displayMessage(string msg, string heading = "")
        {
            MessageDialog dialog = new MessageDialog(msg, heading);
            await dialog.ShowAsync();
        }
        public static void EmptyBackStack(Frame _frame)
        {
            try
            {
                while (_frame.BackStackDepth > 0)
                {
                    _frame.BackStack.RemoveAt(_frame.BackStackDepth - 1);
                }
            }
            catch
            {
                debugLog("Failed to empty backstack");
            }
        }
        public static void debugLog(params string[] p)
        {
            var str = "";
            foreach (string s in p)
            {
                str += s + " | ";
            }
            str += "<<<";
            Debug.WriteLine(str);
        }
        public static Boolean checkInternetConnection()
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
        public static string getregisterMD5(string plainText)
        {
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);
            IBuffer hashed = hasher.HashData(buffer);
            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return encodeBase64(hashedString);
        }

        public static string getMD5(string plainText)
        {
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);
            IBuffer hashed = hasher.HashData(buffer);
            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return getSHA1(hashedString);
        }

        public static String getMD5withoutSHA(string plainText)
        {
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);
            IBuffer hashed = hasher.HashData(buffer);
            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return hashedString;

        }
        public static string getSHA1(string plainText)
        {
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);
            IBuffer hashed = hasher.HashData(buffer);
            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return hashedString;
        }

        public static string getDeviceUniqueID()
        {
            HardwareToken myToken = HardwareIdentification.GetPackageSpecificToken(null);
            IBuffer hardwareId = myToken.Id;
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer hashed = hasher.HashData(hardwareId);
            return CryptographicBuffer.EncodeToHexString(hashed);
        }
        public static string encodeBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static Boolean IsValidEmail(string emailaddress)
        {
            string pattern = null;
            pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (Regex.IsMatch(emailaddress, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public async static void sendEmail()
        {
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = "";
            emailMessage.Subject = "Feedback";
            var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient("feedback@viscenario.com");
            emailMessage.To.Add(emailRecipient);
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
        public static BitmapImage base64image(string base64string)
        {
            byte[] fileBytes = Convert.FromBase64String(base64string);

            using (MemoryStream ms = new MemoryStream(fileBytes, 0, fileBytes.Length))
            {
                ms.Write(fileBytes, 0, fileBytes.Length);
                BitmapImage bitmapImage = new BitmapImage();
             //   bitmapImage.SetSource(ms);
                return bitmapImage;
            }
        }
    }
}
