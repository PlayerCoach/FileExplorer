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

namespace FileExplorer.MVVM.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand OpenFileCommand { get; set; }
        public ICommand CloseAppCommand { get; set; }


        private TreeViewItem? _rootDirectory;

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

    

        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            CloseAppCommand = new RelayCommand(CloseApp, CanCloseApp);
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
        }


        private bool CanCloseApp(object obj)
        {
            return true;
        }

        private void CloseApp(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }
        }
       
}
