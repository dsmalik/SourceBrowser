namespace SourceBrowser.SourceDownloader.Interfaces
{
    public interface ISourceCodeProvider
    {
        Task<string> FetchSourceCodeAsync(string repository, string branch, string destination);
    }
}
