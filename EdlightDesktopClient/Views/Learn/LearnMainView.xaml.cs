using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

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
