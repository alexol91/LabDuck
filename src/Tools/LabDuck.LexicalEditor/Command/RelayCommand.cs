using System;
using System.Windows.Input;

namespace LabDuck.LexicalEditor.Command
{
    public class RelayCommand : ICommand
    {
        private Action action;
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            CanExecuteChanged = null;
            CanExecuteChanged?.Invoke(this, null);
            return true;
        }

        public void Execute(object parameter)
        {
            action?.Invoke();
        }
    }
}
