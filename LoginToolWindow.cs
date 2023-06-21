using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace code_hedgehog_extention_mine
{

    [Guid("d77f916f-53f7-4996-8d22-d1bf6944a581")]
    public class LoginToolWindow : ToolWindowPane
    {
        public LoginToolWindow() : base(null)
        {
            this.Caption = "Вход в ежа";
            this.Content = new LoginToolWindowControl();
        }
    }
}
