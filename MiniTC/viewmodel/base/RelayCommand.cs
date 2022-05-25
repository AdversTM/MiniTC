using System;
using System.Windows.Input;

namespace MiniTC.viewmodel.@base {
    public class RelayCommand : ICommand {

        private readonly Action<object> _action;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> action, Predicate<object> canExecute = null) {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter) {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add {
                if (_canExecute == null) return;
                CommandManager.RequerySuggested += value;
            }
            remove {
                if (_canExecute == null) return;
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}