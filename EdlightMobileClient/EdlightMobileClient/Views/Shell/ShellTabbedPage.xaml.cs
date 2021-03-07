using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EdlightMobileClient.Views.Shell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellTabbedPage : TabbedPage
    {
        public ShellTabbedPage()
        {
            InitializeComponent();
        }
    }
}