using SourceBrowser.Java.Jxr;
using SourceBrowser.SourceDownloader.Archive;
using SourceBrowser.SourceDownloader.Github;
using SourceBrowser.SourceDownloader.Interfaces;

string localWorkingDirectory = "C:\\temp\\source";

Console.WriteLine("Enter Token to access Github - ");
string? token = Console.ReadLine();

var repositories = new List<string>
{
    "https://github.com/dsmalik/FolderSyncUtility",
    "https://github.com/in28minutes/master-spring-and-spring-boot.git"
};

ISourceCodeProvider sourceCodeProvider = new GithubSourceProvider(token);
IZipArchive zipArchive = new ZipArchive();

if (repositories.Count == 0)
{
    Console.WriteLine("No repositories to download");
    return;
}

foreach (var repository in repositories)
{
    await GetSourceCodeAsync(sourceCodeProvider, repository);
}

async Task GetSourceCodeAsync(ISourceCodeProvider sourceCodeProvider, string repositoryPath)
{
    Console.WriteLine($"Fetching source code from {repositoryPath}");

    try
    {
        var targetZipLocation = await sourceCodeProvider.FetchSourceCodeAsync(repositoryPath, "main", localWorkingDirectory);
        var outputFolder = zipArchive.Extract(targetZipLocation);

        var consoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        // Read the pom.xml file
        JxrPluginHelper.AddPluginToPom(outputFolder);
        Console.ForegroundColor = consoleColor;
    }
    catch (Exception)
    {
        Console.WriteLine($"Failed to download source code from {repositoryPath}");
        throw;
    }
}