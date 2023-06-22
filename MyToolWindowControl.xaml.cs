using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Threading.Tasks;
using System;
using static code_hedgehog_extention_mine.SendSolutionCommand;

namespace code_hedgehog_extention_mine
{

    public partial class MyToolWindowControl : UserControl
    {
        public MyToolWindowControl()
        {
            this.InitializeComponent();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]

        private MenuItem previousMenuItem;
        private MenuItem previousLangItem;
        private async void UserControl_LoadedAsync(object sender, RoutedEventArgs e)
        {
            string token;
            using (StreamReader reader = new StreamReader("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\token.txt"))
            {
                token = reader.ReadToEnd();
            }
            //Получаем все задачи
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("http://127.0.0.1:5000/tasks/1");
            var responseString = await response.Content.ReadAsStringAsync();
            var message = responseString.ToString();

           for (int i = 0; i < 10; i++)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = $"Task n.{i}";
                menuItem.Tag = Guid.NewGuid().ToString();
                menuItem.Click += TaskItem_Click;
                TaskMenuItem.Items.Add(menuItem);
            }
            for (int i = 0; i < 5; i++)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = $"Language n.{i}";
                menuItem.Tag = Guid.NewGuid().ToString();
                menuItem.Click += LangItem_Click;
                LangMenuItem.Items.Add(menuItem);
            }
        }

        private async void TaskItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (previousMenuItem != null)
            {
                previousMenuItem.IsChecked = false;
            }
            menuItem.IsChecked = true;
            string name = menuItem.Header.ToString();
            string description = menuItem.Tag.ToString();
            using (StreamWriter writer = new StreamWriter("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\currentTask.txt", false))
            {
                writer.Write(description);
            }
            previousMenuItem = menuItem;
            TaskHeader.Content = name;
            TaskDescription.Text = description;
            MessageBox.Show($"Выбрана задача {name}");
        }

        private async void LangItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (previousLangItem != null)
            {
                previousLangItem.IsChecked = false;
            }
            menuItem.IsChecked = true;
            string name = menuItem.Header.ToString();
            string description = menuItem.Tag.ToString();
            using (StreamWriter writer = new StreamWriter("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\currentLanguage.txt", false))
            {
                writer.Write(description);
            }
            previousLangItem = menuItem;
            MessageBox.Show($"Выбран язык {name}");
        }
    }
}