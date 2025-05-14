namespace YaushServer.Url
{
    public interface ICreateUrlService
    {
        Task CreateUrl(string url);
    }

    public class CreateUrlService(ILogger<CreateUrlService> logger) : ICreateUrlService
    {
        private readonly ILogger<CreateUrlService> _logger = logger;

        public async Task CreateUrl(string url)
        {
            _logger.LogInformation("Create url for {host}", url);
            await Task.Delay(1000);
        }
    }
}
