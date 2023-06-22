using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace code_hedgehog_extention_mine
{
    internal sealed class SendSolutionCommand
    {
        public const int CommandId = 4130;

        public static readonly Guid CommandSet = new Guid("c7fe77cc-4ef5-4314-8c05-e575d7d14633");

        private readonly AsyncPackage package;

        private SendSolutionCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static SendSolutionCommand Instance
        {
            get;
            private set;
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new SendSolutionCommand(package, commandService);
        }

        private async void Execute(object sender, EventArgs e)
        {
            string langId;
            using (StreamReader reader = new StreamReader("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\currentLanguage.txt"))
            {
                langId = reader.ReadToEnd();
            }
            string codeText = Clipboard.GetText();
            var values = new Dictionary<string, string>
            {
                {"code", codeText},
                {"languageId", langId }
            };

            string token;
            using (StreamReader reader = new StreamReader("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\token.txt"))
            {
                token = reader.ReadToEnd();
            }
            string taskId;
            using (StreamReader reader = new StreamReader("D:\\C# Projects\\code-hedgehog-extention-mine\\Resources\\currentTask.txt"))
            {
                taskId = reader.ReadToEnd();
            }
            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync($"http://127.0.0.1:5000/task/{taskId}/test", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var message = responseString.ToString();
            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                this.package,
                message,
                "Отправили",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
