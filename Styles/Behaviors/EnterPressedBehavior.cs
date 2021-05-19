using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace Styles.Behaviors
{
    public class EnterPressedBehavior
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                                                typeof(DelegateCommand),
                                                typeof(EnterPressedBehavior),
                                                new UIPropertyMetadata(CommandChanged));
        public static void SetCommand(DependencyObject target, DelegateCommand value) => target.SetValue(CommandProperty, value);
        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is FrameworkElement fe)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    fe.KeyDown += OnEnterPressed;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    fe.KeyDown -= OnEnterPressed;
                }
            }
        }
        private static void OnEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            FrameworkElement control = sender as FrameworkElement;
            DelegateCommand command = (DelegateCommand)control.GetValue(CommandProperty);
            command.Execute();
        }
    }
}
