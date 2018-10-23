using asssssssssssss.Emtity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class inf : Page
    {
        iff iff = new iff();
        public inf()
        {
            this.InitializeComponent();
            GetInfo();
        }

        public async void GetInfo()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "q5QslJWnQ7QZvGMhCuiDFimcKu9NSp019ibVzckYowfHKnn8mYYwRv0TwLlNvu9a");
            var response = httpClient.GetAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/members/information");
            var content = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(content);
            iff = JsonConvert.DeserializeObject<iff>(content);

            Debug.WriteLine(iff.email);
            this.txt_fullname.Text = iff.firstName + " " + iff.lastName;
            this.txt_birthday.Text = iff.birthday;
            this.txt_email.Text = iff.email;
            this.txt_address.Text = iff.address;
            this.img_avatar.Source = new BitmapImage(new Uri(iff.avatar, UriKind.Absolute));

        }
    }
}
