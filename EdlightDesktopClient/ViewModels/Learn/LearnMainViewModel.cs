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
        private IRegionManager manager;
        #endregion
        #region fields

        #endregion
        #region props

        #endregion
        #region commads
        public DelegateCommand LoadedCommand { get; private set; }
        #endregion
        #region constructo
        public LearnMainViewModel(IRegionManager manager)
        {
            this.manager = manager;

            LoadedCommand = new(OnLoaded);
        }
        #endregion
        private void OnLoaded()
        {
            manager.RequestNavigate(BaseMethods.RegionNames.TestRegion, nameof(TestListView));
            manager.RequestNavigate(BaseMethods.RegionNames.StorageRegion, nameof(StorageListView));
            manager.RequestNavigate(BaseMethods.RegionNames.FileRegion, nameof(FileListFiew));
        }
    }
}
