using System;
using System.Windows.Input;

namespace MiniTC.viewmodel.@base {
    public class RelayCommand<T> : ICommand where T : class {

        private readonly Action<T> _action;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> action, Predicate<T> canExecute = null) {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute?.Invoke(parameter as T) ?? true;
        }

        public void Execute(object parameter) {
            _action(parameter as T);
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

    public class RelayCommand : RelayCommand<object> {
        public RelayCommand(Action<object> action, Predicate<object> canExecute = null) : base(action, canExecute) {
        }
    }
}