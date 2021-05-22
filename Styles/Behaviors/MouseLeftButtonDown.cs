using System.Windows;
using System.Windows.Input;

namespace Styles.Behaviors
{
    public class MouseLeftButtonDown
    {
        public static DependencyProperty CommandProperty =
           DependencyProperty.RegisterAttached("Command",
                                               typeof(ICommand),
                                               typeof(MouseLeftButtonDown),
                                               new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseLeftButtonDown),
                                                new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value) => target.SetValue(CommandProperty, value);
        public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);
        public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);
        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is FrameworkElement control)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    control.MouseLeftButtonDown += OnMouseDown;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    control.MouseLeftButtonDown -= OnMouseDown;
                }
            }
        }
        private static void OnMouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement control)
            {
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                object commandParameter = control.GetValue(CommandParameterProperty);
                command.Execute(commandParameter);
            }
        }
    }
}
