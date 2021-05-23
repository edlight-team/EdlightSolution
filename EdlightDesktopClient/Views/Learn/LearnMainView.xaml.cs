using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EdlightDesktopClient.Views.Learn
{
    /// <summary>
    /// Логика взаимодействия для LearnMainView.xaml
    /// </summary>
    public partial class LearnMainView : UserControl
    {
        public LearnMainView(IRegionManager manager)
        {
            InitializeComponent();

            if (manager != null)
            {
                SetRegionManager(manager, TestRegion, BaseMethods.RegionNames.TestRegion);
                SetRegionManager(manager, StorageRegion, BaseMethods.RegionNames.StorageRegion);
                SetRegionManager(manager, FileRegion, BaseMethods.RegionNames.FileRegion);
            }
        }

        private void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }
    }
}
