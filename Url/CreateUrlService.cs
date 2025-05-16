namespace YaushServer.Url
{
    public interface ICreateUrlService
    {
        Task CreateUrl(string url);
    }

    public class CreateUrlService(ILogger<CreateUrlService> logger, IUrlRepository urlRepository) : ICreateUrlService
    {
        private readonly ILogger<CreateUrlService> _logger = logger;
        private readonly IUrlRepository _urlRepository = urlRepository;

        public async Task CreateUrl(string url)
        {
            _logger.LogInformation("Create url for {host}", url);
            //await Task.Delay(1000);
            await _urlRepository.Get(url);
        }
    }
}
