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

namespace code_hedgehog_extention_mine
{

    public partial class LoginToolWindowControl : UserControl
    {

        public LoginToolWindowControl()
        {
            this.InitializeComponent();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "LoginToolWindow");
        }

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
            var message = responseString.ToString(); //Здесь message если чёто хуёво и token если всё zayebisse
            JObject jsonObject = JObject.Parse(message);
            string output;
            if (jsonObject["token"] != null)
            {
                output = jsonObject["token"].ToString();
                //SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
                //WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
                //userSettingsStore.SetString("code_hedgehog_extention_mine", "token", output); Вот здесь хуйня типа Значение не попадает в ожидаемый диапазон
                //MessageBox.Show(userSettingsStore.GetString("code_hedgehog_extention_mine", "token", defaultValue: string.Empty));
                //Но всё норм, токен получается, сохраняется в аутпуте, осталось его как-то куда-нибудь записать, выше пытался            
            }
            else
            {
                output = jsonObject["message"].ToString();
                MessageBox.Show(output);
            }
        }
    }
}