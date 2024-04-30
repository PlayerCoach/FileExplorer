using FileExplorer.MVVM.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileExplorer.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(FileTree);
            this.DataContext = mainWindowViewModel;
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



    }
}