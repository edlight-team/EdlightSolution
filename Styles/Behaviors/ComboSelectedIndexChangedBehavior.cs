using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace Styles.Behaviors
{
    public class ComboSelectedIndexChangedBehavior
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                                                typeof(DelegateCommand),
                                                typeof(ComboSelectedIndexChangedBehavior),
                                                new UIPropertyMetadata(CommandChanged));
        public static void SetCommand(DependencyObject target, DelegateCommand value) => target.SetValue(CommandProperty, value);
        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is ComboBox cb)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    cb.SelectionChanged += OnSelectionChanged;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    cb.SelectionChanged -= OnSelectionChanged;
                }
            }
        }
        private static void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            DelegateCommand command = (DelegateCommand)control.GetValue(CommandProperty);
            command.Execute();
        }
    }
}
