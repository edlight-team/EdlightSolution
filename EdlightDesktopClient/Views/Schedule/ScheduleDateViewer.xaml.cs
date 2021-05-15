using ApplicationEventsWPF.Events;
using Prism.Events;
using System.Windows.Controls;

namespace EdlightDesktopClient.Views.Schedule
{
    /// <summary>
    /// Логика взаимодействия для ScheduleDateViewer.xaml
    /// </summary>
    public partial class ScheduleDateViewer : UserControl
    {
        public ScheduleDateViewer(IEventAggregator aggregator)
        {
            InitializeComponent();
            aggregator.GetEvent<GridChildChangedEvent>().Subscribe(OnGridChildEvent);
        }
        private void OnGridChildEvent(object child)
        {
            if (child == null) ItemsGrid.Children.Clear();
            else
            {
                if (child is HandyControl.Controls.Card card)
                {
                    ItemsGrid.Children.Add(card);
                }
            }
        }
        private void CardsListScrollChanged(object sender, ScrollChangedEventArgs e) => TimeZonesScroll.ScrollToVerticalOffset(e.VerticalOffset);
        private void TimeZonesScrollChanged(object sender, ScrollChangedEventArgs e) => CardsListScroll.ScrollToVerticalOffset(e.VerticalOffset);
    }
}
