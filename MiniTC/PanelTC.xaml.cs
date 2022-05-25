using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiniTC.model;
using MiniTC.viewmodel.@base;

namespace MiniTC {
    public partial class PanelTC {

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            "Path",
            typeof(string),
            typeof(PanelTC),
            new FrameworkPropertyMetadata()
        );

        public static readonly DependencyProperty DriveItemsProperty = DependencyProperty.Register(
            "DriveItems",
            typeof(string[]),
            typeof(PanelTC),
            new FrameworkPropertyMetadata()
        );

        public static readonly DependencyProperty DriveSelectedProperty = DependencyProperty.Register(
            "DriveSelected",
            typeof(string),
            typeof(PanelTC),
            new FrameworkPropertyMetadata()
        );

        public static readonly DependencyProperty ListItemsProperty = DependencyProperty.Register(
            "ListItems",
            typeof(List<FileEntry>),
            typeof(PanelTC),
            new FrameworkPropertyMetadata()
        );

        public static readonly DependencyProperty ListSelectedProperty = DependencyProperty.Register(
            "ListSelected",
            typeof(FileEntry),
            typeof(PanelTC),
            new FrameworkPropertyMetadata()
        );

        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register(
            "RefreshCommand",
            typeof(ICommand),
            typeof(PanelTC),
            new FrameworkPropertyMetadata()
        );
        
        public static readonly RoutedEvent ItemSelectedEvent = EventManager.RegisterRoutedEvent(
            "ListItemSelected",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PanelTC)
        );

        public event RoutedEventHandler ItemSelected {
            add => AddHandler(ItemSelectedEvent, value);
            remove => RemoveHandler(ItemSelectedEvent, value);
        }

        private void RaiseItemSelected() {
            RaiseEvent(new RoutedEventArgs(ItemSelectedEvent));
        }

        public string Path {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public string[] DriveItems {
            get => (string[])GetValue(DriveItemsProperty);
            set => SetValue(DriveItemsProperty, value);
        }

        public string DriveSelected {
            get => (string)GetValue(DriveSelectedProperty);
            set => SetValue(DriveSelectedProperty, value);
        }

        public List<FileEntry> ListItems {
            get => (List<FileEntry>)GetValue(ListItemsProperty);
            set => SetValue(ListItemsProperty, value);
        }

        public FileEntry ListSelected {
            get => (FileEntry)GetValue(ListSelectedProperty);
            set => SetValue(ListSelectedProperty, value);
        }

        public ICommand RefreshCommand {
            get => (ICommand)GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }

        public PanelTC() {
            InitializeComponent();
        }
        
        private void List_OnLoaded(object sender, RoutedEventArgs e) {
            RefreshCommand = new RelayCommand(_ => RefreshList());
        }
        
        private void Drive_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            Path = DriveSelected;
            RefreshList();
        }

        private void Drive_OnDropDownOpened(object sender, EventArgs e) {
            DriveItems = Directory.GetLogicalDrives();
        }

        private void List_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var sep = System.IO.Path.DirectorySeparatorChar;
            if (sender is not ListBoxItem item) return;
            if (item.Content is not FileEntry entry) return;
            if (entry.IsFile) return;

            Path += entry.Name + sep;
            RefreshList();
        }

        private void RefreshList() {
            Path = System.IO.Path.GetFullPath(Path);

            var sep = System.IO.Path.DirectorySeparatorChar;
            var dirs = Directory.GetDirectories(Path);
            var files = Directory.GetFiles(Path);

            var list = new List<FileEntry>();
            if (Path != DriveSelected)
                list.Add(FileEntry.Parent);

            list.AddRange(dirs.Select(p => new FileEntry(p.Split(sep).Last(), true)));
            list.AddRange(files.Select(p => new FileEntry(p.Split(sep).Last(), false)));

            ListItems = list;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            RaiseItemSelected();
        }
    }
}