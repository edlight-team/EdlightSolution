using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace EdlightDesktopClient.Views.Schedule
{
    /// <summary>
    /// Логика взаимодействия для ScheduleMainView.xaml
    /// </summary>
    public partial class ScheduleMainView : UserControl
    {
        public ScheduleMainView(IRegionManager manager)
        {
            InitializeComponent();
            if (manager != null)
            {
                SetRegionManager(manager, ScheduleDateViewRegion, BaseMethods.RegionNames.ScheduleDateViewRegion);
            }
        }

        private void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }

        private void CreateMenuMouseEnter(object sender, System.Windows.Input.MouseEventArgs e) => CreateMenu.IsSubmenuOpen = true;
    }
}
