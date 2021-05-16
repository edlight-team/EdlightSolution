using ApplicationEventsWPF.Events;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EdlightDesktopClient.Views.Schedule
{
    /// <summary>
    /// Логика взаимодействия для ScheduleDateViewer.xaml
    /// </summary>
    public partial class ScheduleDateViewer : UserControl
    {
        private bool isMoving = false;
        private bool isResizeUp = false;
        private bool isResizeDown = false;
        private readonly IEventAggregator aggregator;

        public ScheduleDateViewer(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
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

        private void CardMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is HandyControl.Controls.Card card)
            {
                if (isResizeUp) return;
                if (isResizeDown) return;
                card.Opacity = 0.5;

                AdornerCardTemplate.Height = card.Height;
                Adorner.Visibility = Visibility.Visible;
                AdornerTargetBorder.Visibility = Visibility.Visible;
                Adorner.Margin = new Thickness(0, card.Margin.Top, 0, 0);

                isMoving = true;
                DragDrop.DoDragDrop(card, card, DragDropEffects.Move);
            }
        }
        private void UpArrowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentControl cc)
            {
                if (cc.Parent is Grid gr)
                {
                    if (gr.Parent is HandyControl.Controls.Card card)
                    {
                        DownRangeBorder.Margin = new Thickness(0, card.Margin.Top + card.Height, 0, 0);
                        card.Opacity = 0.5;

                        AdornerCardTemplate.Height = card.Height;
                        Adorner.Visibility = Visibility.Visible;
                        AdornerTargetBorder.Visibility = Visibility.Collapsed;

                        isResizeUp = true;
                        DragDrop.DoDragDrop(card, card, DragDropEffects.Copy);
                    }
                }
            }
        }
        private void DownArrowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentControl cc)
            {
                if (cc.Parent is Grid gr)
                {
                    if (gr.Parent is HandyControl.Controls.Card card)
                    {
                        UpRangeBorder.Margin = new Thickness(0, card.Margin.Top, 0, 0);
                        card.Opacity = 0.5;

                        AdornerCardTemplate.Height = card.Height;
                        Adorner.Visibility = Visibility.Visible;
                        AdornerTargetBorder.Visibility = Visibility.Collapsed;

                        isResizeDown = true;
                        DragDrop.DoDragDrop(card, card, DragDropEffects.Copy);
                    }
                }
            }
        }

        private void ScheduleMarkupDragEnter(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(ScheduleMarkup);
            var data = e.Data.GetData("HandyControl.Controls.Card");
            if (isResizeUp)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        if ((card.Margin.Top + card.Height) < position.Y + 17)
                        {
                            e.Effects = DragDropEffects.None;
                            Adorner.Margin = new Thickness(0, card.Margin.Top + card.Height - 17, 0, 0);
                            AdornerCardTemplate.Height = 17;
                        }
                        else
                        {
                            e.Effects = DragDropEffects.Copy;
                            Adorner.Margin = new Thickness(0, position.Y, 0, 0);
                            AdornerCardTemplate.Height = DownRangeBorder.Margin.Top - Adorner.Margin.Top;
                        }
                    }
                }
            }
            else if (isResizeDown)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        if (position.Y < UpRangeBorder.Margin.Top)
                        {
                            e.Effects = DragDropEffects.None;
                            AdornerCardTemplate.Height = 17;
                        }
                        else
                        {
                            e.Effects = DragDropEffects.Copy;
                            AdornerCardTemplate.Height = position.Y - card.Margin.Top;
                        }
                    }
                }
            }
            else if (isMoving)
            {
                if (data != null)
                {
                    Adorner.Margin = new Thickness(0, position.Y, 0, 0);
                }
            }
        }
        private void CardsListScrollDragEnter(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(ScheduleMarkup);
            var data = e.Data.GetData("HandyControl.Controls.Card");
            if (isResizeUp)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        if ((card.Margin.Top + card.Height) < position.Y + 17)
                        {
                            Adorner.Margin = new Thickness(0, card.Margin.Top + card.Height - 17, 0, 0);
                            AdornerCardTemplate.Height = 17;
                        }
                    }
                }
            }
            else if (isResizeDown)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        if (position.Y < UpRangeBorder.Margin.Top)
                        {
                            AdornerCardTemplate.Height = 17;
                        }
                    }
                }
            }
        }
        private void CardsListScrollDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData("HandyControl.Controls.Card");
            if (isResizeUp)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        card.Opacity = 1;
                        card.Margin = new Thickness(0, Adorner.Margin.Top, 0, 0);
                        card.Height = AdornerCardTemplate.Height;
                    }
                    Adorner.Visibility = Visibility.Collapsed;
                }
                isResizeUp = false;
            }
            else if (isResizeDown)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        card.Opacity = 1;
                        card.Height = AdornerCardTemplate.Height;
                    }
                    Adorner.Visibility = Visibility.Collapsed;
                }
                isResizeDown = false;
            }
            else if(isMoving)
            {
                if (data != null)
                {
                    if (data is HandyControl.Controls.Card card)
                    {
                        card.Opacity = 1;
                        card.Margin = new Thickness(0, Adorner.Margin.Top, 0, 0);
                    }
                    Adorner.Visibility = Visibility.Collapsed;
                }
                isMoving = false;
            }
        }
    }
}
