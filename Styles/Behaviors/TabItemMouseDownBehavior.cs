using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace Styles.Behaviors
{
    public class TabItemMouseDownBehavior
    {
        public static DependencyProperty CommandProperty =
              DependencyProperty.RegisterAttached("Command",
                                                  typeof(DelegateCommand),
                                                  typeof(TabItemMouseDownBehavior),
                                                  new UIPropertyMetadata(CommandChanged));

        public static void SetCommand(DependencyObject target, DelegateCommand value) => target.SetValue(CommandProperty, value);
        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is TabItem ti)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    ti.MouseDown += OnMouseDown;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    ti.MouseDown -= OnMouseDown;
                }
            }
        }
        private static void OnMouseDown(object sender, RoutedEventArgs e)
        {
            TabItem control = sender as TabItem;
            DelegateCommand command = (DelegateCommand)control.GetValue(CommandProperty);
            command.Execute();
        }
    }
}
