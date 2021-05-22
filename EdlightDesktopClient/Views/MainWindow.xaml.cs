using Prism.Regions;
using System.Windows;
using System.Windows.Input;

namespace EdlightDesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IRegionManager manager)
        {
            InitializeComponent();
            Height = SystemParameters.FullPrimaryScreenHeight;

            if (manager != null)
            {
                SetRegionManager(manager, ScheduleRegion, BaseMethods.RegionNames.ScheduleRegion);
                SetRegionManager(manager, DictionariesRegion, BaseMethods.RegionNames.DictionariesRegion);
                SetRegionManager(manager, ProfileRegion, BaseMethods.RegionNames.ProfileRegion);
                SetRegionManager(manager, LearnRegion, BaseMethods.RegionNames.LearnRegion);
                SetRegionManager(manager, SettingsRegion, BaseMethods.RegionNames.SettingsRegion);
                SetRegionManager(manager, GroupsRegion, BaseMethods.RegionNames.GroupsRegion);
                SetRegionManager(manager, ModalRegion, BaseMethods.RegionNames.ModalRegion);
            }
        }

        private void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }
        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) return;
            DragMove();
        }
    }
}
