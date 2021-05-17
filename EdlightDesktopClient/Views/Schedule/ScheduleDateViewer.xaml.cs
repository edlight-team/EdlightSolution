using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using HandyControl.Controls;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EdlightDesktopClient.Views.Schedule
{
    /// <summary>
    /// Логика взаимодействия для ScheduleDateViewer.xaml
    /// </summary>
    public partial class ScheduleDateViewer : UserControl
    {
        #region fields and services

        private readonly IEventAggregator aggregator;
        private bool isMoving = false;
        private bool isResizeUp = false;
        private bool isResizeDown = false;
        private const int rowSize = 17;

        private readonly DoubleAnimation zoneShowAnimation;
        private readonly DoubleAnimation zoneHideAnimation;
        private readonly DoubleAnimation cardShowAnimation;
        private readonly DoubleAnimation cardHideAnimation;
        private readonly DoubleAnimation adornerShowAnimation;
        private readonly DoubleAnimation adornerHideAnimation;

        #endregion
        #region Конструктор и выгрузка

        public ScheduleDateViewer(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            InitializeComponent();
            Unloaded += ScheduleDateViewerUnloaded;
            aggregator.GetEvent<GridChildChangedEvent>().Subscribe(OnGridChildEvent);

            zoneShowAnimation = new(0.0, 0.5, TimeSpan.FromMilliseconds(500));
            zoneHideAnimation = new(0.5, 0.0, TimeSpan.FromMilliseconds(500));
            cardShowAnimation = new(0.5, 1.0, TimeSpan.FromMilliseconds(250));
            cardHideAnimation = new(1.0, 0.5, TimeSpan.FromMilliseconds(250));
            adornerShowAnimation = new(0.0, 1.0, TimeSpan.FromMilliseconds(250));
            adornerHideAnimation = new(1.0, 0.0, TimeSpan.FromMilliseconds(250));
        }
        private void ScheduleDateViewerUnloaded(object sender, RoutedEventArgs e)
        {
            aggregator.GetEvent<GridChildChangedEvent>().Unsubscribe(OnGridChildEvent);
        }

        #endregion
        #region Создание карточек

        /// <summary>
        /// Ивент создающий карточку
        /// </summary>
        /// <param name="child">Карточка из ивента</param>
        private void OnGridChildEvent(object[] childAndModel)
        {
            if (childAndModel == null) ItemsGrid.Children.Clear();
            else
            {
                if (childAndModel[0] is Card card && childAndModel[1] is LessonsModel lm)
                {
                    //Указываем обратный порядок чтобы сетка с dnd была сверху
                    Grid content = new();
                    content.Children.Add(CreateCardBigInfoGrid(card, lm));
                    content.Children.Add(CreateCardLargeInfoGrid(card, lm));
                    content.Children.Add(CreateCardSmallInfoGrid(card, lm));
                    content.Children.Add(CreateDragAndDropGrid());
                    //Сбрасываем фоновый цвет т.к. используется в сетках
                    card.Background = new SolidColorBrush(Colors.Transparent);
                    //Заполняем главный контрол созданной карточкой
                    card.Content = content;
                    CheckGridVisibilities(card);
                    ItemsGrid.Children.Add(card);
                }
            }
        }
        /// <summary>
        /// Большая сетка
        /// </summary>
        /// <param name="card">Карточка</param>
        /// <returns>Сетка</returns>
        private Grid CreateCardBigInfoGrid(Card card, LessonsModel lm)
        {
            Grid infoGrid = new();
            infoGrid.Name = "BigInfo";

            ColumnDefinition smallColumn1 = new();
            smallColumn1.Width = new GridLength(15);
            ColumnDefinition smallColumn2 = new();
            smallColumn2.Width = new GridLength(15);

            infoGrid.ColumnDefinitions.Add(smallColumn1);
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(smallColumn2);

            RowDefinition smallRow = new();
            smallRow.Height = new GridLength(35);
            RowDefinition row1 = new();
            row1.Height = new GridLength(40);
            RowDefinition row2 = new();
            row2.Height = new GridLength(40);
            RowDefinition row3 = new();
            row3.Height = new GridLength(40);
            RowDefinition row4 = new();
            row4.Height = new GridLength(40);
            RowDefinition endRow = new();
            endRow.Height = new GridLength(15);

            infoGrid.RowDefinitions.Add(smallRow);
            infoGrid.RowDefinitions.Add(row1);
            infoGrid.RowDefinitions.Add(row2);
            infoGrid.RowDefinitions.Add(row3);
            infoGrid.RowDefinitions.Add(row4);
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(endRow);

            Border border = new();
            border.CornerRadius = new CornerRadius(5);
            //border.Background = card.Background;
            border.Background = new SolidColorBrush(Colors.Blue);

            Grid.SetColumn(border, 0);
            Grid.SetRow(border, 0);
            Grid.SetColumnSpan(border, 4);
            Grid.SetRowSpan(border, 7);

            infoGrid.Children.Add(border);

            return infoGrid;
        }
        /// <summary>
        /// Средняя сетка
        /// </summary>
        /// <param name="card">Карточка</param>
        /// <returns>Сетка</returns>
        private Grid CreateCardLargeInfoGrid(Card card, LessonsModel lm)
        {
            Grid infoGrid = new();
            infoGrid.Name = "LargeInfo";

            ColumnDefinition smallColumn1 = new();
            smallColumn1.Width = new GridLength(15);
            ColumnDefinition smallColumn2 = new();
            smallColumn2.Width = new GridLength(15);

            infoGrid.ColumnDefinitions.Add(smallColumn1);
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(smallColumn2);

            RowDefinition smallRow = new();
            smallRow.Height = new GridLength(35);
            RowDefinition endRow = new();
            endRow.Height = new GridLength(15);

            infoGrid.RowDefinitions.Add(smallRow);
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(endRow);

            Border border = new();
            border.CornerRadius = new CornerRadius(5);
            border.Background = card.Background;

            Grid.SetColumn(border, 0);
            Grid.SetRow(border, 0);
            Grid.SetColumnSpan(border, 4);
            Grid.SetRowSpan(border, 6);

            infoGrid.Children.Add(border);
            infoGrid.ShowGridLines = true;

            return infoGrid;
        }
        /// <summary>
        /// Маленькая сетка
        /// </summary>
        /// <param name="card">Карточка</param>
        /// <returns>Сетка</returns>
        private Grid CreateCardSmallInfoGrid(Card card, LessonsModel lm)
        {
            #region Ищем ресурсы

            Collection<ResourceDictionary> merged = Application.Current.Resources.MergedDictionaries;

            ResourceDictionary textBlocks = null;
            ResourceDictionary brushes = null;
            ResourceDictionary svg = null;

            Style simpleTextBlockStyle = null;
            Style toolTipTextBlockStyle = null;

            ControlTemplate classTypeTemplate = null;
            ControlTemplate teacherTemplate = null;
            ControlTemplate doorTemplate = null;
            ControlTemplate bookTemplate = null;
            ControlTemplate groupTemplate = null;
            ControlTemplate commentTemplate = null;

            SolidColorBrush primaryBrush = null;
            SolidColorBrush innerBrush = null;

            foreach (ResourceDictionary rd in merged)
            {
                if (rd.Source == null) continue;
                if (rd.Source.OriginalString.Contains("TextBlock")) textBlocks = rd;
                else if (rd.Source.OriginalString.Contains("SolidBrushes")) brushes = rd;
                else if (rd.Source.OriginalString.Contains("SVGCollection")) svg = rd;
            }
            foreach (var key in textBlocks.Keys)
            {
                if (key.ToString() == "SimpleTextBlock")
                {
                    simpleTextBlockStyle = (Style)textBlocks[key];
                }
                if (key.ToString() == "ToolTipTextBlock")
                {
                    toolTipTextBlockStyle = (Style)textBlocks[key];
                }
            }
            foreach (var key in brushes.Keys)
            {
                if (key.ToString() == "InactiveBackgroundTabHeaderBtush")
                {
                    innerBrush = (SolidColorBrush)brushes[key];
                }
                else if (key.ToString() == "PrimaryFontBrush")
                {
                    primaryBrush = (SolidColorBrush)brushes[key];
                }
            }
            foreach (var key in svg.Keys)
            {
                if (key.ToString() == "ClassType")
                {
                    classTypeTemplate = (ControlTemplate)svg[key];
                }
                else if (key.ToString() == "Teacher")
                {
                    teacherTemplate = (ControlTemplate)svg[key];
                }
                else if (key.ToString() == "Door")
                {
                    doorTemplate = (ControlTemplate)svg[key];
                }
                else if (key.ToString() == "Book")
                {
                    bookTemplate = (ControlTemplate)svg[key];
                }
                else if (key.ToString() == "Group")
                {
                    groupTemplate = (ControlTemplate)svg[key];
                }
                else if (key.ToString() == "Comment")
                {
                    commentTemplate = (ControlTemplate)svg[key];
                }
            }

            #endregion

            Grid infoGrid = new();
            infoGrid.Name = "SmallInfo";

            ColumnDefinition smallColumn1 = new();
            smallColumn1.Width = new GridLength(15);
            ColumnDefinition iconColumn = new();
            iconColumn.Width = new GridLength(45);
            ColumnDefinition smallColumn2 = new();
            smallColumn2.Width = new GridLength(15);

            infoGrid.ColumnDefinitions.Add(smallColumn1);
            infoGrid.ColumnDefinitions.Add(iconColumn);
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(smallColumn2);

            RowDefinition smallRow1 = new();
            smallRow1.Height = new GridLength(15);
            RowDefinition smallRow2 = new();
            smallRow2.Height = new GridLength(15);

            infoGrid.RowDefinitions.Add(smallRow1);
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(smallRow2);

            Border border = new();
            border.Name = "SmallGridBorder";
            border.CornerRadius = new CornerRadius(5);
            border.Background = card.Background;

            Border innerBorder = new();
            innerBorder.CornerRadius = new CornerRadius(5);
            innerBorder.Background = innerBrush;

            Grid.SetColumn(border, 0);
            Grid.SetRow(border, 0);
            Grid.SetColumnSpan(border, 5);
            Grid.SetRowSpan(border, 3);

            Grid.SetColumn(innerBorder, 1);
            Grid.SetRow(innerBorder, 1);
            Grid.SetColumnSpan(innerBorder, 3);

            infoGrid.Children.Add(border);
            infoGrid.Children.Add(innerBorder);

            ContentControl classType = new();
            classType.Width = 25;
            classType.Margin = new Thickness(10, 0, 0, 0);
            classType.Foreground = primaryBrush;
            classType.Template = classTypeTemplate;
            classType.HorizontalAlignment = HorizontalAlignment.Left;
            classType.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetColumn(classType, 1);
            Grid.SetRow(classType, 1);

            infoGrid.Children.Add(classType);

            TextBlock disciplineTextBlock = new();
            classType.Margin = new Thickness(15, 0, 0, 0);
            disciplineTextBlock.Text = lm.TypeClass?.Title;
            disciplineTextBlock.Style = simpleTextBlockStyle;
            disciplineTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            disciplineTextBlock.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetColumn(disciplineTextBlock, 2);
            Grid.SetRow(disciplineTextBlock, 1);

            infoGrid.Children.Add(disciplineTextBlock);

            #region Иконки с тултипами

            StackPanel controlsStack = new();
            controlsStack.Orientation = Orientation.Horizontal;
            controlsStack.HorizontalAlignment = HorizontalAlignment.Center;
            controlsStack.VerticalAlignment = VerticalAlignment.Center;
            controlsStack.Margin = new Thickness(10, 0, 0, 0);

            Grid.SetColumn(controlsStack, 3);
            Grid.SetRow(controlsStack, 1);

            #region Преподаватель

            ContentControl teacherControl = new();
            teacherControl.Width = 30;
            teacherControl.Margin = new Thickness(0, 0, 5, 0);
            teacherControl.Foreground = primaryBrush;
            teacherControl.Template = teacherTemplate;
            teacherControl.HorizontalAlignment = HorizontalAlignment.Left;
            teacherControl.VerticalAlignment = VerticalAlignment.Center;

            TextBlock teacherToolTipTextBlock = new();
            teacherToolTipTextBlock.Style = toolTipTextBlockStyle;
            teacherToolTipTextBlock.Text = lm.Teacher.FullName;

            ToolTip teacherToolTip = new();
            teacherToolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            teacherToolTip.Content = teacherToolTipTextBlock;
            teacherControl.ToolTip = teacherToolTip;

            controlsStack.Children.Add(teacherControl);

            #endregion
            #region Аудитория

            ContentControl audienceControl = new();
            audienceControl.Width = 30;
            audienceControl.Margin = new Thickness(0, 0, 5, 0);
            audienceControl.Foreground = primaryBrush;
            audienceControl.Template = doorTemplate;
            audienceControl.HorizontalAlignment = HorizontalAlignment.Left;
            audienceControl.VerticalAlignment = VerticalAlignment.Center;

            TextBlock audienceToolTipTextBlock = new();
            audienceToolTipTextBlock.Style = toolTipTextBlockStyle;
            audienceToolTipTextBlock.Text = lm.Audience?.NumberAudience;

            ToolTip audienceToolTip = new();
            audienceToolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            audienceToolTip.Content = audienceToolTipTextBlock;
            audienceControl.ToolTip = audienceToolTip;

            controlsStack.Children.Add(audienceControl);

            #endregion
            #region Дисциплина

            ContentControl academicDisciplineControl = new();
            academicDisciplineControl.Width = 45;
            academicDisciplineControl.Margin = new Thickness(0, 0, 5, 0);
            academicDisciplineControl.Foreground = primaryBrush;
            academicDisciplineControl.Template = bookTemplate;
            academicDisciplineControl.HorizontalAlignment = HorizontalAlignment.Left;
            academicDisciplineControl.VerticalAlignment = VerticalAlignment.Center;

            TextBlock academicDisciplineToolTipTextBlock = new();
            academicDisciplineToolTipTextBlock.Style = toolTipTextBlockStyle;
            academicDisciplineToolTipTextBlock.Text = lm.AcademicDiscipline?.Title;

            ToolTip academicDisciplineToolTip = new();
            academicDisciplineToolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            academicDisciplineToolTip.Content = academicDisciplineToolTipTextBlock;
            academicDisciplineControl.ToolTip = academicDisciplineToolTip;

            controlsStack.Children.Add(academicDisciplineControl);

            #endregion
            #region Группа

            ContentControl groupControl = new();
            groupControl.Width = 40;
            groupControl.Margin = new Thickness(0, 0, 5, 0);
            groupControl.Foreground = primaryBrush;
            groupControl.Template = groupTemplate;
            groupControl.HorizontalAlignment = HorizontalAlignment.Left;
            groupControl.VerticalAlignment = VerticalAlignment.Center;

            TextBlock groupToolTipTextBlock = new();
            groupToolTipTextBlock.Style = toolTipTextBlockStyle;
            groupToolTipTextBlock.Text = lm.Group?.Group;

            ToolTip groupToolTip = new();
            groupToolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            groupToolTip.Content = groupToolTipTextBlock;
            groupControl.ToolTip = groupToolTip;

            controlsStack.Children.Add(groupControl);

            #endregion
            #region Комменты

            ContentControl commentControl = new();
            commentControl.Width = 30;
            commentControl.Margin = new Thickness(0, 0, 5, 0);
            commentControl.Foreground = primaryBrush;
            commentControl.Template = commentTemplate;
            commentControl.HorizontalAlignment = HorizontalAlignment.Left;
            commentControl.VerticalAlignment = VerticalAlignment.Center;

            //TextBlock groupToolTipTextBlock = new();
            //groupToolTipTextBlock.Style = toolTipTextBlockStyle;
            //groupToolTipTextBlock.Text = lm.Group?.Group;

            //ToolTip groupToolTip = new();
            //groupToolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            //groupToolTip.Content = groupToolTipTextBlock;
            //commentControl.ToolTip = groupToolTip;

            controlsStack.Children.Add(commentControl);

            #endregion

            infoGrid.Children.Add(controlsStack);

            #endregion

            return infoGrid;
        }
        /// <summary>
        /// Сетка с Drag'n'Drop методами
        /// </summary>
        /// <returns>Сетка</returns>
        private Grid CreateDragAndDropGrid()
        {
            Grid subGrid = new();

            ColumnDefinition smallColumn1 = new();
            smallColumn1.Width = new GridLength(15);
            ColumnDefinition smallColumn2 = new();
            smallColumn2.Width = new GridLength(15);

            subGrid.ColumnDefinitions.Add(smallColumn1);
            subGrid.ColumnDefinitions.Add(new ColumnDefinition());
            subGrid.ColumnDefinitions.Add(smallColumn2);

            RowDefinition smallRow = new();
            smallRow.Height = new GridLength(35);
            RowDefinition endRow = new();
            endRow.Height = new GridLength(15);

            subGrid.RowDefinitions.Add(smallRow);
            subGrid.RowDefinitions.Add(new RowDefinition());
            subGrid.RowDefinitions.Add(endRow);

            #region Ищем ресурсы

            Collection<ResourceDictionary> merged = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary borders = null;
            ResourceDictionary svg = null;

            foreach (ResourceDictionary rd in merged)
            {
                if (rd.Source == null) continue;
                if (rd.Source.OriginalString.Contains("Borders")) borders = rd;
                else if (rd.Source.OriginalString.Contains("SVGCollection")) svg = rd;
            }

            #endregion
            #region Прозрачная рамка

            foreach (var key in borders.Keys)
            {
                if (key.ToString() == "VisibleMouseOverGrid")
                {
                    subGrid.Style = (Style)borders[key];
                }
            }

            Border transparent_border_up = new();
            transparent_border_up.Background = new SolidColorBrush(Colors.Transparent);
            Grid.SetRow(transparent_border_up, 0);
            Grid.SetColumn(transparent_border_up, 0);
            Grid.SetColumnSpan(transparent_border_up, 3);

            Border transparent_border_down = new();
            transparent_border_down.Background = new SolidColorBrush(Colors.Transparent);
            Grid.SetRow(transparent_border_down, 3);
            Grid.SetColumn(transparent_border_down, 0);
            Grid.SetColumnSpan(transparent_border_down, 3);

            subGrid.Children.Add(transparent_border_up);
            subGrid.Children.Add(transparent_border_down);

            #endregion
            #region Контрол передвигающий карту

            ContentControl move_control = new();
            move_control.Height = 12;
            move_control.Margin = new Thickness(3);
            move_control.HorizontalAlignment = HorizontalAlignment.Left;
            move_control.VerticalAlignment = VerticalAlignment.Top;
            move_control.Cursor = Cursors.SizeAll;
            foreach (var key in svg.Keys)
            {
                if (key.ToString() == "Move")
                {
                    move_control.Template = (ControlTemplate)svg[key];
                }
            }
            move_control.MouseDown += CardMouseDown;
            subGrid.Children.Add(move_control);

            #endregion
            #region Контрол увеличивающий вверх

            ContentControl up_arrow = new();
            up_arrow.Height = 12;
            up_arrow.Margin = new Thickness(3);
            up_arrow.HorizontalAlignment = HorizontalAlignment.Right;
            up_arrow.VerticalAlignment = VerticalAlignment.Top;
            up_arrow.Cursor = Cursors.SizeNS;
            foreach (var key in svg.Keys)
            {
                if (key.ToString() == "Up")
                {
                    up_arrow.Template = (ControlTemplate)svg[key];
                }
            }
            up_arrow.MouseDown += UpArrowMouseDown;

            Grid.SetRow(up_arrow, 0);
            Grid.SetColumn(up_arrow, 3);

            subGrid.Children.Add(up_arrow);

            #endregion
            #region Контрол увеличивающий вниз

            ContentControl down_arrow = new();
            down_arrow.Height = 12;
            down_arrow.Margin = new Thickness(3);
            down_arrow.HorizontalAlignment = HorizontalAlignment.Right;
            down_arrow.VerticalAlignment = VerticalAlignment.Bottom;
            down_arrow.Cursor = Cursors.SizeNS;
            foreach (var key in svg.Keys)
            {
                if (key.ToString() == "Down")
                {
                    down_arrow.Template = (ControlTemplate)svg[key];
                }
            }
            down_arrow.MouseDown += DownArrowMouseDown;

            Grid.SetRow(down_arrow, 3);
            Grid.SetColumn(down_arrow, 3);

            subGrid.Children.Add(down_arrow);

            #endregion
            return subGrid;
        }

        #endregion
        #region Метод проверки видимости сеток

        private void CheckGridVisibilities(Card card)
        {
            Grid smallGrid = null;
            Grid largeGrid = null;
            Grid bigGrid = null;

            if (card.Content is Grid mgr)
            {
                foreach (var child in mgr.Children)
                {
                    if (child is Grid ch_gr)
                    {
                        if (ch_gr.Name == "SmallInfo") smallGrid = ch_gr;
                        else if (ch_gr.Name == "LargeInfo") largeGrid = ch_gr;
                        else if (ch_gr.Name == "BigInfo") bigGrid = ch_gr;
                    }
                }
            }
            smallGrid.Visibility = Visibility.Collapsed;
            largeGrid.Visibility = Visibility.Collapsed;
            bigGrid.Visibility = Visibility.Collapsed;

            if (card.Height < rowSize * 7) smallGrid.Visibility = Visibility.Visible;
            if (card.Height <= rowSize * 7 || card.Height <= rowSize * 14) largeGrid.Visibility = Visibility.Visible;
            if (card.Height > rowSize * 14) bigGrid.Visibility = Visibility.Visible;
        }

        #endregion
        #region Одновременный скролл

        private void CardsListScrollChanged(object sender, ScrollChangedEventArgs e) => TimeZonesScroll.ScrollToVerticalOffset(e.VerticalOffset);
        private void TimeZonesScrollChanged(object sender, ScrollChangedEventArgs e) => CardsListScroll.ScrollToVerticalOffset(e.VerticalOffset);

        #endregion
        #region Перемещение и растягивание карточек

        private Brush FindBorderBackgroundBrush(Grid source)
        {
            foreach (var ch in source.Children)
            {
                if (ch is Grid ch_gr && ch_gr.Name == "SmallInfo")
                {
                    foreach (var sub_ch in ch_gr.Children)
                    {
                        if (sub_ch is Border brd && brd.Name == "SmallGridBorder")
                        {
                            return brd.Background;
                        }
                    }
                }
            }
            return null;
        }
        private void CardMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isResizeUp) return;
            if (isResizeDown) return;
            if (isMoving) return;
            if (sender is ContentControl cc)
            {
                if (cc.Parent is Grid gr)
                {
                    if (gr.Parent is Grid pgr)
                    {
                        Brush borderBrush = FindBorderBackgroundBrush(pgr);
                        if (pgr.Parent is Card card)
                        {
                            card.BeginAnimation(OpacityProperty, cardHideAnimation);

                            AdornerCardTemplate.Height = card.Height;
                            AdornerCardTemplate.Background = borderBrush;

                            AdornerTargetBorder.Visibility = Visibility.Visible;
                            Adorner.BeginAnimation(OpacityProperty, adornerShowAnimation);
                            Adorner.Margin = new Thickness(0, card.Margin.Top, 0, 0);

                            isMoving = true;
                            AdornerZoneLeft.BeginAnimation(OpacityProperty, zoneShowAnimation);

                            DragDrop.DoDragDrop(card, card, DragDropEffects.Move);

                            AdornerTargetBorder.Visibility = Visibility.Collapsed;
                            card.BeginAnimation(OpacityProperty, cardShowAnimation);
                            Adorner.BeginAnimation(OpacityProperty, adornerHideAnimation);
                            AdornerZoneLeft.BeginAnimation(OpacityProperty, zoneHideAnimation);
                            isMoving = false;
                        }
                    }
                }
            }
        }
        private void UpArrowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentControl cc)
            {
                if (cc.Parent is Grid gr)
                {
                    if (gr.Parent is Grid pgr)
                    {
                        Brush borderBrush = FindBorderBackgroundBrush(pgr);
                        if (pgr.Parent is Card card)
                        {
                            DownRangeBorder.Margin = new Thickness(0, card.Margin.Top + card.Height, 0, 0);
                            card.BeginAnimation(OpacityProperty, cardHideAnimation);

                            Adorner.Margin = new Thickness(0, card.Margin.Top, 0, 0);
                            AdornerTargetBorder.Visibility = Visibility.Collapsed;
                            AdornerCardTemplate.Background = borderBrush;
                            AdornerCardTemplate.Height = DownRangeBorder.Margin.Top - Adorner.Margin.Top;
                            Adorner.BeginAnimation(OpacityProperty, adornerShowAnimation);
                            AdornerZoneRight.BeginAnimation(OpacityProperty, zoneShowAnimation);

                            isResizeUp = true;
                            DragDrop.DoDragDrop(card, card, DragDropEffects.Copy);

                            card.BeginAnimation(OpacityProperty, cardShowAnimation);
                            Adorner.BeginAnimation(OpacityProperty, adornerHideAnimation);
                            AdornerZoneRight.BeginAnimation(OpacityProperty, zoneHideAnimation);
                            isResizeUp = false;
                        }
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
                    if (gr.Parent is Grid pgr)
                    {
                        Brush borderBrush = FindBorderBackgroundBrush(pgr);
                        if (pgr.Parent is Card card)
                        {
                            UpRangeBorder.Margin = new Thickness(0, card.Margin.Top + (rowSize * 4), 0, 0);
                            card.BeginAnimation(OpacityProperty, cardHideAnimation);

                            AdornerTargetBorder.Visibility = Visibility.Collapsed;
                            AdornerCardTemplate.Background = borderBrush;
                            AdornerCardTemplate.Height = card.Height;
                            Adorner.Margin = new Thickness(0, card.Margin.Top, 0, 0);
                            Adorner.BeginAnimation(OpacityProperty, adornerShowAnimation);
                            AdornerZoneRight.BeginAnimation(OpacityProperty, zoneShowAnimation);

                            isResizeDown = true;
                            DragDrop.DoDragDrop(card, card, DragDropEffects.Copy);

                            card.BeginAnimation(OpacityProperty, cardShowAnimation);
                            Adorner.BeginAnimation(OpacityProperty, adornerHideAnimation);
                            AdornerZoneRight.BeginAnimation(OpacityProperty, zoneHideAnimation);
                            isResizeDown = false;
                        }
                    }
                }
            }
        }
        private void ScheduleMarkupDragEnter(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(ScheduleMarkup);
            var data = e.Data.GetData("HandyControl.Controls.Card");
            if (data == null) return;
            if (data is Card card)
            {
                if (isResizeUp)
                {
                    if ((card.Margin.Top + card.Height) < position.Y + (rowSize * 4))
                    {
                        Adorner.Margin = new Thickness(0, card.Margin.Top + card.Height - (rowSize * 4), 0, 0);
                        AdornerCardTemplate.Height = rowSize * 4;
                    }
                    else
                    {
                        Adorner.Margin = new Thickness(0, position.Y, 0, 0);
                        AdornerCardTemplate.Height = DownRangeBorder.Margin.Top - Adorner.Margin.Top;
                    }
                }
                else if (isResizeDown)
                {
                    if (position.Y < UpRangeBorder.Margin.Top)
                    {
                        AdornerCardTemplate.Height = rowSize * 4;
                    }
                    else
                    {
                        AdornerCardTemplate.Height = position.Y - card.Margin.Top;
                    }
                }
                else if (isMoving)
                {
                    Adorner.Margin = new Thickness(0, position.Y, 0, 0);
                }
            }
        }
        private void CardsListScrollDrop(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(ScheduleMarkup);
            var data = e.Data.GetData("HandyControl.Controls.Card");
            if (data == null) return;
            if (data is Card card)
            {
                if (isResizeUp)
                {
                    int index = (int)Math.Truncate(Math.Round(Adorner.Margin.Top / rowSize));
                    int top = index * rowSize;

                    int indexOfHeight = (int)Math.Round(Math.Round(AdornerCardTemplate.Height) / rowSize);
                    double height = indexOfHeight * rowSize;

                    card.Margin = new Thickness(0, top, 0, 0);
                    card.Height = height;
                }
                else if (isResizeDown)
                {
                    int indexOfHeight = (int)Math.Round(Math.Round(AdornerCardTemplate.Height) / rowSize);
                    double height = indexOfHeight * rowSize;

                    card.Height = height;
                }
                else if (isMoving)
                {
                    int index = (int)Math.Round(Math.Round(Adorner.Margin.Top / rowSize));
                    int top = index * rowSize;
                    card.Margin = new Thickness(0, top, 0, 0);
                }
                CheckGridVisibilities(card);
                aggregator.GetEvent<CardMoveOrResizeEvent>().Publish(card);
            }
        }

        #endregion
    }
}
