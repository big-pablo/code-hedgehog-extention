using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.ComponentModel.Composition;
using static code_hedgehog_extention_mine.ExtStorageClass;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace code_hedgehog_extention_mine
{

    public partial class LoginToolWindowControl : UserControl
    {
        //private WritableSettingsStore __writableSettingsStore;
        public LoginToolWindowControl()
        {
            this.InitializeComponent();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var body = new Dictionary<string, string>
            {
                {"password", PasswordBox.Password},
                {"login", EmailTextBox.Text},
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsync("http://127.0.0.1:5000/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var message = responseString.ToString();
            JObject jsonObject = JObject.Parse(message);
            string output;
            if (jsonObject["token"] != null)
            {
                output = jsonObject["token"].ToString();
                using (StreamWriter writer = new StreamWriter("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\token.txt", false))
                {
                    writer.Write(output);
                }           
            }
            else
            {
                output = jsonObject["message"].ToString();
                MessageBox.Show(output);
            }
            MessageBox.Show(output);
        }
    }
}