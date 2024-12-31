using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBrowser.SourceDownloader.Archive
{
    public class ZipArchive :IZipArchive
    {
        public string Extract(string zipFilePath)
        {
            var fileInfo = new FileInfo(zipFilePath);
            var fileName = fileInfo.Name.Replace(".zip", "");
            string extractToDirectory = fileInfo.DirectoryName;
            string finalDirectoryName = @$"{extractToDirectory}\{fileName}";

            try
            {
                // Ensure the base extraction directory exists
                Directory.CreateDirectory(extractToDirectory);

                Console.WriteLine($"Extracting {zipFilePath} to {extractToDirectory}...");
                ExtractZipFile(zipFilePath, extractToDirectory);

                // Rename the extracted folder
                string extractedFolderName = Directory.GetDirectories(extractToDirectory).First(x => x.Contains(fileName));
                if (Directory.Exists(finalDirectoryName))
                {
                    Directory.Delete(finalDirectoryName, true);
                }

                Directory.Move(extractedFolderName, finalDirectoryName);

                Console.WriteLine($"Extraction done: {finalDirectoryName}");
                return finalDirectoryName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        static void ExtractZipFile(string zipFilePath, string extractToDirectory)
        {
            ZipFile.ExtractToDirectory(zipFilePath, extractToDirectory, overwriteFiles: true);
        }
    }
}
