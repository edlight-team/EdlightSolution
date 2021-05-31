using ApplicationEventsWPF.Events.GroupEvent;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.NotificationService;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Groups
{
    public class AddDeleteGroupViewModel : BindableBase, INavigationAware
    {
        #region services
        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly INotificationService notification;
        private readonly IEventAggregator aggregator;
        #endregion
        #region fields
        private LoaderModel loader;

        private bool isAddingGroup;

        private string groupName;
        private ObservableCollection<GroupsModel> groups;
        private GroupsModel selectedGroup;
        #endregion
        #region props
        public LoaderModel Loader { get => loader; set => SetProperty(ref loader, value); }

        public bool IsAddingGroup { get => isAddingGroup; set => SetProperty(ref isAddingGroup, value); }

        public string GroupName { get => groupName ??= string.Empty; set => SetProperty(ref groupName, value); }
        public ObservableCollection<GroupsModel> Groups { get => groups ??= new(); set => SetProperty(ref groups, value); }
        public GroupsModel SelectedGroup { get => selectedGroup ??= new(); set => SetProperty(ref selectedGroup, value); }
        #endregion
        #region commands
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand GroupCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }
        #endregion
        #region ctor
        public AddDeleteGroupViewModel(IRegionManager manager, IWebApiService api, INotificationService notification, IEventAggregator aggregator)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.notification = notification;
            this.aggregator = aggregator;

            LoadedCommand = new(OnLoaded);
            GroupCommand = new(OnGroup);
            CloseModalCommand = new(OnCloseModal);
        }
        #endregion
        #region methods
        private async void OnLoaded()
        {
            if (!IsAddingGroup)
            {
                try
                {
                    Loader.SetDefaultLoadingInfo();

                    Groups = new(await api.GetModels<GroupsModel>(WebApiTableNames.Groups));
                }
                catch (Exception ex)
                {
                    notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                    throw;
                }
                finally
                {
                    await Loader.Clear();
                }
            }
        }
        private async void OnGroup()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();

                if (IsAddingGroup)
                {
                    if (string.IsNullOrEmpty(GroupName))
                    {
                        notification.ShowInformation("Введите имя группы");
                        return;
                    }

                    GroupsModel model = new();
                    model.Group = GroupName;
                    await api.PostModel(model, WebApiTableNames.Groups);
                }
                else
                {
                    if (SelectedGroup == null || SelectedGroup == default)
                    {
                        notification.ShowInformation("Выберите группу");
                        return;
                    }

                    List<StudentsGroupsModel> models = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdGroup = '{SelectedGroup.Id}'");

                    if (!(models == null || models == default))
                    {
                        List<Task> delete_list = new();
                        foreach (var item in models)
                        {
                            Task delete_list_task = Task.Run(async () => await api.DeleteModel(item.Id, WebApiTableNames.StudentsGroups));
                            delete_list.Add(delete_list_task);
                        }
                        Task all = Task.WhenAll(delete_list);
                        await all;
                    }

                    await api.DeleteModel(SelectedGroup.Id, WebApiTableNames.Groups);
                }
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                await Loader.Clear();
                aggregator.GetEvent<GroupsUpdatedEvent>().Publish();
                manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
            }
        }
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        #endregion
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsAddingGroup = navigationContext.Parameters.GetValue<bool>("isadding");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
    }
}
