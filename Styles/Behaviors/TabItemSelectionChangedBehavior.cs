using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace Styles.Behaviors
{
    public class TabItemSelectionChangedBehavior
    {
        public static DependencyProperty CommandProperty =
              DependencyProperty.RegisterAttached("Command",
                                                  typeof(DelegateCommand),
                                                  typeof(TabItemSelectionChangedBehavior),
                                                  new UIPropertyMetadata(CommandChanged));

        public static void SetCommand(DependencyObject target, DelegateCommand value) => target.SetValue(CommandProperty, value);
        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is TabControl tc)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    tc.SelectionChanged += OnSelectionChanged;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    tc.SelectionChanged -= OnSelectionChanged;
                }
            }
        }
        private static void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            TabControl control = sender as TabControl;
            DelegateCommand command = (DelegateCommand)control.GetValue(CommandProperty);
            command.Execute();
        }
    }
}
