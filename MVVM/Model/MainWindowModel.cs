using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileExplorer.MVVM.Model
{
    class MainWindowModel
    {
        public TreeViewItem createTree(string path)
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

        public void printTreeViewToDebug(TreeViewItem item)
        {
            System.Diagnostics.Debug.WriteLine("********** " + item.Header + " **********");
            foreach (TreeViewItem subItem in item.Items)
            {
                printTreeViewToDebug(subItem);
            }
        }
    }
}
