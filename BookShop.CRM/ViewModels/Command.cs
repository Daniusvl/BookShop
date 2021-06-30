using System;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Func<object, bool> canExecute;
        private Action<object> execute;

        public Command(Func<object, bool> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
