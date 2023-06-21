using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace code_hedgehog_extention_mine
{
    [Guid("f9becab8-5319-4621-a968-77a6de2c92d0")]
    public class MyToolWindow : ToolWindowPane
    {
        public MyToolWindow() : base(null)
        {
            this.Caption = "Задание CodeHedgehog";

            this.Content = new MyToolWindowControl();
        }
    }
}
