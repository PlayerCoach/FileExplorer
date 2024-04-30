using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileExplorer.MVVM.Model
{
    class MainWindowModel
    {
        private ObservableCollection<TreeViewItem> TreeItems  = new ObservableCollection<TreeViewItem>();
        private string? mainFolderPath; // usefull for restroing the tree after deleting a file or folder

        public TreeViewItem GetFolderTree(string path)
        {
            TreeItems.Clear();
            TreeItems.Add(createTree(path));
            mainFolderPath = path;
            return TreeItems[0];
        }
        private TreeViewItem createTree(string path)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = System.IO.Path.GetFileName(path);
            item.Tag = path;

            foreach (string file in Directory.GetFiles(path))
            {
                TreeViewItem subItem = new TreeViewItem();
                subItem.Header = System.IO.Path.GetFileName(file);
                subItem.Tag = file;
                item.Items.Add(subItem);
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                item.Items.Add(createTree(directory));
            }
            return item;

        }
        public void DeleteItem(string path)
        {
            if (mainFolderPath == null)
            {
                System.Diagnostics.Debug.WriteLine("No folder selected.");
                return;
            }
            if (path == mainFolderPath)
            {
                System.Diagnostics.Debug.WriteLine("Cannot delete root folder.");
                return;
            }
            if (File.Exists(path))
            {
                
                DeleteFile(path);
                TreeItems.Clear();
                TreeItems.Add(createTree(mainFolderPath));
            }
            else if (Directory.Exists(path))
            {

                DeleteFolder(path);
                TreeItems.Clear();
                TreeItems.Add(createTree(mainFolderPath));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Path does not exist.");
            }
        }

        static void DeleteFile(string filePath)
        {
            //if file has read only attribute, remove it
            if ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(filePath, File.GetAttributes(filePath) & ~FileAttributes.ReadOnly);
            }

            try
            {
                File.Delete(filePath);
                System.Diagnostics.Debug.WriteLine("File deleted successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            }
        }

        static void DeleteFolder(string folderPath)
        {
            try
            {
                Directory.Delete(folderPath, true);
                System.Diagnostics.Debug.WriteLine("Folder and its contents deleted successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred while deleting the folder: {ex.Message}");
            }
        }

        public TreeViewItem GetTreeItem()
        {
            return TreeItems[0];
        }

    }
}
