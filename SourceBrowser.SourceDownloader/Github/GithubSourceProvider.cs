using Octokit;
using SourceBrowser.SourceDownloader.Interfaces;

namespace SourceBrowser.SourceDownloader.Github
{
    public class GithubSourceProvider : ISourceCodeProvider
    {
        private GitHubClient _gitHubClient = new GitHubClient(new ProductHeaderValue("RepositoryDownloader"));
        private string _token;

        public GithubSourceProvider(string token)
        {
            _token = token;
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to download source from Github.");
            }

            _gitHubClient.Credentials = new Credentials(token);
        }

        public async Task<string> FetchSourceCodeAsync(string repository, string branch, string destination)
        {
            var repoInfo = GetRepoInfoFromUrl(repository);
            
            var githubRepository = await _gitHubClient.Repository.Get(repoInfo.Item1, repoInfo.Item2);

            var archiveLink = await _gitHubClient.Repository.Content.GetArchive(githubRepository.Id, ArchiveFormat.Zipball);

            var targetFilePath = Path.Combine(destination, repoInfo.Item2 + ".zip");
            using (var fileStream = new FileStream(targetFilePath, System.IO.FileMode.OpenOrCreate, FileAccess.Write))
            {
                fileStream.Write(archiveLink, 0, archiveLink.Length);
            }

            return targetFilePath;
        }

        private Tuple<string, string> GetRepoInfoFromUrl(string repository)
        {
            var repoUrl = new Uri(repository);
            var repoPath = repoUrl.AbsolutePath;
            var repoPathParts = repoPath.Split('/');
            return Tuple.Create(repoPathParts[1], repoPathParts[2].Replace(".git", ""));
        }
    }
}
