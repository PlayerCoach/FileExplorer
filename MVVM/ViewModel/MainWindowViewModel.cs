using FileExplorer.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using FileExplorer.MVVM.View;
using FileExplorer.MVVM.Model;
using System.Windows;
using System.Diagnostics;

namespace FileExplorer.MVVM.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand OpenFileCommand { get; set; }
        public ICommand CloseAppCommand { get; set; }
        public ICommand ReadTagCommand { get; set; }  // Add this line

        private TreeViewItem? _rootDirectory;
        private string? header = null;
        private System.Windows.Controls.TreeView FileTree;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TreeViewItem? RootDirectory
        {
            get { return _rootDirectory; }
            set
            {
                _rootDirectory = value;
                OnPropertyChanged();
            }
        }

        private MainWindowModel mainWindowModel = new MainWindowModel();

        public MainWindowViewModel(System.Windows.Controls.TreeView thisFileTree)
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            CloseAppCommand = new RelayCommand(CloseApp, CanCloseApp);
            ReadTagCommand = new RelayCommand(ReadTag, _ => true);  // Add this line
            FileTree = thisFileTree;
            FileTree.SelectedItemChanged += TreeView_SelectedItemChanged; // Add this line
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item)
            {
                // Assuming the Tag property is set and is of type string
                string tagValue = item.Tag as string;
                System.Diagnostics.Debug.WriteLine(tagValue); // Print the Tag value
            }
        }


        private bool CanOpenFile(object obj)
        {
            return true;
        }

        private void OpenFile(object obj)
        {
            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
            dlg.ShowDialog();
            if (string.IsNullOrEmpty(dlg.SelectedPath))
            {
                return;
            }

            RootDirectory = mainWindowModel.createTree(dlg.SelectedPath);
            header = System.IO.Path.GetFileName(dlg.SelectedPath);
            RootDirectory.MouseLeftButtonDown += treeItem_MouseLeftButtonUp;
            FileTree.Items.Add(RootDirectory);
        }

        private bool CanCloseApp(object obj)
        {
            return true;
        }

        private void CloseApp(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ReadTag(object tag)
        {
            Debug.WriteLine($"Tag: {tag}");
        }

        private void treeItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CHUJ CI NA PIZDE"); // Print the Tag value
            if (sender is TreeViewItem item)
            {
                // Assuming the Tag property is set and is of type string
                string tagValue = item.Tag as string;

            }
        }
    }
}
