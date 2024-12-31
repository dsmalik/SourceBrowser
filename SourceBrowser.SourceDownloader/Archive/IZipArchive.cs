using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBrowser.SourceDownloader.Archive
{
    public interface IZipArchive
    {
        string Extract(string zipFilePath);
    }
}
