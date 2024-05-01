using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileExplorer.MVVM.ViewModel.FileOperations
{
    internal class FileOperator
    {
        public FileOperator() { }
        public string ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileContent) && IsFileReadable(filePath))
                {
                    return fileContent;
                }
                else
                {
                    return "No available preview for this file";
                }
            }
            else
            {
                return "No available preview for this file";
            }
        }

        private static bool IsFileReadable(string filePath)
        {
           
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();

                string[] textExtensions = { ".txt", ".log", ".md", ".xml", ".json" };

                string[] codeExtensions = { ".c", ".cpp", ".java", ".py", ".cs", ".js", ".html", ".css" };

                if (Array.Exists(textExtensions, e => e == extension) ||
                    Array.Exists(codeExtensions, e => e == extension))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
  
    }
}
