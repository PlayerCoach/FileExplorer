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
using System.Windows.Media;
using FileExplorer.MVVM.ViewModel.FileOperations;
using TreeView = System.Windows.Controls.TreeView;
using TextBlock = System.Windows.Controls.TextBlock;

namespace FileExplorer.MVVM.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand OpenFileCommand { get; set; }
        public ICommand CloseAppCommand { get; set; }
        public ICommand ReadTagCommand { get; set; }  
        public ICommand DeleteFileCommand { get; set; }
        public ICommand CreateFileCommand { get; set; }

        private TreeViewItem? _selectedItem;

        private TreeViewItem? _rootDirectory;
        private TreeView FileTree;
        private TextBlock FilePreviewTextBlock;
        private FileOperator fileOperator = new FileOperator();

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

        public MainWindowViewModel(TreeView thisFileTree, TextBlock thisFilepreviewTextBlock)
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            CloseAppCommand = new RelayCommand(CloseApp, CanCloseApp);
            ReadTagCommand = new RelayCommand(ReadTag,CanReadTag);
            DeleteFileCommand = new RelayCommand(DeleteFile, CanDeleteFile);
            CreateFileCommand = new RelayCommand(CreateFile, CanCreateFile);
            FileTree = thisFileTree;
            FilePreviewTextBlock = thisFilepreviewTextBlock;
            FileTree.PreviewMouseRightButtonDown += TreeView_PreviewMouseRightButtonDown;
            FileTree.PreviewMouseLeftButtonDown += TreeView_PreviewMouseLeftButtonDown; // Add this line
        }



        private bool CanCreateFile(object obj)
        {
            // Check if the selected item is a directory
            if (_selectedItem != null && Directory.Exists(_selectedItem.Tag as string))
            {
                return true;
            }
            return false;
        }

        private void CreateFile(object obj)
        {
            // Open the CreateFileForm window
            CreateFileForm createFileForm = new CreateFileForm(_selectedItem.Tag as string);
            createFileForm.ShowDialog();
        }


        private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement originalSource)
            {
                TreeViewItem item = GetNearestContainer(originalSource);
                if (item == null && _selectedItem != null)
                {
                    // If the clicked element is not a TreeViewItem, clear the selection
                    _selectedItem.IsSelected = false;
                    _selectedItem = null;
                }
                else if (item != null)
                {
                    _selectedItem = item;
                    FilePreviewTextBlock.Text = fileOperator.ReadFile(item.Tag as string);

                }
            }
        }

        private void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement originalSource)
            {
                TreeViewItem item = GetNearestContainer(originalSource);
                if (item != null)
                {
                    item.IsSelected = true; 
                    ShowContextMenu(item);
                    e.Handled = true;
                    _selectedItem = item;
                }
            }
        }


        private TreeViewItem GetNearestContainer(FrameworkElement element)
        {
            // Walk up the tree and stop when reaching a TreeViewItem
            while (element != null && !(element is TreeViewItem))
            {
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }
            return element as TreeViewItem;
        }

        private void ShowContextMenu(TreeViewItem item)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem showPathItem = new MenuItem
            {
                Header = "Show Path",
                Command = ReadTagCommand,
                CommandParameter = item.Tag

            };

            MenuItem deleteFileItem = new MenuItem
            {
                Header = "Delete File",
                Command = DeleteFileCommand,
                CommandParameter = item.Tag
            };

            MenuItem createFileItem = new MenuItem
            {
                Header = "Create File",
                Command = CreateFileCommand,
                CommandParameter = item.Tag
            };

            contextMenu.Items.Add(showPathItem);
            contextMenu.Items.Add(deleteFileItem);
            contextMenu.Items.Add(createFileItem);

            contextMenu.IsOpen = true;
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
            FileTree.Items.Clear();
            RootDirectory = mainWindowModel.GetFolderTree(dlg.SelectedPath);
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
        private bool CanReadTag(object obj)
        {
            return true;
            
        }

        private void ReadTag(object tag)
        {
            System.Diagnostics.Debug.WriteLine($"Tag: {tag}");
        }

        private bool CanDeleteFile(object tag)
        {
            return true;
        }

        private void DeleteFile(object tag)
        {
            mainWindowModel.DeleteItem(tag as string);
            FileTree.Items.Clear();
            FileTree.Items.Add(mainWindowModel.GetTreeItem());
        }
    }
}
