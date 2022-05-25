using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiniTC.model;
using MiniTC.util;
using MiniTC.viewmodel.@base;

namespace MiniTC.viewmodel {
    public class TCViewModel : ViewModelBase {

        public TCPanel Panel1 { get; } = new();
        public TCPanel Panel2 { get; } = new();
        
        public int ActivePanel { get; set; } = -1;
        public string CopyText => ActivePanel == 1 ? "<< Copy" : "Copy >>";

        public ICommand Refresh1Command { get; set; }
        public ICommand Refresh2Command { get; set; }
        public ICommand CopyCommand { get; }
        public ICommand Selected1Command { get; }
        public ICommand Selected2Command { get; }

        private TCPanel p1 => ActivePanel == 0 ? Panel1 : Panel2;
        private TCPanel p2 => ActivePanel == 0 ? Panel2 : Panel1;

        public TCViewModel() {
            CopyCommand = new RelayCommand(
                _ => CopyFile(p1, p2),
                _ => CanCopy()
            );
            Selected1Command = new RelayCommand(_ => {
                ActivePanel = 0;
                var v = Panel1.Selected;
                Panel2.Selected = null;
                OnPropertyChanged("CopyText");
                OnPropertyChanged("Panel2");
                Panel1.Selected = v;
                OnPropertyChanged("Panel1");
            });
            Selected2Command = new RelayCommand(_ => {
                ActivePanel = 1;
                var v = Panel2.Selected;
                Panel1.Selected = null;
                OnPropertyChanged("CopyText");
                OnPropertyChanged("Panel1");
                Panel2.Selected = v;
                OnPropertyChanged("Panel2");
            });
        }

        private bool CanCopy() {
            if (ActivePanel < 0) return false;
            return p1.FilePath != null && p2.Path != null && p1.Selected != null/* &&
                   p1.FilePath != p2.Path + p1.Selected.Name*/;
        }

        private void CopyFile(TCPanel panel1, TCPanel panel2) {
            CopyFile(panel1.FilePath, panel2.Path, panel1.Selected);
        }

        private void CopyFile(string from, string to, FileEntry file) {
            var target = to + file.Name;
            var overwrite = file.IsDirectory ? Directory.Exists(target) : File.Exists(target);

            Debug.WriteLine($"copy{(file.IsDirectory ? " directory" : "")} {from} -> {to}");

            if (file.IsDirectory) {
                if (overwrite) {
                    var box = MessageBox.Show("Folder już istnieje, czy chcesz nadpisać jego zawartość?",
                        "Nadpisywanie folderu",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                    if (box == MessageBoxResult.Cancel) return;
                    overwrite = box == MessageBoxResult.Yes;
                }

                IO.CopyFilesRecursively(from, target, overwrite);

            } else {
                if (overwrite && MessageBox.Show("Plik już istnieje, czy chcesz go zastąpić?", "Nadpisywanie pliku",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                    return;

                File.Copy(from, target, overwrite);
            }

            Debug.WriteLine($"copied{(file.IsDirectory ? " directory" : "")} {from} -> {to}, overwrite: {overwrite}");

            Refresh1Command.Execute(null);
            Refresh2Command.Execute(null);
        }
    }
}