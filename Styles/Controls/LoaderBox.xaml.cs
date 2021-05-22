using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Styles.Controls
{
    /// <summary>
    /// Логика взаимодействия для LoaderBox.xaml
    /// </summary>
    public partial class LoaderBox : UserControl
    {
        #region dependepcy fields

        private static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(LoaderBox), new PropertyMetadata(default(bool), OnActiveChanged)
           );
        private static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(LoaderBox), new PropertyMetadata(default(string))
           );

        #endregion
        #region dependency props

        /// <summary>
        /// Статус loader'а
        /// </summary>
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        /// <summary>
        /// Информация
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        #endregion
        #region property changed
        private static void OnActiveChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is LoaderBox lb)
            {
                DoubleAnimation opacityAnimation = new() { Duration = TimeSpan.FromMilliseconds(200) };
                if (lb.IsActive)
                {
                    lb.Visibility = Visibility.Visible;
                    opacityAnimation.From = 0;
                    opacityAnimation.To = 1;
                }
                else
                {
                    opacityAnimation.From = 1;
                    opacityAnimation.To = 0;
                    opacityAnimation.Completed += (s, e) => { lb.Visibility = Visibility.Hidden; };
                }
                lb.BeginAnimation(OpacityProperty, opacityAnimation);
            }
        }
        #endregion
        public LoaderBox()
        {
            InitializeComponent();
            Opacity = 0;
        }
    }
}
