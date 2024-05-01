using FileExplorer.Commands;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace FileExplorer.MVVM.ViewModel
{
    internal class CreateFileFormViewModel
    {
        private string path;
        public string FileName { get; set; }
        public bool IsFile { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsArchive { get; set; }
        public bool IsHidden { get; set; }
        public bool IsSystem { get; set; }

        public ICommand CreateCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public CreateFileFormViewModel(string path)
        {
            this.path = path;
            CreateCommand = new RelayCommand(CreateFile,_ => true);
            CancelCommand = new RelayCommand(Cancel, _ => true);
        }

        private void CreateFile(object parameter)
        {
            if(CheckIfValidFileName())
            {
                string fullPath = Path.Combine(path, FileName);
                if (IsFile)
                {
                    File.Create(fullPath);
                }
                else
                {
                    Directory.CreateDirectory(fullPath);
                }

                FileAttributes attributes = File.GetAttributes(fullPath);
                if (IsReadOnly) attributes |= FileAttributes.ReadOnly;
                if (IsArchive) attributes |= FileAttributes.Archive;
                if (IsHidden) attributes |= FileAttributes.Hidden;
                if (IsSystem) attributes |= FileAttributes.System;
                File.SetAttributes(fullPath, attributes);
                Cancel(parameter);
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid file name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            try
            {
                Window window = parameter as Window;
                if (window != null)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An error occurred while closing the window: " + ex.Message);
            }
        }

        private bool CheckIfValidFileName()
        {

        string pattern = @"^[a-zA-Z0-9_~-]{1,8}\.(txt|php|html)$";
        return Regex.IsMatch(FileName, pattern);

        }

}
}
