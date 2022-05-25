using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiniTC.viewmodel.@base {
    public abstract class ViewModelBase : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}