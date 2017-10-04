
using Android.Graphics;
using System.Net;


namespace Mica.Droid.Server
{
    public class Client
    {
        private string urlImage;
        private string urlTxt;
        private string urlLights;

        public Client()
        {
            urlImage = "http://192.168.100.11:8033/imagen.jpg";
            urlTxt = "http://192.168.100.11:8033/casa.txt";
            urlLights = "http://192.168.100.11:8033/";
        }

        public Bitmap GetImageBitmapFromUrl()
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(urlImage);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }

        public string GetTextFromUrl()
        {
            string text = null;
            using (var webClient = new WebClient())
            {
                text = webClient.DownloadString(urlTxt);
            }
            return text;
        }

        public void SendLightByUrl(string lightsInfo)
        {
            string url = urlLights + lightsInfo;
            using (var webClient = new WebClient())
            {
                string text = webClient.DownloadString(url);
            }
        }
    }
}