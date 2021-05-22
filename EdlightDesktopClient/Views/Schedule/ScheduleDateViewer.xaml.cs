using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationModels.Models;
using ApplicationWPFServices.MemoryService;
using EdlightDesktopClient.AccessConfigurations;
using HandyControl.Controls;
using Prism.Events;
using Styles.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace EdlightDesktopClient.Views.Schedule
{
    /// <summary>
    /// Логика взаимодействия для ScheduleDateViewer.xaml
    /// </summary>
    public partial class ScheduleDateViewer : UserControl
    {
        #region resources

        private readonly ResourceDictionary textBlocks = null;
        private readonly ResourceDictionary brushes = null;
        private readonly ResourceDictionary svg = null;

        private readonly Style elementTextBlockStyle = null;
        private readonly Style simpleTextBlockStyle = null;
        private readonly Style toolTipTextBlockStyle = null;

        private readonly ControlTemplate cancelTemplate = null;
        private readonly ControlTemplate classTypeTemplate = null;
        private readonly ControlTemplate teacherTemplate = null;
        private readonly ControlTemplate doorTemplate = null;
        private readonly ControlTemplate bookTemplate = null;
        private readonly ControlTemplate groupTemplate = null;
        private readonly ControlTemplate commentTemplate = null;

        private readonly SolidColorBrush falseOrErrorBrush = null;
        private readonly SolidColorBrush primaryBrush = null;
        private readonly SolidColorBrush elementFontBrush = null;
        private readonly SolidColorBrush innerBrush = null;

        #endregion
        #region fields and services

        private readonly IEventAggregator aggregator;
        private readonly IMemoryService memory;
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

        public ScheduleDateViewer(IEventAggregator aggregator, IMemoryService memory)
        {
            this.aggregator = aggregator;
            this.memory = memory;
            InitializeComponent();
            Loaded += ScheduleDateViewerLoaded;
            Unloaded += ScheduleDateViewerUnloaded;
            aggregator.GetEvent<GridChildChangedEvent>().Subscribe(OnGridChildEvent);
            aggregator.GetEvent<CommentsChangedEvent>().Subscribe(OnCardCommentsChanged);

            zoneShowAnimation = new(0.0, 0.5, TimeSpan.FromMilliseconds(500));
            zoneHideAnimation = new(0.5, 0.0, TimeSpan.FromMilliseconds(500));
            cardShowAnimation = new(0.5, 1.0, TimeSpan.FromMilliseconds(250));
            cardHideAnimation = new(1.0, 0.5, TimeSpan.FromMilliseconds(250));
            adornerShowAnimation = new(0.0, 1.0, TimeSpan.FromMilliseconds(250));
            adornerHideAnimation = new(1.0, 0.0, TimeSpan.FromMilliseconds(250));

            #region Ищем ресурсы

            Collection<ResourceDictionary> merged = Application.Current.Resources.MergedDictionaries;

            foreach (ResourceDictionary rd in merged)
            {
                if (rd.Source == null) continue;
                if (rd.Source.OriginalString.Contains("TextBlock")) textBlocks = rd;
                else if (rd.Source.OriginalString.Contains("SolidBrushes")) brushes = rd;
                else if (rd.Source.OriginalString.Contains("SVGCollection")) svg = rd;
            }
            foreach (object key in textBlocks.Keys)
            {
                if (key.ToString() == "ElementTextBlock") elementTextBlockStyle = (Style)textBlocks[key];
                else if (key.ToString() == "ToolTipTextBlock") toolTipTextBlockStyle = (Style)textBlocks[key];
                else if (key.ToString() == "SimpleTextBlock") simpleTextBlockStyle = (Style)textBlocks[key];
            }
            foreach (object key in brushes.Keys)
            {
                if (key.ToString() == "FalseOrErrorBrush") falseOrErrorBrush = (SolidColorBrush)brushes[key];
                else if (key.ToString() == "InactiveBackgroundTabHeaderBtush") innerBrush = (SolidColorBrush)brushes[key];
                else if (key.ToString() == "PrimaryFontBrush") primaryBrush = (SolidColorBrush)brushes[key];
                else if (key.ToString() == "ElementFontBrush") elementFontBrush = (SolidColorBrush)brushes[key];
            }
            foreach (object key in svg.Keys)
            {
                if (key.ToString() == "Cancel") cancelTemplate = (ControlTemplate)svg[key];
                else if (key.ToString() == "ClassType") classTypeTemplate = (ControlTemplate)svg[key];
                else if (key.ToString() == "Teacher") teacherTemplate = (ControlTemplate)svg[key];
                else if (key.ToString() == "Door") doorTemplate = (ControlTemplate)svg[key];
                else if (key.ToString() == "Book") bookTemplate = (ControlTemplate)svg[key];
                else if (key.ToString() == "Group") groupTemplate = (ControlTemplate)svg[key];
                else if (key.ToString() == "Comment") commentTemplate = (ControlTemplate)svg[key];
            }

            #endregion
        }
        private void ScheduleDateViewerLoaded(object sender, RoutedEventArgs e) => OnClearEffects();
        private void ScheduleDateViewerUnloaded(object sender, RoutedEventArgs e)
        {
            aggregator.GetEvent<GridChildChangedEvent>().Unsubscribe(OnGridChildEvent);
            aggregator.GetEvent<CommentsChangedEvent>().Unsubscribe(OnCardCommentsChanged);
        }

        #endregion
        #region Создание карточек

        /// <summary>
        /// Ивент создающий карточку
        /// </summary>
        /// <param name="child">Карточка из ивента</param>
        private void OnGridChildEvent(object[] childAndModel)
        {
            if (childAndModel == null)
            {
                ItemsGrid.Children.Clear();
            }
            else if (childAndModel[0] is Card card && childAndModel[1] is LessonsModel lm && childAndModel[2] is IEnumerable<CommentModel> comments)
            {
                //Указываем обратный порядок чтобы сетка с dnd была сверху
                Grid content = new();
                content.Children.Add(CreateCardBigInfoGrid(card, lm, comments.ToList()));
                content.Children.Add(CreateCardLargeInfoGrid(card, lm, comments.ToList()));
                content.Children.Add(CreateCardSmallInfoGrid(card, lm, comments.ToList()));
                content.Children.Add(CreateDragAndDropGrid());
                //Сбрасываем фоновый цвет т.к. используется в сетках
                card.Background = new SolidColorBrush(Colors.Transparent);
                //Заполняем главный контрол созданной карточкой
                card.Content = content;
                CheckGridVisibilities(card);
                card.MouseDoubleClick += CardMouseDoubleClick;
                ItemsGrid.Children.Add(card);
            }
            else if (childAndModel[0] is LessonsModel dlm && childAndModel[1] is bool deleting)
            {
                for (int i = 0; i < ItemsGrid.Children.Count; i++)
                {
                    if (ItemsGrid.Children[i] is Card delete_card && delete_card.Uid == dlm.Id.ToString().ToUpper())
                    {
                        ItemsGrid.Children.Remove(delete_card);
                    }
                }
            }
        }

        /// <summary>
        /// Большая сетка
        /// </summary>
        /// <param name="card">Карточка</param>
        /// <returns>Сетка</returns>
        private Grid CreateCardBigInfoGrid(Card card, LessonsModel lm, List<CommentModel> commentsList)
        {
            #region Объявление сетки

            Grid infoGrid = new();
            infoGrid.Name = "BigInfo";

            #endregion
            #region Разметка

            ColumnDefinition smallColumn1 = new();
            smallColumn1.Width = new GridLength(15);
            ColumnDefinition controlIconCollumn = new();
            controlIconCollumn.Width = new GridLength(80);
            ColumnDefinition commentIconCollumn = new();
            commentIconCollumn.Width = new GridLength(80);
            ColumnDefinition smallColumn2 = new();
            smallColumn2.Width = new GridLength(15);

            infoGrid.ColumnDefinitions.Add(smallColumn1);
            infoGrid.ColumnDefinitions.Add(controlIconCollumn);
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(commentIconCollumn);
            infoGrid.ColumnDefinitions.Add(smallColumn2);

            RowDefinition smallRow = new();
            smallRow.Height = new GridLength(15);
            RowDefinition header = new();
            header.Height = new GridLength(40);
            RowDefinition splitRow = new();
            splitRow.Height = new GridLength(15);
            RowDefinition endRow = new();
            endRow.Height = new GridLength(15);

            infoGrid.RowDefinitions.Add(smallRow);
            infoGrid.RowDefinitions.Add(header);
            infoGrid.RowDefinitions.Add(splitRow);
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(endRow);

            #endregion
            #region Рамки

            Border border = new();
            border.CornerRadius = new CornerRadius(5);
            border.Background = card.Background;
            border.SetGridPosition(0, 0, 8, 5);

            Border innerBorder = new();
            innerBorder.CornerRadius = new CornerRadius(5);
            innerBorder.Background = innerBrush;
            innerBorder.SetGridPosition(1, 1, columnSpan: 2);

            Border innerBorderOther = new();
            innerBorderOther.CornerRadius = new CornerRadius(5);
            innerBorderOther.Background = innerBrush;
            innerBorderOther.SetGridPosition(3, 1, 4, 3);

            infoGrid.Children.Add(border);
            infoGrid.Children.Add(innerBorder);
            infoGrid.Children.Add(innerBorderOther);

            #endregion
            #region Название дисциплины

            ContentControl classType;
            if (string.IsNullOrEmpty(lm.CanceledReason))
            {
                classType = CreateControl(classTypeTemplate, null, 25);
                classType.Margin = new Thickness(15, 0, 0, 0);
                classType.SetGridPosition(1, 1);
                infoGrid.Children.Add(classType);
            }
            else
            {
                classType = CreateControl(cancelTemplate, null, 25);
                classType.Margin = new Thickness(15, 0, 0, 0);
                classType.Foreground = falseOrErrorBrush;
                classType.SetGridPosition(1, 1);

                ToolTip cancelTip = CreateToolTip("Занятие было отменено по причине:\r\n" + lm.CanceledReason);
                classType.ToolTip = cancelTip;

                infoGrid.Children.Add(classType);
            }

            TextBlock disciplineTextBlock = CreateTextBlock(lm.TypeClass?.Title);
            disciplineTextBlock.Margin = new Thickness(50, 0, 0, 0);
            disciplineTextBlock.SetGridPosition(1, 2);
            infoGrid.Children.Add(disciplineTextBlock);

            #endregion
            #region Комментарии

            ContentControl comments = CreateControl(commentTemplate, CreateCommentsToolTip(commentsList), 40);
            comments.Name = "GridComments";
            comments.HorizontalAlignment = HorizontalAlignment.Center;
            comments.SetGridPosition(1, 3);
            infoGrid.Children.Add(comments);

            #endregion
            #region Прочее

            ContentControl teacherControl = CreateControl(teacherTemplate, null, 30);
            ContentControl audienceControl = CreateControl(doorTemplate, null, 30);
            ContentControl disciplineControl = CreateControl(bookTemplate, null, 45);
            ContentControl groupControl = CreateControl(groupTemplate, null, 40);

            teacherControl.Margin = new Thickness(15, 0, 0, 0);
            audienceControl.Margin = new Thickness(15, 0, 0, 0);
            disciplineControl.Margin = new Thickness(15, 0, 0, 0);
            groupControl.Margin = new Thickness(15, 0, 0, 0);

            teacherControl.SetGridPosition(3, 1);
            audienceControl.SetGridPosition(4, 1);
            disciplineControl.SetGridPosition(5, 1);
            groupControl.SetGridPosition(6, 1);

            infoGrid.Children.Add(teacherControl);
            infoGrid.Children.Add(audienceControl);
            infoGrid.Children.Add(disciplineControl);
            infoGrid.Children.Add(groupControl);

            TextBlock teacherText = CreateTextBlock(lm.Teacher?.FullName);
            TextBlock audienceText = CreateTextBlock(lm.Audience?.NumberAudience);
            TextBlock disciplineText = CreateTextBlock(lm.AcademicDiscipline?.Title);
            TextBlock groupText = CreateTextBlock(lm.Group?.Group);

            teacherText.SetGridPosition(3, 2);
            audienceText.SetGridPosition(4, 2);
            disciplineText.SetGridPosition(5, 2);
            groupText.SetGridPosition(6, 2);

            infoGrid.Children.Add(teacherText);
            infoGrid.Children.Add(audienceText);
            infoGrid.Children.Add(disciplineText);
            infoGrid.Children.Add(groupText);

            #endregion
            return infoGrid;
        }
        /// <summary>
        /// Средняя сетка
        /// </summary>
        /// <param name="card">Карточка</param>
        /// <returns>Сетка</returns>
        private Grid CreateCardLargeInfoGrid(Card card, LessonsModel lm, List<CommentModel> commentsList)
        {
            #region Объявление сетки

            Grid infoGrid = new();
            infoGrid.Name = "LargeInfo";

            #endregion
            #region Разметка

            ColumnDefinition smallColumn1 = new();
            smallColumn1.Width = new GridLength(15);
            ColumnDefinition commentIconCollumn = new();
            commentIconCollumn.Width = new GridLength(80);
            ColumnDefinition smallColumn2 = new();
            smallColumn2.Width = new GridLength(15);

            infoGrid.ColumnDefinitions.Add(smallColumn1);
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition());
            infoGrid.ColumnDefinitions.Add(commentIconCollumn);
            infoGrid.ColumnDefinitions.Add(smallColumn2);

            RowDefinition smallRow = new();
            smallRow.Height = new GridLength(15);
            RowDefinition headerRow = new();
            headerRow.Height = new GridLength(40);
            RowDefinition endRow = new();
            endRow.Height = new GridLength(15);

            infoGrid.RowDefinitions.Add(smallRow);
            infoGrid.RowDefinitions.Add(headerRow);
            infoGrid.RowDefinitions.Add(new RowDefinition());
            infoGrid.RowDefinitions.Add(endRow);

            #endregion
            #region Рамки

            Border border = new();
            border.CornerRadius = new CornerRadius(5);
            border.Background = card.Background;
            border.SetGridPosition(0, 0, 4, 4);

            Border innerBorder = new();
            innerBorder.CornerRadius = new CornerRadius(5);
            innerBorder.Background = innerBrush;
            innerBorder.SetGridPosition(1, 1);

            infoGrid.Children.Add(border);
            infoGrid.Children.Add(innerBorder);

            #endregion
            #region Название дисциплины

            ContentControl classType;
            if (string.IsNullOrEmpty(lm.CanceledReason))
            {
                classType = CreateControl(classTypeTemplate, null, 25);
                classType.Margin = new Thickness(15, 0, 0, 0);
                classType.SetGridPosition(1, 1);
                infoGrid.Children.Add(classType);
            }
            else
            {
                classType = CreateControl(cancelTemplate, null, 25);
                classType.Margin = new Thickness(15, 0, 0, 0);
                classType.Foreground = falseOrErrorBrush;
                classType.SetGridPosition(1, 1);

                ToolTip cancelTip = CreateToolTip("Занятие было отменено по причине:\r\n" + lm.CanceledReason);
                classType.ToolTip = cancelTip;

                infoGrid.Children.Add(classType);
            }

            TextBlock disciplineTextBlock = CreateTextBlock(lm.TypeClass?.Title);
            disciplineTextBlock.Margin = new Thickness(50, 0, 0, 0);
            disciplineTextBlock.SetGridPosition(1, 1);
            infoGrid.Children.Add(disciplineTextBlock);

            #endregion
            #region Комментарии

            ContentControl comments = CreateControl(commentTemplate, CreateCommentsToolTip(commentsList), 40);
            comments.Name = "GridComments";
            comments.HorizontalAlignment = HorizontalAlignment.Center;
            comments.SetGridPosition(1, 2);
            infoGrid.Children.Add(comments);

            #endregion
            #region Остальные иконки

            Grid others = new();
            others.VerticalAlignment = VerticalAlignment.Center;
            others.HorizontalAlignment = HorizontalAlignment.Center;

            ColumnDefinition col1 = new();
            ColumnDefinition col2 = new();
            ColumnDefinition col3 = new();
            ColumnDefinition col4 = new();

            col1.Width = new GridLength(75);
            col2.Width = new GridLength(75);
            col3.Width = new GridLength(75);
            col4.Width = new GridLength(75);

            others.ColumnDefinitions.Add(new ColumnDefinition());
            others.ColumnDefinitions.Add(col1);
            others.ColumnDefinitions.Add(col2);
            others.ColumnDefinitions.Add(col3);
            others.ColumnDefinitions.Add(col4);
            others.ColumnDefinitions.Add(new ColumnDefinition());

            others.SetGridPosition(2, 1, columnSpan: 2);

            ContentControl teacherControl = CreateControl(teacherTemplate, CreateToolTip(lm.Teacher?.FullName), 30);
            ContentControl audienceControl = CreateControl(doorTemplate, CreateToolTip(lm.Audience?.NumberAudience), 30);
            ContentControl disciplineControl = CreateControl(bookTemplate, CreateToolTip(lm.AcademicDiscipline?.Title), 45);
            ContentControl groupControl = CreateControl(groupTemplate, CreateToolTip(lm.Group?.Group), 40);

            teacherControl.SetGridPosition(0, 1);
            audienceControl.SetGridPosition(0, 2);
            disciplineControl.SetGridPosition(0, 3);
            groupControl.SetGridPosition(0, 4);

            others.Children.Add(teacherControl);
            others.Children.Add(audienceControl);
            others.Children.Add(disciplineControl);
            others.Children.Add(groupControl);

            infoGrid.Children.Add(others);

            #endregion
            return infoGrid;
        }
        /// <summary>
        /// Маленькая сетка
        /// </summary>
        /// <param name="card">Карточка</param>
        /// <returns>Сетка</returns>
        private Grid CreateCardSmallInfoGrid(Card card, LessonsModel lm, List<CommentModel> commentsList)
        {
            #region Объявление сетки

            Grid infoGrid = new();
            infoGrid.Name = "SmallInfo";

            #endregion
            #region Разметка

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

            #endregion
            #region Рамки

            Border border = new();
            border.Name = "SmallGridBorder";
            border.CornerRadius = new CornerRadius(5);
            border.Background = card.Background;
            border.SetGridPosition(0, 0, 3, 5);

            Border innerBorder = new();
            innerBorder.CornerRadius = new CornerRadius(5);
            innerBorder.Background = innerBrush;
            innerBorder.SetGridPosition(1, 1, columnSpan: 2);

            infoGrid.Children.Add(border);
            infoGrid.Children.Add(innerBorder);

            #endregion
            #region Название дисциплины

            ContentControl classType;
            if (string.IsNullOrEmpty(lm.CanceledReason))
            {
                classType = CreateControl(classTypeTemplate, null, 25);
                classType.Margin = new Thickness(15, 0, 0, 0);
                classType.SetGridPosition(1, 1);
                infoGrid.Children.Add(classType);
            }
            else
            {
                classType = CreateControl(cancelTemplate, null, 25);
                classType.Margin = new Thickness(15, 0, 0, 0);
                classType.Foreground = falseOrErrorBrush;
                classType.SetGridPosition(1, 1);

                ToolTip cancelTip = CreateToolTip("Занятие было отменено по причине:\r\n" + lm.CanceledReason);
                classType.ToolTip = cancelTip;

                infoGrid.Children.Add(classType);
            }

            TextBlock disciplineTextBlock = CreateTextBlock(lm.TypeClass?.Title);
            disciplineTextBlock.SetGridPosition(1, 2);
            infoGrid.Children.Add(disciplineTextBlock);

            #endregion
            #region Иконки с тултипами

            StackPanel controlsStack = new();
            controlsStack.Name = "IconsStack";
            controlsStack.Orientation = Orientation.Horizontal;
            controlsStack.HorizontalAlignment = HorizontalAlignment.Center;
            controlsStack.VerticalAlignment = VerticalAlignment.Center;
            controlsStack.Margin = new Thickness(10, 0, 0, 0);
            controlsStack.SetGridPosition(1, 3);

            #region Преподаватель
            controlsStack.Children.Add(CreateControl(teacherTemplate, CreateToolTip(lm.Teacher?.FullName), 30));
            #endregion
            #region Аудитория
            controlsStack.Children.Add(CreateControl(doorTemplate, CreateToolTip(lm.Audience?.NumberAudience), 30));
            #endregion
            #region Дисциплина
            controlsStack.Children.Add(CreateControl(bookTemplate, CreateToolTip(lm.AcademicDiscipline?.Title), 45));
            #endregion
            #region Группа
            controlsStack.Children.Add(CreateControl(groupTemplate, CreateToolTip(lm.Group?.Group), 40));
            #endregion
            #region Комменты
            ContentControl commentsControl = CreateControl(commentTemplate, CreateCommentsToolTip(commentsList), 40);
            commentsControl.Name = "GridComments";
            controlsStack.Children.Add(commentsControl);
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
            subGrid.Name = "TopGrid";
            if (memory.GetItem<ScheduleConfig>(nameof(ScheduleConfig)).CanMoveOrResizeScheduleCards)
            {
                #region Разметка

                ColumnDefinition smallColumn1 = new();
                smallColumn1.Width = new GridLength(15);
                ColumnDefinition smallColumn2 = new();
                smallColumn2.Width = new GridLength(15);

                subGrid.ColumnDefinitions.Add(smallColumn1);
                subGrid.ColumnDefinitions.Add(new ColumnDefinition());
                subGrid.ColumnDefinitions.Add(smallColumn2);

                RowDefinition smallRow = new();
                smallRow.Height = new GridLength(15);
                RowDefinition endRow = new();
                endRow.Height = new GridLength(15);

                subGrid.RowDefinitions.Add(smallRow);
                subGrid.RowDefinitions.Add(new RowDefinition());
                subGrid.RowDefinitions.Add(endRow);

                #endregion
                #region Прозрачная рамка

                Border transparent_border_up = new();
                transparent_border_up.Background = new SolidColorBrush(Colors.Transparent);
                transparent_border_up.SetGridPosition(0, 0, columnSpan: 3);

                Border transparent_border_down = new();
                transparent_border_down.Background = new SolidColorBrush(Colors.Transparent);
                transparent_border_down.SetGridPosition(2, 0, columnSpan: 3);

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
                foreach (object key in svg.Keys)
                {
                    if (key.ToString() == "Move")
                    {
                        move_control.Template = (ControlTemplate)svg[key];
                    }
                }
                move_control.Foreground = elementFontBrush;
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
                foreach (object key in svg.Keys)
                {
                    if (key.ToString() == "Up")
                    {
                        up_arrow.Template = (ControlTemplate)svg[key];
                    }
                }
                up_arrow.Foreground = elementFontBrush;
                up_arrow.MouseDown += UpArrowMouseDown;
                up_arrow.SetGridPosition(0, 3);
                subGrid.Children.Add(up_arrow);

                #endregion
                #region Контрол увеличивающий вниз

                ContentControl down_arrow = new();
                down_arrow.Height = 12;
                down_arrow.Margin = new Thickness(3);
                down_arrow.HorizontalAlignment = HorizontalAlignment.Right;
                down_arrow.VerticalAlignment = VerticalAlignment.Bottom;
                down_arrow.Cursor = Cursors.SizeNS;
                foreach (object key in svg.Keys)
                {
                    if (key.ToString() == "Down")
                    {
                        down_arrow.Template = (ControlTemplate)svg[key];
                    }
                }
                down_arrow.Foreground = elementFontBrush;
                down_arrow.MouseDown += DownArrowMouseDown;
                down_arrow.SetGridPosition(3, 3);
                subGrid.Children.Add(down_arrow);

                #endregion
            }
            return subGrid;
        }

        #region Создание контролов

        private ContentControl CreateControl(ControlTemplate template, ToolTip tip, int size)
        {
            ContentControl control = new();
            control.Width = size;
            control.Margin = new Thickness(0, 0, 5, 0);
            control.Foreground = elementFontBrush;
            control.Template = template;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.VerticalAlignment = VerticalAlignment.Center;
            control.ToolTip = tip;
            return control;
        }
        private TextBlock CreateTextBlock(string message)
        {
            TextBlock txt = new();
            txt.Text = message;
            txt.Style = elementTextBlockStyle;
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.VerticalAlignment = VerticalAlignment.Center;
            return txt;
        }
        private ToolTip CreateToolTip(string message)
        {
            TextBlock textBlock = new();
            textBlock.Style = toolTipTextBlockStyle;
            textBlock.Text = message;

            ToolTip toolTip = new();
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            toolTip.Content = textBlock;

            return toolTip;
        }
        private ToolTip CreateCommentsToolTip(List<CommentModel> comments)
        {
            StackPanel controlsStack = new();
            controlsStack.Orientation = Orientation.Vertical;
            controlsStack.HorizontalAlignment = HorizontalAlignment.Center;
            controlsStack.VerticalAlignment = VerticalAlignment.Top;

            if (comments != null && comments.Count != 0)
            {
                int rangeLimit = 1;
                foreach (CommentModel com in comments)
                {
                    if (rangeLimit >= 6) break;

                    Border border = new();
                    border.Margin = new Thickness(3);
                    border.Background = innerBrush;

                    Border split = new();
                    split.Height = 1;
                    split.VerticalAlignment = VerticalAlignment.Bottom;
                    split.Margin = new Thickness(6, 0, 6, 0);
                    split.Opacity = 0.5;
                    split.Background = new SolidColorBrush(Colors.White);

                    TextBlock from = new();
                    from.Style = elementTextBlockStyle;
                    from.Margin = new Thickness(6);
                    from.Text = com.User.Surname + " " + com.User.Name;
                    from.HorizontalAlignment = HorizontalAlignment.Left;
                    from.VerticalAlignment = VerticalAlignment.Top;
                    from.FontSize = 11;

                    TextBlock time = new();
                    time.Style = elementTextBlockStyle;
                    time.Margin = new Thickness(6);
                    time.Text = com.Date.ToString();
                    time.HorizontalAlignment = HorizontalAlignment.Right;
                    time.VerticalAlignment = VerticalAlignment.Top;
                    time.FontSize = 11;

                    TextBlock msg = new();
                    msg.Style = elementTextBlockStyle;
                    msg.Margin = new Thickness(6);
                    msg.Text = com.Message;
                    msg.HorizontalAlignment = HorizontalAlignment.Stretch;
                    msg.VerticalAlignment = VerticalAlignment.Top;
                    msg.FontSize = 12;
                    msg.TextWrapping = TextWrapping.Wrap;

                    Grid gr = new();
                    gr.Uid = com.Id.ToString().ToUpper();
                    gr.Height = 70;
                    gr.Width = 330;

                    RowDefinition head = new();
                    head.Height = new GridLength(30);

                    gr.RowDefinitions.Add(head);
                    gr.RowDefinitions.Add(new RowDefinition());

                    Grid.SetRow(border, 0);
                    Grid.SetRow(msg, 1);
                    Grid.SetRowSpan(border, 3);

                    gr.Children.Add(border);
                    gr.Children.Add(split);
                    gr.Children.Add(from);
                    gr.Children.Add(time);
                    gr.Children.Add(msg);

                    controlsStack.Children.Add(gr);
                    rangeLimit++;
                }
            }
            else
            {
                TextBlock empty = new();
                empty.Style = simpleTextBlockStyle;
                empty.Margin = new Thickness(6);
                empty.Text = "Нет комментариев.";
                empty.HorizontalAlignment = HorizontalAlignment.Center;
                empty.VerticalAlignment = VerticalAlignment.Center;
                empty.FontSize = 11;

                controlsStack.Children.Add(empty);
            }

            ToolTip toolTip = new();
            toolTip.Width = 400;
            toolTip.Height = 400;
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            toolTip.Content = controlsStack;

            return toolTip;
        }

        #endregion
        #region Обновление комментов ивентом

        private void OnCardCommentsChanged(KeyValuePair<string, IEnumerable<CommentModel>> pair)
        {
            foreach (var ch in ItemsGrid.Children)
            {
                if (ch is Card card && card.Uid.ToUpper() == pair.Key.ToUpper())
                {
                    Grid content = card.Content as Grid;
                    foreach (var sub_ch in content.Children)
                    {
                        if (sub_ch is Grid sub_grid)
                        {
                            foreach (var infoGrid in sub_grid.Children)
                            {
                                if (infoGrid is ContentControl cc && cc.Name == "GridComments")
                                {
                                    var tip = CreateCommentsToolTip(pair.Value.ToList());
                                    cc.ToolTip = tip;
                                }
                                if (infoGrid is StackPanel sp && sp.Name == "IconsStack")
                                {
                                    foreach (var sp_ch in sp.Children)
                                    {
                                        if (sp_ch is ContentControl sp_cc && sp_cc.Name == "GridComments")
                                        {
                                            var tip = CreateCommentsToolTip(pair.Value.ToList());
                                            sp_cc.ToolTip = tip;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
        #endregion
        #region Метод выбора карточки

        private void CardMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Card card)
            {
                if (card.Effect == null)
                {
                    SetEffect(card, true, true);
                    foreach (object ch in ItemsGrid.Children)
                    {
                        if (ch is Card other && other.Uid != card.Uid)
                        {
                            SetEffect(other, false);
                        }
                    }
                }
                else
                {
                    SetEffect(card, false, true);
                }
            }
        }
        private void SetEffect(Card card, bool selected, bool sendEventMessage = false)
        {
            if (!selected) card.Effect = null;
            else card.Effect = new DropShadowEffect() { Color = Colors.Red, Opacity = 0.85, ShadowDepth = 0, BlurRadius = 20, Direction = 0 };
            if (sendEventMessage)
            {
                aggregator.GetEvent<CardSelectingEvent>().Publish(new KeyValuePair<string, bool>(card.Uid, selected));
            }
        }
        private void OnClearEffects()
        {
            foreach (object ch in ItemsGrid.Children)
            {
                if (ch is Card card)
                {
                    SetEffect(card, false, true);
                }
            }
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
                foreach (object child in mgr.Children)
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
            foreach (object ch in source.Children)
            {
                if (ch is Grid ch_gr && ch_gr.Name == "SmallInfo")
                {
                    foreach (object sub_ch in ch_gr.Children)
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
            Point position = e.GetPosition(ScheduleMarkup);
            object data = e.Data.GetData("HandyControl.Controls.Card");
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
            Point position = e.GetPosition(ScheduleMarkup);
            object data = e.Data.GetData("HandyControl.Controls.Card");
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
