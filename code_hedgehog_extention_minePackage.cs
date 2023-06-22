using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.ComponentModel.Composition;


namespace code_hedgehog_extention_mine
{


    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid("73a7c5cd-049e-44ec-9d38-9d5137d8634a")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MyToolWindow))]
    [ProvideToolWindow(typeof(LoginToolWindow))]
    public sealed class code_hedgehog_extention_minePackage : AsyncPackage
    {

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await MyToolWindowCommand.InitializeAsync(this);
            await LoginToolWindowCommand.InitializeAsync(this);;
            await SendSolutionCommand.InitializeAsync(this);
        }
    }
}
