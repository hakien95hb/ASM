using asssssssssssss.Emtity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace asssssssssssss.views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class register : Page
    {
        user kien = new user();
        private string currentUploadUrl;
        private StorageFile photo;
        public register()
        {
            this.InitializeComponent();
        }

        private async void regi_bth(object sender, RoutedEventArgs e)
        {
            this.kien.email = this.t_email.Text;
            this.kien.password = this.t_password.Password;
            this.kien.firstName = this.t_firstname.Text;
            this.kien.lastName = this.t_lastname.Text;
            this.kien.address = this.t_address.Text;
            this.kien.introduction = this.t_introduction.Text;
            this.kien.phone = this.t_phone.Text;
            this.kien.avatar = this.UrlImage.Text;

            HttpClient http = new HttpClient();
            var kien0 = new StringContent(JsonConvert.SerializeObject(kien), System.Text.Encoding.UTF8, "application/json");
            var kien2 = http.PostAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/members",kien0);
            var kien3 = await kien2.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(kien3);
            if (kien2.Result.StatusCode == HttpStatusCode.Created)
            {
                

            }
            else
            {

                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(kien3);
                //Debug.WriteLine(errorResponse.error["firstName"]);
                if (errorResponse.error.Count > 0)
                {
                    foreach (var key in errorResponse.error.Keys)
                    {
                        if (this.FindName(key) is TextBlock textBlock)
                        {
                            textBlock.Text = errorResponse.error[key];                     
                        }
                    }
                }
            }
        }

        private async void capture_bth(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }

            HttpClient httpClient = new HttpClient();
            currentUploadUrl = await httpClient.GetStringAsync("https://2-dot-backup-server-002.appspot.com/get-upload-token");
            Debug.WriteLine(currentUploadUrl);
            HttpUploadFile(currentUploadUrl, "myFile", "image/png");

        }
        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await this.photo.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            System.Net.WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                
                string imageUrl = reader2.ReadToEnd();
                ava_pic.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                UrlImage.Text = imageUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }
        private void radio_checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            this.kien.gender = Int32.Parse(radio.Tag.ToString());
        }

        private void change_time(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.kien.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
        }
    }
}