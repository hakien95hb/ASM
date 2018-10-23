using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


using System.Net;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace asssssssssssss.views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogIn : Page
    {
        public LogIn()
        {
            this.InitializeComponent();
        }

        private async void login_bth(object sender, RoutedEventArgs e)
        {
            string email = this.t_name.Text;
            string password = this.t_pass.Password;
            Dictionary<String, String> memberLogin = new Dictionary<string, string>();
            memberLogin.Add("email", email);
            memberLogin.Add("password", password);
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(memberLogin), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication", content);
            var kien102 = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(kien102);

        }
    }
}
