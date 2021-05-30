using EdlightDesktopClient.Views.Learn;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class LearnMainViewModel : BindableBase
    {
        #region services
        private readonly IRegionManager manager;
        #endregion
        #region fields

        private bool _isConfigured;
        private int _selectedTabIndex;

        #endregion
        #region props

        public int SelectedTabIndex { get => _selectedTabIndex; set => SetProperty(ref _selectedTabIndex, value); }

        #endregion
        #region commads
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand TabSelectionChangedCommand { get; private set; }
        #endregion
        #region constructor
        public LearnMainViewModel(IRegionManager manager)
        {
            this.manager = manager;

            LoadedCommand = new(OnLoaded);
            //TabSelectionChangedCommand = new DelegateCommand(OnTabSelectionChanged);
        }
        #endregion
        private void OnLoaded()
        {
            if (!_isConfigured)
            {
                manager.RequestNavigate(BaseMethods.RegionNames.TestRegion, nameof(TestListView));
                manager.RequestNavigate(BaseMethods.RegionNames.StorageRegion, nameof(StorageListView));
                manager.RequestNavigate(BaseMethods.RegionNames.FileRegion, nameof(FileListFiew));
                _isConfigured = true;
            }
        }
        private void OnTabSelectionChanged()
        {
            if (manager.Regions.ContainsRegionWithName(BaseMethods.RegionNames.TestRegion))
            {
                manager.Regions[BaseMethods.RegionNames.TestRegion].RemoveAll();
            }
            if (manager.Regions.ContainsRegionWithName(BaseMethods.RegionNames.StorageRegion))
            {
                manager.Regions[BaseMethods.RegionNames.StorageRegion].RemoveAll();
            }
            if (manager.Regions.ContainsRegionWithName(BaseMethods.RegionNames.FileRegion))
            {
                manager.Regions[BaseMethods.RegionNames.FileRegion].RemoveAll();
            }

            switch (SelectedTabIndex)
            {
                case 0:
                    {
                        manager.RequestNavigate(BaseMethods.RegionNames.TestRegion, nameof(TestListView));
                    }
                    break;
                case 1:
                    {
                        manager.RequestNavigate(BaseMethods.RegionNames.StorageRegion, nameof(StorageListView));
                    }
                    break;
                case 2:
                    {
                        manager.RequestNavigate(BaseMethods.RegionNames.FileRegion, nameof(FileListFiew));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
